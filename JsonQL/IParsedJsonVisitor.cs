// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL;

public delegate bool VisitJsonValueDelegate(IParsedValue parsedValue);

/// <summary>
/// Represents an interface for visiting parsed JSON values. This interface
/// defines methods for traversing and processing the parsed JSON structure
/// using a custom delegate to handle specific JSON values.
/// </summary>
public interface IParsedJsonVisitor
{
    /// <summary>
    /// Visits a parsed JSON value and processes it using the provided delegate.
    /// </summary>
    /// <param name="parsedValue">
    /// The parsed JSON value to visit. This value can be an array, object, or simple JSON value,
    /// such as a string, number, or boolean.
    /// </param>
    /// <param name="visitJsonValue">
    /// A delegate that defines the processing logic for the visited JSON value. The delegate
    /// takes an <see cref="IParsedValue"/> as its input and returns a boolean indicating the outcome
    /// of the visit operation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the given <paramref name="parsedValue"/> is not an instance of <see cref="IParsedArrayValue"/>
    /// or <see cref="IParsedJson"/>.
    /// </exception>
    void Visit(IParsedValue parsedValue, VisitJsonValueDelegate visitJsonValue);
}

/// <inheritdoc />
public class ParsedJsonVisitor : IParsedJsonVisitor
{
    /// <inheritdoc />
    public void Visit(IParsedValue parsedValue, VisitJsonValueDelegate visitJsonValue)
    {
        if (parsedValue is IParsedArrayValue parsedArrayValue)
            VisitValue(parsedArrayValue, visitJsonValue);
        else if (parsedValue is IParsedJson parsedJson)
            VisitParsedJson(parsedJson, visitJsonValue);
        else if (parsedValue is IParsedSimpleValue parsedSimpleValue)
            VisitValue(parsedSimpleValue, visitJsonValue);
        else
            throw new ArgumentException($"Expected an instance of [{typeof(IParsedArrayValue).FullName}] or [{typeof(IParsedJson).FullName}].", nameof(parsedValue));
    }
   
    private bool VisitParsedJson(IParsedJson parsedJson, VisitJsonValueDelegate visitJsonValue)
    {
        foreach (var keyValue in parsedJson.KeyValues)
        {
            if (!VisitValue(keyValue.Value, visitJsonValue))
                return false;
        }

        return true;
    }

    private bool VisitValue(IParsedValue parsedValue, VisitJsonValueDelegate visitJsonValue)
    {
        if (!visitJsonValue(parsedValue))
            return false;

        if (parsedValue is IParsedArrayValue parsedArrayValue)
        {
            for (var i = 0; i < parsedArrayValue.Values.Count; ++i)
            {
                if (!VisitValue(parsedArrayValue.Values[i], visitJsonValue))
                    return false;
            }

            return true;
        }

        if (parsedValue is IParsedJson parsedJson)
        {
            if (!VisitParsedJson(parsedJson, visitJsonValue))
                return false;
        }

        return true;
    }
}
