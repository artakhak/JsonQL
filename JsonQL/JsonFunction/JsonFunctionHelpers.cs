using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using System.Diagnostics.CodeAnalysis;
using TypeCode = JsonQL.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.JsonFunction;

public static class JsonFunctionHelpers
{
    /// <summary>
    /// Tries to convert value in <param name="evaluatedValue"></param> to instance of <see cref="IJsonComparable"/>.<br/>
    /// If the returned value is true, the value in <param name="jsonComparable"></param> will be non-null.<br/>
    /// -If the value in <param name="evaluatedValue"></param> is not null, and is one of the types specified in<br/>
    /// <see cref="SimpleTypes.TypeCode"/> (e.g., <see cref="String"/>, <see cref="Double"/>, etc.), then<br/>
    /// <param name="jsonComparable"></param> will be set to an instance of <see cref="IJsonComparable"/> created with the value of<br/>
    /// <param name="evaluatedValue"></param>.<br/>
    /// -Otherwise, if the value is a non-empty collection of <see cref="IParsedValue"/>, and the first item is <see cref="IParsedSimpleValue"/>, with<br/>
    /// non-null value of <see cref="IParsedSimpleValue.Value"/>, <param name="jsonComparable"></param> is either created from<br/>
    /// <see cref="IParsedSimpleValue.Value"/><br/>
    /// </summary>
    /// <param name="evaluatedValue">Value to convert.
    /// </param>
    /// <param name="expectedConvertedType">If the value is not null, conversion succeeds only if converted valid is of type <param name="expectedConvertedType"></param></param>
    /// <param name="jsonComparable">Output parameter for converted value. If the returned value is trie, this value is not null.</param>
    /// <returns></returns>
    public static bool TryConvertValueToJsonComparable(object? evaluatedValue, TypeCode? expectedConvertedType,
        [NotNullWhen(true)] out IJsonComparable? jsonComparable)
    {
        jsonComparable = null;

        if (evaluatedValue == null)
        {
            return false;
        }

        if (evaluatedValue is ICollectionJsonValuePathLookupResult collectionJsonValuePathLookupResult)
            evaluatedValue = collectionJsonValuePathLookupResult.ParsedValues;
        else if (evaluatedValue is ISingleItemJsonValuePathLookupResult singleItemJsonValuePathLookupResult)
            evaluatedValue = singleItemJsonValuePathLookupResult.ParsedValue;
        else if (evaluatedValue is double doubleValue)
        {
            jsonComparable = new DoubleJsonComparable(doubleValue);
            return true;
        }
       
        if (evaluatedValue is string stringValue)
        {
            if (expectedConvertedType != null)
            {
                switch (expectedConvertedType)
                {
                    case TypeCode.String:
                        break;

                    case TypeCode.DateTime:

                        if (!DateTimeOperationsAmbientContext.Context.TryParse(stringValue, out var parsedDateTime))
                            return false;

                        jsonComparable = new DateTimeJsonComparable(parsedDateTime.Value);
                        return true;

                    default:
                        return false;
                }
            }

            jsonComparable = new StringJsonComparable(stringValue);
            return true;
        }

        if (evaluatedValue is bool booleanValue)
        {
            if (expectedConvertedType != null && expectedConvertedType != TypeCode.Boolean)
                return false;

            jsonComparable = new BooleanJsonComparable(booleanValue);
            return true;
        }

        if (evaluatedValue is DateTime dateTimeValue)
        {
            if (expectedConvertedType != null && expectedConvertedType != TypeCode.DateTime)
                return false;

            jsonComparable = new DateTimeJsonComparable(dateTimeValue);
            return true;
        }

        var parsedValue = evaluatedValue as IParsedValue;

        if (parsedValue == null &&
            evaluatedValue is IEnumerable<IParsedValue> parsedValues)
        {
            var parsedValuesList = parsedValues.ToList();

            if (!parsedValuesList.Any())
                return false;

            parsedValue = parsedValuesList[0];
        }

        if (parsedValue is IParsedSimpleValue { Value: not null } parsedSimpleValue)
        {
            if (parsedSimpleValue.Value == null)
                return false;

            if (parsedSimpleValue.IsString)
            {
                if (expectedConvertedType != null)
                {
                    switch (expectedConvertedType)
                    {
                        case TypeCode.DateTime:
                            if (parsedSimpleValue.Value == null || !DateTimeOperationsAmbientContext.Context.TryParse(parsedSimpleValue.Value, out var parsedDateTime))
                                return false;

                            jsonComparable = new DateTimeJsonComparable(parsedDateTime.Value);
                            return true;
                        case TypeCode.String:
                            break;
                        default:
                            return false;
                    }
                }

                jsonComparable = new StringJsonComparable(parsedSimpleValue.Value);
                return true;
            }

            if (parsedSimpleValue.Value == Constants.JsonTrueValue || parsedSimpleValue.Value == Constants.JsonFalseValue)
            {
                if (expectedConvertedType != null && expectedConvertedType != TypeCode.Boolean)
                    return false;

                jsonComparable = new BooleanJsonComparable(parsedSimpleValue.Value == Constants.JsonTrueValue);
                return true;
            }

            if (expectedConvertedType != null && expectedConvertedType != TypeCode.Double)
                return false;

            if (double.TryParse(parsedSimpleValue.Value, out var parsedDoubleValue))
            {
                jsonComparable = new DoubleJsonComparable(parsedDoubleValue);
                return true;
            }
        }

        return false;
    }
}
