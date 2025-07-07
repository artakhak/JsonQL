// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator;

/// <summary>
/// Represents an abstract base class for JSON value mutators that work with computed or calculated values.
/// This class is designed to modify and process JSON structures by utilizing specific JSON functions and
/// parsed values, providing an abstraction for implementing more specialized mutator behaviors.
/// </summary>
public abstract class CalculatedJsonValueMutatorAbstr : JsonValueMutatorAbstr
{
    private readonly IParsedJsonVisitor _parsedJsonVisitor;
    private readonly IStringFormatter _stringFormatter;

    /// <summary>
    /// Represents an abstract base class for mutators that calculate custom JSON values during processing.
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="JsonValueMutatorAbstr"/> and provides functionality to handle parsed JSON values,
    /// functions, and formatting during the mutation process.
    /// </remarks>
    protected CalculatedJsonValueMutatorAbstr(IParsedSimpleValue parsedSimpleValue,
        IJsonFunction jsonFunction,
        IParsedJsonVisitor parsedJsonVisitor,
        IStringFormatter stringFormatter)
        : base(jsonFunction.LineInfo)
    {
        _parsedJsonVisitor = parsedJsonVisitor;
        _stringFormatter = stringFormatter;
        JsonFunction = jsonFunction;
        ParsedSimpleValue = parsedSimpleValue;
    }

