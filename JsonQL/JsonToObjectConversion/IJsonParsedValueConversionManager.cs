// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using JsonQL.JsonObjects;
using JsonQL.JsonToObjectConversion.ConvertedObjectPath;
using JsonQL.JsonToObjectConversion.NullabilityCheck;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Represents a manager responsible for converting parsed JSON values into strongly-typed objects.
/// This interface defines the contract for handling the transformation of JSON data into various target types
/// while respecting type nullability and applying any specified conversion settings overrides.
/// </summary>
public interface IJsonParsedValueConversionManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parsedValue"></param>
    /// <param name="typeToConvertTo">TYpe to convert too.</param>
    /// <param name="convertedValueNullability">
    /// If the value is not null, specifies the nullability of returned value and might result in errors/warnings<br/>
    /// being reported if the returned value is null, or any item in returned collection items are null.<br/>
    /// 'Non-Nullable value not set' errors will be reported only if <see cref="ConversionErrorType.NonNullablePropertyNotSet"/> or<br/>
    /// <see cref="ConversionErrorType.NonNullableCollectionItemValueNotSet"/> are not configured with <see cref="ErrorReportingType.Ignore"/>.<br/>
    /// Note, this value only affects the returned value. Property values in returned objects (including collection item nullability)  are checked<br/>
    /// by using microsoft's nullability flag '?'.<br/>
    /// The following rules are used.<br/> 
    /// -If the returned value is not a collection, <paramref name="convertedValueNullability"/> should <br/>
    /// have a single value with <b>true</b> for nullable returned value and <b>false</b> otherwise.<br/>
    /// -If the returned type is a collection, such as <see cref="IReadOnlyList{T}"/>, then the first<br/>
    /// item specifies nullability of collection itself (<b>true</b> for nullable, <b>false</b> for non-nullable),<br/>
    /// the second value specifies the nullability of first level items, the third item<br/>
    /// specifies nullability of second level items (such as items in lists of lists), etc.<br/>
    /// For example if returned type is <b>IEnumerable&lt;IReadOnlyList&lt;IEmployee&gt;&gt; Employees {get; set}</b>,<br/>
    /// then [true, false, true] will result in implementation assuming that the returned value can be nullable,<br/>
    /// lists <b>IReadOnlyList&lt;IEmployee&gt;</b> in returned value are not nullable, and <b>IEmployee</b> items in <b>IReadOnlyList&lt;IEmployee&gt;</b> are nullable.
    /// </param> 
    /// <param name="jsonConversionSettingOverrides"></param>
    /// <returns></returns>
    IConversionResult<object?> Convert(IParsedValue parsedValue, Type typeToConvertTo, IReadOnlyList<bool>? convertedValueNullability,
        IJsonConversionSettingsOverrides? jsonConversionSettingOverrides = null);
}

/// <inheritdoc />
public class JsonParsedValueConversionManager : IJsonParsedValueConversionManager
{
    private readonly ISimpleJsonValueSerializer _simpleJsonValueSerializer;
    private readonly IValueNullabilityHelpers _valueNullabilityHelpers;

    private readonly ICollectionTypeHelpers _collectionTypeHelpers;
    private readonly IModelClassMapper _modelClassMapper;
    private readonly IModelClassInstanceCreator _modelClassInstanceCreator;
    private readonly IConvertedObjectPathFactory _convertedObjectPathFactory;
    private readonly IRootConvertedObjectPathElementFactory _rootConvertedObjectPathElementFactory;
    private readonly IPropertyNameConvertedObjectPathElementFactory _propertyNameConvertedObjectPathElementFactory;
    private readonly IIndexConvertedObjectPathElementFactory _indexConvertedObjectPathElementFactory;
    private readonly IJsonConversionSettingsWrapper _jsonConversionSettingsWrapper;

    private const string GeneralConversionError = "Conversion failed";

