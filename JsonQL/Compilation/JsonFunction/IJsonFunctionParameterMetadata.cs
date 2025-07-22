// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;

namespace JsonQL.Compilation.JsonFunction;

public interface IJsonFunctionParameterMetadata
{
    /// <summary>
    /// Name that will appear in logs. For example, "operand1", etc. The name might tbe different from actual parameter value
    /// passed to classes that extend <see cref="JsonFunctionAbstr"/>.
    /// </summary>
    string Name { get; }
   
    /// <summary>
    /// Indicates whether a parameter should be validated to ensure it is not a JSON value path
    /// that resolves to a selector path containing elements that can select multiple items.
    /// For example, a path with elements that do not exclusively select single items
    /// would fail validation when this property is set to true.
    /// </summary>
    bool ValidateIsNotMultipleValuesSelectorPath { get; }

    /// <summary>
    /// One of the types: <see cref="IBooleanJsonFunction"/>, <see cref="IStringJsonFunction"/>, <see cref="IDoubleJsonFunction"/>,
    /// <see cref="IDateTimeJsonFunction"/>, <see cref="IJsonFunction"/>, etc.
    /// </summary>
    Type ExpectedParameterFunctionType { get; }

    /// <summary>
    /// True, if parameter is required.
    /// </summary>
    bool IsRequired { get; }
}

public class JsonFunctionParameterMetadata: IJsonFunctionParameterMetadata
{
    /// <summary>
    /// Represents metadata information for a JSON function parameter.
    /// Provides details such as the parameter's name, expected type, and whether it is required.
    /// </summary>
    /// <param name="name">
    /// Name that will appear in logs. For example, "operand1", etc. The name might tbe different from actual parameter value
    /// passed to classes that extend <see cref="JsonFunctionAbstr"/>.
    /// </param>
    /// <param name="expectedParameterFunctionType">
    /// One of the types: <see cref="IBooleanJsonFunction"/>, <see cref="IStringJsonFunction"/>, <see cref="IDoubleJsonFunction"/>,
    /// <see cref="IDateTimeJsonFunction"/>, <see cref="IJsonFunction"/>, etc.
    /// </param>
    /// <param name="isRequired">
    /// True, if parameter is required.
    /// </param>
    public JsonFunctionParameterMetadata(string name, Type expectedParameterFunctionType, bool isRequired)
    {
        Name = name;
        ExpectedParameterFunctionType = expectedParameterFunctionType;
        IsRequired = isRequired;
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool ValidateIsNotMultipleValuesSelectorPath { get; set; } = true;

    /// <inheritdoc />
    public Type ExpectedParameterFunctionType { get; }

    /// <inheritdoc />
    public bool IsRequired { get; }
}
