// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Provides an interface for creating instances of a model class based on a specified type and collection of property data.
/// This interface defines a method for attempting to create an object instance with appropriate validation and error reporting.
/// It is intended to be used in JSON-to-object conversion processes where instances of model classes need to be dynamically created.
/// </summary>
public interface IModelClassInstanceCreator
{
    /// <summary>
    /// Attempts to create an instance of the specified type using the provided collection of property data.
    /// </summary>
    /// <param name="createdInstanceType">The <see cref="Type"/> of the instance to be created.</param>
    /// <param name="propertyValues">A collection of property data to be used for populating the instance.</param>
    /// <param name="createdInstance">
    /// When successful, contains the created instance of the specified type. If unsuccessful, it will be <see langword="null"/>.
    /// </param>
    /// <param name="errorMessage">
    /// When the creation fails, contains an error message describing the reason for failure. Otherwise, it will be <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the instance was created successfully; otherwise, <see langword="false"/>.
    /// </returns>
    bool TryCreate(Type createdInstanceType, IReadOnlyList<IModelClassCreationPropertyData> propertyValues, [NotNullWhen(true)] out object? createdInstance, 
        [NotNullWhen(false)] out string? errorMessage);
}

/// <inheritdoc />
public class ModelClassInstanceCreator : IModelClassInstanceCreator
{
    /// <inheritdoc />
    public bool TryCreate(Type createdInstanceType, IReadOnlyList<IModelClassCreationPropertyData> propertyValues, 
        [NotNullWhen(true)] out object? createdInstance, [NotNullWhen(false)] out string? errorMessage)
    {
        createdInstance = null;
        errorMessage = null;

        if (!createdInstanceType.IsClass || createdInstanceType.IsAbstract)
        {
            errorMessage = $"Type [{createdInstanceType}] is abstract or is not a reference type.";
            return false;
        }

        var publicConstructors = createdInstanceType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).ToList();
        
        if (publicConstructors.Count == 0)
        {
            errorMessage = $"Type [{createdInstanceType}] has no public constructors.";
            return false;
        }

        if (publicConstructors.Count > 1)
        {
            errorMessage = $"Type [{createdInstanceType}] has more than one public constructors.";
            return false;
        }

        var constructorInfo = publicConstructors[0];
       
        var constructorParametersMetadata = constructorInfo.GetParameters();
        if (constructorParametersMetadata.Length > 0)
        {
            var constructorParametersValues = new object?[constructorParametersMetadata.Length];

            var constructorParameterNameToPropertyData = new Dictionary<string, IModelClassCreationPropertyData>(StringComparer.OrdinalIgnoreCase);
            
            foreach (var propertyValue in propertyValues)
            {
                var constructorParameterName = propertyValue.PropertyInfo.Name.Length == 1 ? propertyValue.PropertyInfo.Name.ToLower() :
                    string.Concat(Char.ToLower(propertyValue.PropertyInfo.Name[0]), propertyValue.PropertyInfo.Name[1..]);
                constructorParameterNameToPropertyData[constructorParameterName] = propertyValue;
            }

            for (var paramIndex = 0; paramIndex < constructorParametersMetadata.Length; ++paramIndex)
            {
                var parameterInfo = constructorParametersMetadata[paramIndex];

                object? parameterValue = null;
                if (parameterInfo.Name != null && constructorParameterNameToPropertyData.TryGetValue(parameterInfo.Name, out var propertyValue))
                {
                    if (!parameterInfo.ParameterType.IsAssignableFrom(propertyValue.PropertyInfo.PropertyType))
                    {
                        errorMessage = $"Constructor parameter [{parameterInfo.Name}] of type [{parameterInfo.ParameterType.FullName}] is not compatible with type [{propertyValue.PropertyInfo.PropertyType.FullName}] of property [{propertyValue.PropertyInfo.Name}].";
                        return false;
                    }

                    parameterValue = propertyValue.PropertyValue;
                }

                constructorParametersValues[paramIndex] = parameterValue;
            }

            try
            {
                var createdInstanceLocal = constructorInfo.Invoke(constructorParametersValues);
                createdInstance = createdInstanceLocal;
            }
            catch (Exception e)
            {
                errorMessage = $"Failed to create an instance of type [{createdInstanceType}] with given parameters. Error message: {e.Message}";
                ThreadStaticLoggingContext.Context.Error(errorMessage, e);
                return false;
            }
        }
        else
        {
            createdInstance = constructorInfo.Invoke(Array.Empty<object?>());
        }

        foreach (var propertyValue in propertyValues)
        {
            if (propertyValue.PropertyValue == null)
                continue;

            if (!propertyValue.PropertyInfo.CanWrite)
                continue;

            var setter = propertyValue.PropertyInfo.GetSetMethod();

            if (!(setter?.IsPublic??false))
                continue;

            try
            {
                propertyValue.PropertyInfo.SetValue(createdInstance, propertyValue.PropertyValue);
            }
            catch (Exception e)
            {
                ThreadStaticLoggingContext.Context.Error($"Failed to set the value of property [{propertyValue.PropertyInfo.Name}] in [{createdInstance.GetType()}]", e);
                continue;
            }
        }

        return true;
    }
}