    public JsonParsedValueConversionManager(ISimpleJsonValueSerializer simpleJsonValueSerializer, IJsonConversionSettings jsonConversionSettings,
        IJsonConversionSettingsWrapperFactory jsonConversionSettingsWrapperFactory,
        IValueNullabilityHelpers valueNullabilityHelpers, ICollectionTypeHelpers collectionTypeHelpers,
        IModelClassMapper modelClassMapper, IModelClassInstanceCreator modelClassInstanceCreator,
        IConvertedObjectPathFactory convertedObjectPathFactory,
        IRootConvertedObjectPathElementFactory rootConvertedObjectPathElementFactory,
        IPropertyNameConvertedObjectPathElementFactory propertyNameConvertedObjectPathElementFactory,
        IIndexConvertedObjectPathElementFactory indexConvertedObjectPathElementFactory)
    {
        _simpleJsonValueSerializer = simpleJsonValueSerializer;
        _valueNullabilityHelpers = valueNullabilityHelpers;
        _collectionTypeHelpers = collectionTypeHelpers;
        _modelClassMapper = modelClassMapper;
        _modelClassInstanceCreator = modelClassInstanceCreator;
        _convertedObjectPathFactory = convertedObjectPathFactory;
        _rootConvertedObjectPathElementFactory = rootConvertedObjectPathElementFactory;
        _propertyNameConvertedObjectPathElementFactory = propertyNameConvertedObjectPathElementFactory;
        _indexConvertedObjectPathElementFactory = indexConvertedObjectPathElementFactory;
        _jsonConversionSettingsWrapper = jsonConversionSettingsWrapperFactory.Create(jsonConversionSettings);
    }

    /// <inheritdoc />
    public IConversionResult<object?> Convert(IParsedValue parsedValue, Type typeToConvertTo, IReadOnlyList<bool>? convertedValueNullability, IJsonConversionSettingsOverrides? jsonConversionSettingOverrides = null)
    {
        var mergedJsonConversionSettings = CreateMergedSettings(jsonConversionSettingOverrides);
        var errorsAndWarnings = new ConversionErrorsAndWarnings(new ConversionErrors(), new ConversionErrors());
        var currentlyConvertedObjectContext = new CurrentlyConvertedObjectContext(
            _convertedObjectPathFactory.Create(_rootConvertedObjectPathElementFactory.Create(typeToConvertTo)),
            _indexConvertedObjectPathElementFactory, _propertyNameConvertedObjectPathElementFactory,
            _valueNullabilityHelpers, convertedValueNullability);

        var valueNotSetErrorReportingType = ErrorReportingType.Ignore;
        var nonNullablePropertyNotSetErrorReportingType = ErrorReportingType.Ignore;
        var nonNullableCollectionItemValueNotSetErrorReportingType = ErrorReportingType.Ignore;

        if (mergedJsonConversionSettings.TryGetConversionErrorTypeConfiguration(ConversionErrorType.ValueNotSet, out var conversionErrorTypeConfiguration))
            valueNotSetErrorReportingType = conversionErrorTypeConfiguration.ErrorReportingType;

        if (mergedJsonConversionSettings.TryGetConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, out conversionErrorTypeConfiguration))
            nonNullablePropertyNotSetErrorReportingType = conversionErrorTypeConfiguration.ErrorReportingType;

        if (mergedJsonConversionSettings.TryGetConversionErrorTypeConfiguration(ConversionErrorType.NonNullableCollectionItemValueNotSet, out conversionErrorTypeConfiguration))
            nonNullableCollectionItemValueNotSetErrorReportingType = conversionErrorTypeConfiguration.ErrorReportingType;

        var contextObject = new ContextObject(currentlyConvertedObjectContext, mergedJsonConversionSettings, errorsAndWarnings,
            valueNotSetErrorReportingType, nonNullablePropertyNotSetErrorReportingType, nonNullableCollectionItemValueNotSetErrorReportingType);

