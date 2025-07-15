// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Text;

namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// Represents a contract for generating display values from a converted object path.
/// Provides methods to obtain a string representation suitable for display purposes based on the structure
/// and elements of a converted object path.
public interface IConvertedObjectPathToDisplayValueGenerator
{
    /// Generates a string representation suitable for display purposes based on the structure
    /// and elements of a converted object path.
    /// <param name="convertedObjectPath">The converted object path containing the root element
    /// and value selector elements that define the navigational structure.</param>
    /// <returns>A string representation of the converted object path that is formatted for display.</returns>
    string GetDisplayValue(IConvertedObjectPath convertedObjectPath);
}

/// <inheritdoc />
public class ConvertedObjectPathToDisplayValueGenerator : IConvertedObjectPathToDisplayValueGenerator
{
    /// <inheritdoc />
    public string GetDisplayValue(IConvertedObjectPath convertedObjectPath)
    {
        var displayValueStrBldr = new StringBuilder(convertedObjectPath.Path.Count * 50);

        displayValueStrBldr.Append(convertedObjectPath.RootConvertedObjectPathElement.Name);

        foreach (var convertedObjectPathValueSelectorElement in convertedObjectPath.Path)
        {
            switch (convertedObjectPathValueSelectorElement)
            {
                case IIndexConvertedObjectPathElement indexConvertedObjectPathElement:

                    displayValueStrBldr.Append('[').Append(indexConvertedObjectPathElement.Name).Append(']');
                    break;

                case IPropertyNameConvertedObjectPathElement propertyNameConvertedObjectPathElement:
                    displayValueStrBldr.Append('.').Append(propertyNameConvertedObjectPathElement.Name);
                    break;

                default:
                    ThreadStaticLoggingContext.Context.ErrorFormat("Unhandled type [{0}]. Display value will be generated, but check the type.",
                        convertedObjectPathValueSelectorElement.GetType());

                    displayValueStrBldr.Append('.').Append(convertedObjectPathValueSelectorElement.Name);
                    break;
            }
        }

        return displayValueStrBldr.ToString();
    }
}