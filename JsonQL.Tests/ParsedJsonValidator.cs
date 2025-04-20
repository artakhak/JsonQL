using System.Text;
using JsonQL.JsonObjects;

namespace JsonQL.Tests;

public static class ParsedJsonValidator
{
    public static void ValidateRootParsedJson(IRootParsedValue validatedParsedValue, IRootParsedValue expectedParsedValue)
    {
        ValidateParsedValue(validatedParsedValue, expectedParsedValue);
    }

    private static void ValidateParsedValue(IParsedValue validatedParsedValue, IParsedValue expectedParsedValue)
    {
        switch (expectedParsedValue)
        {
            case IParsedJson expectedParsedJson:
                ValidateParsedJson(expectedParsedJson, validatedParsedValue);
                break;

            case IParsedArrayValue expectedParsedArrayValue:
                ValidateParsedArray(expectedParsedArrayValue, validatedParsedValue);
                break;

            case IParsedSimpleValue expectedParsedSimpleValue:

                if (validatedParsedValue is not IParsedSimpleValue validatedParsedSimpleValue ||
                    validatedParsedSimpleValue.IsString != expectedParsedSimpleValue.IsString ||
                    !string.Equals(validatedParsedSimpleValue.Value, expectedParsedSimpleValue.Value, StringComparison.Ordinal))
                {
                    AssertJsonObjectsMisMatch(validatedParsedValue, expectedParsedValue);
                }
                break;

            default:
                Assert.Fail($"Invalid json object type [{validatedParsedValue.GetType()}].");
                return;
        }
    }

    private static void AssertJsonObjectsMisMatch(IParsedValue validatedParsedValue, IParsedValue expectedParsedValue)
    {
        var errorMessage = new StringBuilder();

        errorMessage.AppendLine();
        errorMessage.AppendLine("Validated json does not match expected json.");

        errorMessage.Append("Validated json object path at mismatch: ").Append(validatedParsedValue.GetPath());
        errorMessage.AppendLine();

        errorMessage.Append("Expected json object path at mismatch: ").Append(expectedParsedValue.GetPath());
        errorMessage.AppendLine();
        errorMessage.AppendLine();

        Assert.Fail(errorMessage.ToString());
    }

    private static void ValidateParsedJson(IParsedJson expectedParsedJson, IParsedValue validatedParsedValue)
    {
        if (validatedParsedValue is not IParsedJson validatedParsedJson ||
            expectedParsedJson.KeyValues.Count != validatedParsedJson.KeyValues.Count)
        {
            AssertJsonObjectsMisMatch(validatedParsedValue, expectedParsedJson);
            return;
        }

        for (var i = 0; i < expectedParsedJson.KeyValues.Count; ++i)
        {
            var validatedKeyValue = validatedParsedJson.KeyValues[i];
            var expectedKeyValue = expectedParsedJson.KeyValues[i];

            if (!string.Equals(validatedKeyValue.Key, expectedKeyValue.Key, StringComparison.Ordinal))
            {
                AssertJsonObjectsMisMatch(validatedKeyValue.Value, expectedKeyValue.Value);
                return;
            }

            ValidateParsedValue(validatedKeyValue.Value, expectedKeyValue.Value);
        }
    }

    private static void ValidateParsedArray(IParsedArrayValue expectedParsedArrayValue, IParsedValue validatedParsedValue)
    {
        if (validatedParsedValue is not IParsedArrayValue validatedParsedArrayValue ||
            validatedParsedArrayValue.Values.Count != expectedParsedArrayValue.Values.Count)
        {
            AssertJsonObjectsMisMatch(validatedParsedValue, expectedParsedArrayValue);
            return;
        }

        for (var i = 0; i < expectedParsedArrayValue.Values.Count; ++i)
        {
            var validatedValue = expectedParsedArrayValue.Values[i];
            var expectedValue = validatedParsedArrayValue.Values[i];

            ValidateParsedValue(validatedValue, expectedValue);
        }
    }
}