        try
        {
            var isConversionSuccess = ConvertJsonValue(parsedValue, 0, typeToConvertTo, contextObject, out var convertedValue);

            if (convertedValue == null && !currentlyConvertedObjectContext.IsValueNullable(typeToConvertTo) &&
                nonNullableCollectionItemValueNotSetErrorReportingType != ErrorReportingType.Ignore)
                AddError(contextObject, ConversionErrorType.ValueNotSet, "Return value is null", parsedValue);

            if (!isConversionSuccess && convertedValue != null)
                return new ConversionResult<object?>(errorsAndWarnings);

            return new ConversionResult<object?>(convertedValue, errorsAndWarnings);
        }
        catch (JsonConversionException)
        {
            if (errorsAndWarnings.ConversionErrors.Errors.Count == 0)
            {
                // We should never get here
                ThreadStaticLoggingContext.Context.ErrorFormat("No errors are reported when [{0}] is thrown", typeof(JsonConversionException));
                AddError(contextObject, ConversionErrorType.Error, GeneralConversionError, null);
            }

            return new ConversionResult<object?>(errorsAndWarnings);
        }
    }

    /// <summary>
    /// If returned value is false, an error will be added to <paramref name="contextObject"/>
    /// </summary>
    private bool ConvertJsonValue(IParsedValue parsedValue, int valueLevelInType,
        Type typeToConvertTo, ContextObject contextObject, out object? convertedValue)
    {
        convertedValue = null;
        switch (parsedValue)
        {
            case IParsedSimpleValue parsedSimpleValue:
                convertedValue = ConvertSimpleValue(parsedSimpleValue, typeToConvertTo, contextObject);
                break;

            case IParsedJson parsedJson:
                convertedValue = ConvertParsedJson(parsedJson, typeToConvertTo, contextObject);
                break;

            case IParsedArrayValue parsedArrayValue:
                var collectionItemLevelInType = valueLevelInType + 1;

                if (TryGetCollectionItemTypeData(parsedArrayValue, typeToConvertTo, collectionItemLevelInType, contextObject, out var itemTypeData))
                    convertedValue = ConvertParsedArrayValue(parsedArrayValue, typeToConvertTo, itemTypeData, collectionItemLevelInType, contextObject);

                break;

            default:
                var errorMessage = $"Invalid type [{parsedValue.GetType()}]";
                ThreadStaticLoggingContext.Context.Error(errorMessage);

                AddError(contextObject, ConversionErrorType.Error, errorMessage, parsedValue);
                return false;
        }

        if (contextObject.ErrorsAndWarnings.ConversionErrors.Errors.Count > 0)
        {
            convertedValue = null;
            return false;
        }

        if (convertedValue == null)
            return !typeToConvertTo.IsValueType || Nullable.GetUnderlyingType(typeToConvertTo) != null;

        if (!typeToConvertTo.IsInstanceOfType(convertedValue))
        {
            // We should never get here, since the calls to private methods in this class already do the checks
            var errorMessage = $"Internal error. The query result is expected to be of type [{typeToConvertTo}]. Actual type is [{convertedValue.GetType()}]";
            ThreadStaticLoggingContext.Context.Error(errorMessage);

            AddError(contextObject, ConversionErrorType.Error, errorMessage, parsedValue);
            return false;
        }

        return true;
    }

    private object? ConvertSimpleValue(IParsedSimpleValue parsedSimpleValue, Type typeToConvertTo, ContextObject contextObject)
    {
        object? convertedValue;
        if (parsedSimpleValue.Value == null)
        {
            convertedValue = null;
        }
        else if (!_simpleJsonValueSerializer.TrySerialize(typeToConvertTo, parsedSimpleValue.Value, out convertedValue) &&
                 AddError(contextObject, ConversionErrorType.FailedToConvertJsonValueToExpectedType,
                     string.Format("Json value [{0}] cannot be converted to [{1}]. Make sure a serializer of type [{2}] is registered that de-serializes the value.",
                         parsedSimpleValue.IsString ? $"\"{parsedSimpleValue.Value}\"" : parsedSimpleValue.Value,
                         typeToConvertTo.FullName, typeToConvertTo.FullName), parsedSimpleValue))
        {
            throw new JsonConversionException();
        }

        return convertedValue;
    }

    private bool CanCreateInstanceOfType(Type type)
    {
        return type is { IsClass: true, IsAbstract: false };
    }

    private object? ConvertParsedJson(IParsedJson parsedJson, Type typeToConvertTo, ContextObject contextObject)
    {
        Type? typeToConvertToImplementation = null;

        if (contextObject.MergedJsonConversionSettings.TryGetInterfaceToImplementationMapping(typeToConvertTo, parsedJson, out var mappedType))
        {
            typeToConvertToImplementation = mappedType;

            if (!CanCreateInstanceOfType(typeToConvertToImplementation) && _modelClassMapper.TryMap(typeToConvertToImplementation, out var typeToConvertToImplementation2))
            {
                typeToConvertToImplementation = typeToConvertToImplementation2;
            }
        }
        else if (_modelClassMapper.TryMap(typeToConvertTo, out var typeToConvertToImplementation2))
        {
            typeToConvertToImplementation = typeToConvertToImplementation2;
        }

        string? createInstanceErrorMessage = null;
        if (typeToConvertToImplementation != null)
        {
            if (!CanCreateInstanceOfType(typeToConvertToImplementation))
            {
                createInstanceErrorMessage = $"Cannot create an instance of type [{typeToConvertToImplementation}] mapped to [{typeToConvertTo}]";
            }
        }
        else if (CanCreateInstanceOfType(typeToConvertTo))
        {
            typeToConvertToImplementation = typeToConvertTo;
        }
        else
        {
            createInstanceErrorMessage = $"Cannot created an instance of type [{typeToConvertTo}] and no mapping was found to map the class to non-abstract class.";
        }

        if (createInstanceErrorMessage != null || typeToConvertToImplementation == null)
        {
            if (!AddError(contextObject, ConversionErrorType.CannotCreateInstanceOfClass,
                    createInstanceErrorMessage ?? $"Cannot create an instance of type [{typeToConvertTo}].", parsedJson))
                throw new JsonConversionException();

            return null;
        }

        var allProperties = typeToConvertToImplementation.GetProperties();

        var modelClassCreationPropertyData = new List<IModelClassCreationPropertyData>(allProperties.Length);

        foreach (var propertyInfo in allProperties)
        {
            try
            {
                contextObject.ConvertedObjectContext.OnPropertyProcessingStarted(propertyInfo);

                object? propertyValue = null;

                var propertyKey = propertyInfo.Name;

                if (contextObject.MergedJsonConversionSettings.JsonPropertyFormat == JsonPropertyFormat.CamelCase)
                    propertyKey = propertyKey.Length == 1 ? propertyKey.ToLower() : string.Concat(char.ToLower(propertyKey[0]), propertyKey.Substring(1));

                if (parsedJson.TryGetJsonKeyValue(propertyKey, out var jsonKeyValue))
                {
                    if (!ConvertJsonValue(jsonKeyValue.Value, 0, propertyInfo.PropertyType, contextObject, out propertyValue) &&
                        !CheckConversionCanContinue(contextObject))
                    {
                        throw new JsonConversionException();
                    }
                }

                modelClassCreationPropertyData.Add(new ModelClassCreationPropertyData(propertyInfo, propertyValue));

                if (propertyValue == null &&
                    contextObject.NonNullablePropertyNotSetErrorReportingType != ErrorReportingType.Ignore &&
                    !contextObject.ConvertedObjectContext.IsValueNullable(propertyInfo.PropertyType))
                {
                    if (!AddError(contextObject, ConversionErrorType.NonNullablePropertyNotSet,
                            $"Failed to retrieve and set the value of non-nullable property [{propertyInfo.Name}] in type [{typeToConvertToImplementation}].", parsedJson))
                        throw new JsonConversionException();
                }
            }
            finally
            {
                contextObject.ConvertedObjectContext.OnPropertyProcessingCompleted();
            }
        }

        if (!_modelClassInstanceCreator.TryCreate(typeToConvertToImplementation, modelClassCreationPropertyData, out var createdInstance, out var errorMessage))
        {
            if (AddError(contextObject, ConversionErrorType.CannotCreateInstanceOfClass,
                        $"Failed to create an instance of type [{typeToConvertToImplementation}]. Error details: {errorMessage}", parsedJson))
                throw new JsonConversionException();

            return null;
        }

        return createdInstance;
    }

    private bool TryGetCollectionItemTypeData(IParsedArrayValue parsedArrayValue,
        Type collectionTypeToConvertTo, int collectionItemLevel, ContextObject contextObject, [NotNullWhen(true)] out CollectionItemTypeData? itemTypeData)
    {
        itemTypeData = null;

        if (!_collectionTypeHelpers.TryGetItemType(collectionTypeToConvertTo, out var itemType))
        {
            if (!AddError(contextObject, ConversionErrorType.FailedToConvertJsonValueToExpectedType,
                        $"Invalid collection type [{collectionTypeToConvertTo}]. Failed to determine collection item type.", parsedArrayValue))
                throw new JsonConversionException();

            return false;
        }

        itemTypeData = new CollectionItemTypeData(itemType, contextObject.ConvertedObjectContext.AreCollectionItemsNullable(itemType, collectionItemLevel));

        return true;
    }

    private object ConvertParsedArrayValue(IParsedArrayValue parsedArrayValue, Type collectionTypeToConvertTo,
        CollectionItemTypeData collectionItemTypeData, int collectionItemLevel, ContextObject contextObject)
    {
        var enumerable = ConvertParsedArrayValueToEnumerable(parsedArrayValue, collectionItemTypeData, collectionItemLevel, contextObject);
        return _collectionTypeHelpers.ConvertToCollection(enumerable, collectionTypeToConvertTo, collectionItemTypeData.ItemType);
    }

    private IEnumerable<object?> ConvertParsedArrayValueToEnumerable(IParsedArrayValue parsedArrayValue,
        CollectionItemTypeData collectionItemTypeData, int collectionItemLevel, ContextObject contextObject)
    {
        for (var i = 0; i < parsedArrayValue.Values.Count; ++i)
        {
            try
            {
                contextObject.ConvertedObjectContext.OnCollectionItemProcessingStarted(i, collectionItemTypeData.ItemType);
                var parsedValue = parsedArrayValue.Values[i];

                if (ConvertJsonValue(parsedValue, collectionItemLevel, collectionItemTypeData.ItemType, contextObject, out var convertedItem))
                {
                    if (convertedItem == null && collectionItemLevel > 0 && !collectionItemTypeData.IsNullable &&
                        contextObject.NonNullableCollectionItemValueNotSetErrorReportingType != ErrorReportingType.Ignore)
                    {
                        if (!AddError(contextObject, ConversionErrorType.NonNullableCollectionItemValueNotSet,
                                "Collection item was not set", parsedValue))
                            throw new JsonConversionException();
                    }
                }

                if (!CheckConversionCanContinue(contextObject))
                    throw new JsonConversionException();

                yield return convertedItem;
            }
            finally
            {
                contextObject.ConvertedObjectContext.OnCollectionItemProcessingCompleted();
            }
        }
    }

    /// <summary>
    /// Adds an error and returns true if processing can continue.
    /// Returns false if added error should fail conversion.
    /// </summary>
    private bool AddError(ContextObject contextObject, ConversionErrorType conversionErrorType, string error, IParsedValue? parsedValue)
    {
        IConversionErrors conversionErrors;
        if (contextObject.MergedJsonConversionSettings.TryGetConversionErrorTypeConfiguration(conversionErrorType,
                out var conversionErrorTypeConfiguration))
        {
            switch (conversionErrorTypeConfiguration.ErrorReportingType)
            {
                case ErrorReportingType.Ignore:
                    return true;

                case ErrorReportingType.ReportAsError:
                    conversionErrors = contextObject.ErrorsAndWarnings.ConversionErrors;
                    break;

                case ErrorReportingType.ReportAsWarning:
                    conversionErrors = contextObject.ErrorsAndWarnings.ConversionWarnings;
                    break;
                default:
                    conversionErrors = contextObject.ErrorsAndWarnings.ConversionErrors;
                    break;
            }
        }
        else
        {
            conversionErrors = contextObject.ErrorsAndWarnings.ConversionErrors;
        }

        conversionErrors.AddError(new ConversionError(conversionErrorType, error, contextObject.ConvertedObjectContext.ConvertedObjectPath.Clone(),
            parsedValue?.GetPath(), parsedValue?.PathInReferencedJson));

        if (conversionErrors == contextObject.ErrorsAndWarnings.ConversionWarnings)
            return true;

        return !contextObject.MergedJsonConversionSettings.FailOnFirstError;
    }

    private bool CheckConversionCanContinue(ContextObject contextObject)
    {
        return contextObject.ErrorsAndWarnings.ConversionErrors.Errors.Count == 0 || !contextObject.MergedJsonConversionSettings.FailOnFirstError;
    }

    private MergedJsonConversionSettings CreateMergedSettings(IJsonConversionSettingsOverrides? jsonConversionSettingOverrides)
    {
        return new MergedJsonConversionSettings(_jsonConversionSettingsWrapper, jsonConversionSettingOverrides);
    }

    private class ContextObject
    {
        public ContextObject(CurrentlyConvertedObjectContext convertedObjectContext,
            MergedJsonConversionSettings mergedJsonConversionSettings,
            ConversionErrorsAndWarnings errorsAndWarnings, ErrorReportingType valueNotSetErrorReportingType, ErrorReportingType nonNullablePropertyNotSetErrorReportingType, ErrorReportingType nonNullableCollectionItemValueNotSetErrorReportingType)
        {
            ConvertedObjectContext = convertedObjectContext;
            MergedJsonConversionSettings = mergedJsonConversionSettings;
            ErrorsAndWarnings = errorsAndWarnings;
            ValueNotSetErrorReportingType = valueNotSetErrorReportingType;
            NonNullablePropertyNotSetErrorReportingType = nonNullablePropertyNotSetErrorReportingType;
            NonNullableCollectionItemValueNotSetErrorReportingType = nonNullableCollectionItemValueNotSetErrorReportingType;
        }

        public CurrentlyConvertedObjectContext ConvertedObjectContext { get; }

        public MergedJsonConversionSettings MergedJsonConversionSettings { get; }
        public ConversionErrorsAndWarnings ErrorsAndWarnings { get; }

        public ErrorReportingType ValueNotSetErrorReportingType { get; }
        public ErrorReportingType NonNullablePropertyNotSetErrorReportingType { get; }
        public ErrorReportingType NonNullableCollectionItemValueNotSetErrorReportingType { get; }
    }

    private class CurrentlyConvertedObjectContext
    {
        private readonly IIndexConvertedObjectPathElementFactory _indexConvertedObjectPathElementFactory;
        private readonly IPropertyNameConvertedObjectPathElementFactory _propertyNameConvertedObjectPathElementFactory;
        private readonly IValueNullabilityHelpers _valueNullabilityHelpers;
        private readonly IReadOnlyList<bool>? _convertedValueNullability;

        private readonly Stack<IEnumerable<CustomAttributeData>> _currentlyProcessedPropertyAttributes = new();

        public CurrentlyConvertedObjectContext(IConvertedObjectPath convertedObjectPath,
            IIndexConvertedObjectPathElementFactory indexConvertedObjectPathElementFactory,
            IPropertyNameConvertedObjectPathElementFactory propertyNameConvertedObjectPathElementFactory,
            IValueNullabilityHelpers valueNullabilityHelpers,
            IReadOnlyList<bool>? convertedValueNullability)
        {
            ConvertedObjectPath = convertedObjectPath;
            _indexConvertedObjectPathElementFactory = indexConvertedObjectPathElementFactory;
            _propertyNameConvertedObjectPathElementFactory = propertyNameConvertedObjectPathElementFactory;
            _valueNullabilityHelpers = valueNullabilityHelpers;
            _convertedValueNullability = convertedValueNullability;
        }

        public IConvertedObjectPath ConvertedObjectPath { get; }

        public void OnCollectionItemProcessingStarted(int itemIndex, Type itemType)
        {
            ConvertedObjectPath.Push(_indexConvertedObjectPathElementFactory.Create(itemIndex, itemType));
        }

        public void OnCollectionItemProcessingCompleted()
        {
            if (ConvertedObjectPath.Path.Count == 0)
            {
                ThreadStaticLoggingContext.Context.ErrorFormat("The stack [{0}] is empty. This should never happen.", nameof(ConvertedObjectPath));
                return;
            }

            ConvertedObjectPath.Pop();
        }

        public void OnPropertyProcessingStarted(PropertyInfo propertyInfo)
        {
            ConvertedObjectPath.Push(_propertyNameConvertedObjectPathElementFactory.Create(propertyInfo.Name, propertyInfo.PropertyType));
            _currentlyProcessedPropertyAttributes.Push(CustomAttributeData.GetCustomAttributes(propertyInfo));
        }

        public void OnPropertyProcessingCompleted()
        {
            if (_currentlyProcessedPropertyAttributes.Count == 0)
            {
                ThreadStaticLoggingContext.Context.ErrorFormat("The stack [{0}] is empty. This should never happen.", nameof(_currentlyProcessedPropertyAttributes));
                return;
            }

            if (ConvertedObjectPath.Path.Count == 0)
            {
                ThreadStaticLoggingContext.Context.ErrorFormat("The stack [{0}] is empty. This should never happen.", nameof(ConvertedObjectPath));
                return;
            }

            ConvertedObjectPath.Pop();
            _currentlyProcessedPropertyAttributes.Pop();
        }

        public bool IsValueNullable(Type valueType)
        {
            if (_currentlyProcessedPropertyAttributes.Count > 0)
                return _valueNullabilityHelpers.IsValueNullable(valueType, _currentlyProcessedPropertyAttributes.Peek());

            if (_convertedValueNullability == null || _convertedValueNullability.Count == 0)
                return true;

            return _convertedValueNullability[0];
        }

        public bool AreCollectionItemsNullable(Type collectionItemType, int collectionItemLevel)
        {
            if (_currentlyProcessedPropertyAttributes.Count > 0)
                return _valueNullabilityHelpers.AreCollectionItemsNullable(collectionItemType, collectionItemLevel, _currentlyProcessedPropertyAttributes.Peek());

            if (_convertedValueNullability == null || collectionItemLevel >= _convertedValueNullability.Count)
                return false;

            return _convertedValueNullability[collectionItemLevel];
        }
    }

    private class MergedJsonConversionSettings
    {
        private readonly IJsonConversionSettingsWrapper _globalJsonConversionSettingsWrapper;
        private readonly IJsonConversionSettingsWrapper _overrideJsonConversionSettingsWrapper;

        public MergedJsonConversionSettings(IJsonConversionSettingsWrapper globalJsonConversionSettingsWrapper,
            IJsonConversionSettingsOverrides? jsonConversionSettingsOverrides)
        {
            IJsonConversionSettings overrideJsonConversionSettings = new JsonConversionSettings
            {
                TryMapJsonConversionType = jsonConversionSettingsOverrides?.TryMapJsonConversionType,
                ConversionErrorTypeConfigurations = jsonConversionSettingsOverrides?.ConversionErrorTypeConfigurations ?? globalJsonConversionSettingsWrapper.JsonConversionSettings.ConversionErrorTypeConfigurations,
                JsonPropertyFormat = jsonConversionSettingsOverrides?.JsonPropertyFormat ?? globalJsonConversionSettingsWrapper.JsonConversionSettings.JsonPropertyFormat,
                FailOnFirstError = jsonConversionSettingsOverrides?.FailOnFirstError ?? globalJsonConversionSettingsWrapper.JsonConversionSettings.FailOnFirstError,
            };

            _globalJsonConversionSettingsWrapper = globalJsonConversionSettingsWrapper;
            _overrideJsonConversionSettingsWrapper = new JsonConversionSettingsWrapper(overrideJsonConversionSettings);
            FailOnFirstError = overrideJsonConversionSettings.FailOnFirstError;
            JsonPropertyFormat = overrideJsonConversionSettings.JsonPropertyFormat;
        }

        public bool FailOnFirstError { get; }
        public JsonPropertyFormat JsonPropertyFormat { get; }

        public bool TryGetInterfaceToImplementationMapping(Type defaultTypeToConvertParsedJsonTo, IParsedJson convertedParsedJson, [NotNullWhen(true)] out Type? mappedType)
        {
            if (_overrideJsonConversionSettingsWrapper.JsonConversionSettings.TryMapJsonConversionType != null)
            {
                mappedType = _overrideJsonConversionSettingsWrapper.JsonConversionSettings.TryMapJsonConversionType(defaultTypeToConvertParsedJsonTo, convertedParsedJson);

                if (mappedType != null)
                    return true;
            }

            mappedType = _globalJsonConversionSettingsWrapper.JsonConversionSettings.TryMapJsonConversionType?.Invoke(defaultTypeToConvertParsedJsonTo, convertedParsedJson);
            return mappedType != null;
        }

        public bool TryGetConversionErrorTypeConfiguration(ConversionErrorType conversionErrorType, [NotNullWhen(true)] out IConversionErrorTypeConfiguration? conversionErrorTypeConfiguration)
        {
            if (_overrideJsonConversionSettingsWrapper.TryGetConversionErrorTypeConfiguration(conversionErrorType, out conversionErrorTypeConfiguration))
            {
                return true;
            }

            return _globalJsonConversionSettingsWrapper.TryGetConversionErrorTypeConfiguration(conversionErrorType, out conversionErrorTypeConfiguration);
        }
    }
}