    /// <inheritdoc />
    public sealed override void Mutate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, List<IJsonObjectParseError> errors)
    {
        var parsedSimpleValue = MutatorHelpers.TryGetParsedSimpleValue(this.ParsedSimpleValue);

        if (parsedSimpleValue == null)
        {
            ThreadStaticLoggingContext.Context.WarnFormat("The mutator [{0}] will not execute.", GetType().FullName??"null");
            return;
        }

        var jsonFunctionResult = JsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, null);

        if (jsonFunctionResult.Errors.Count > 0)
        {
            errors.AddRange(jsonFunctionResult.Errors);
            return;
        }
       
        IParsedValue? parsedEvaluatedValue = null;

        IParsedArrayValue? CreateParsedArrayValue(IReadOnlyList<IParsedValue> parsedValues)
        {
            if (parsedSimpleValue.ParentJsonValue == null)
            {
                ThreadStaticLoggingContext.Context.ErrorFormat("The value of [{0}.{1}] is null.",
                    typeof(IParsedSimpleValue).FullName!, nameof(IParsedSimpleValue.ParentJsonValue));
                return null;
            }

            var parsedArrayValue = new ParsedArrayValue(_parsedJsonVisitor, parsedSimpleValue.RootParsedValue, parsedSimpleValue.ParentJsonValue,
                null, null);

            foreach (var parsedValue in parsedValues)
                parsedArrayValue.AddValue(parsedValue);

            return parsedArrayValue;
        }

        if (jsonFunctionResult.Value is IJsonValuePathLookupResult jsonValuePathLookupResult)
        {
            if (jsonValuePathLookupResult is ICollectionJsonValuePathLookupResult collectionJsonValuePathLookupResult)
            {
                parsedEvaluatedValue = CreateParsedArrayValue(collectionJsonValuePathLookupResult.ParsedValues);
            }
            else if (jsonValuePathLookupResult is ISingleItemJsonValuePathLookupResult singleItemJsonValuePathLookupResult)
            {
                parsedEvaluatedValue = singleItemJsonValuePathLookupResult.ParsedValue;
            }
            else
            {
                var jsonValuePathJsonFunction = this.JsonFunction as IJsonValuePathJsonFunction;

                ThreadStaticLoggingContext.Context.ErrorFormat("Invalid implementation [{0}] in path [{1}]. Expected either [{2}] or [{3}].",
                    jsonValuePathLookupResult.GetType().FullName??"null",
                    jsonValuePathJsonFunction?.JsonValuePath.ToString()??"null",
                    typeof(ICollectionJsonValuePathLookupResult),
                    typeof(ISingleItemJsonValuePathLookupResult));
            }
        }
        else
        {
            if (jsonFunctionResult.Value == null)
            {
                parsedEvaluatedValue = new ParsedSimpleValue(parsedSimpleValue.RootParsedValue, null, null, null,
                    null, false);
            }
            else
            {
                var isStringJsonValue = true;

                switch (jsonFunctionResult.Value)
                {
                    case double:
                    case bool:
                        isStringJsonValue = false;
                        break;
                }

                switch (jsonFunctionResult.Value)
                {
                    case double:
                    case bool:
                    case string:
                    case DateTime:

                        if (!_stringFormatter.TryFormat(jsonFunctionResult.Value, out var formattedValue))
                        {
                            formattedValue = jsonFunctionResult.Value.ToString()!;
                            ThreadStaticLoggingContext.Context.ErrorFormat("Failed to format value [{0}] of type [{1}].",
                                formattedValue, jsonFunctionResult.Value.GetType());
                        }

                        parsedEvaluatedValue = new ParsedSimpleValue(parsedSimpleValue.RootParsedValue, null, null, null,
                            formattedValue, isStringJsonValue);
                        break;

                    case IParsedValue evaluatedParsedValue:
                        parsedEvaluatedValue = evaluatedParsedValue;
                        break;

                    case IReadOnlyList<IParsedValue> evaluatedParsedValues:
                        parsedEvaluatedValue = CreateParsedArrayValue(evaluatedParsedValues);
                        break;
                }
            }
        }

        if (parsedEvaluatedValue == null)
        {
            parsedEvaluatedValue = new ParsedSimpleValue(parsedSimpleValue.RootParsedValue, parsedSimpleValue.ParentJsonValue, null, null,
                null, false);
        }

        MutateValue(parsedSimpleValue, parsedEvaluatedValue, errors);
    }

    /// <summary>
    /// Represents the parsed simple value utilized by the JSON value mutator.
    /// This property holds the parsed value from the JSON structure that the mutator operates upon.
    /// It provides the context necessary to apply transformations or to evaluate JSON paths.
    /// </summary>
    /// <remarks>
    /// The <see cref="ParsedSimpleValue"/> is a protected member used internally by derived
    /// classes to access the specific parsed simple value to be mutated.
    /// Implementations of the <see cref="CalculatedJsonValueMutatorAbstr"/>
    /// class rely on this property for performing actions on the associated
    /// <see cref="IParsedSimpleValue"/> during the mutation process.
    /// </remarks>
    protected IParsedSimpleValue ParsedSimpleValue { get; }

    /// <summary>
    /// Represents the JSON function used in the calculated JSON value mutator.
    /// This property provides access to an implementation of <see cref="IJsonFunction"/>
    /// that defines the computational logic or lookup for evaluating JSON values during mutation processes.
    /// </summary>
    /// <remarks>
    /// The <see cref="JsonFunction"/> is a protected member utilized by the
    /// <see cref="CalculatedJsonValueMutatorAbstr"/> class to perform JSON function evaluations.
    /// It is instrumental in determining how JSON values are processed or modified, enabling
    /// complex transformation scenarios involving computational logic or path-based lookups.
    /// </remarks>
    protected IJsonFunction JsonFunction { get; }

    /// <summary>
    /// Mutates the json that owns <param name="currentParsedSimpleValueWithPath"></param>.
    /// </summary>
    /// <param name="currentParsedSimpleValueWithPath">Jason value that has the expression with referenced path.</param>
    /// <param name="referencedParsedValue">Evaluated json value.
    /// </param>
    /// <param name="errors">The implementation can add errors to this list.</param>
    protected abstract void MutateValue(
        IParsedSimpleValue currentParsedSimpleValueWithPath,
        IParsedValue referencedParsedValue,
        List<IJsonObjectParseError> errors);
}