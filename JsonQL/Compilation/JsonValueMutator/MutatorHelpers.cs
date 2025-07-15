// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator;

/// <summary>
/// Provides helper methods used for mutating and validating parsed JSON values in the context
/// of JSON value mutators.
/// </summary>
internal static class MutatorHelpers
{
    /// <summary>
    /// Attempts to retrieve and validate the parsed simple value associated with the given input.
    /// </summary>
    /// <param name="parsedSimpleValue">The parsed simple value to validate and retrieve.</param>
    /// <returns>
    /// The validated and retrieved parsed simple value if found and unaltered; otherwise, null if the value is not found or has been modified.
    /// </returns>
    internal static IParsedSimpleValue? TryGetParsedSimpleValue(IParsedSimpleValue parsedSimpleValue)
    {
        const string valueChangedIsOkMessage = "This is Ok, since the value might have been replaced by other mutators.";

        if (!parsedSimpleValue.RootParsedValue.TryGetParsedValue(parsedSimpleValue.Id, out var parsedValue))
        {
            // Probably the value was replaced by other mutators by the time we processed the value.
            ThreadStaticLoggingContext.Context.InfoFormat("The value with Id=[{0}] was not found. {1}", parsedSimpleValue.Id, valueChangedIsOkMessage);
            return null;
        }

        if (parsedValue is not IParsedSimpleValue parsedSimpleValueCurrent || parsedSimpleValueCurrent.Value != parsedSimpleValue.Value)
        {
            ThreadStaticLoggingContext.Context.InfoFormat("The value with Id=[{0}] was modified and does not match the original value anymore. {1}",
                parsedSimpleValue.Id, valueChangedIsOkMessage);
            return null;
        }

        return parsedSimpleValueCurrent;
    }
}