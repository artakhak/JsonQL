using OROptimizer.Diagnostics.Log;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator;

public static class MutatorHelpers
{
    public static IParsedSimpleValue? TryGetParsedSimpleValue(IParsedSimpleValue parsedSimpleValue)
    {
        const string valueChangedIsOkMessage = "This is Ok, since the value might have been replaced by other mutators.";

        if (!parsedSimpleValue.RootParsedValue.TryGetParsedValue(parsedSimpleValue.Id, out var parsedValue))
        {
            // Probably the value was replaced by other mutators by the time we processed the value.
            LogHelper.Context.Log.InfoFormat("The value with Id=[{0}] was not found. {1}", parsedSimpleValue.Id, valueChangedIsOkMessage);
            return null;
        }

        if (parsedValue is not IParsedSimpleValue parsedSimpleValueCurrent || parsedSimpleValueCurrent.Value != parsedSimpleValue.Value)
        {
            LogHelper.Context.Log.InfoFormat("The value with Id=[{0}] was modified and does not match the original value anymore. {1}",
                parsedSimpleValue.Id, valueChangedIsOkMessage);
            return null;
        }

        return parsedSimpleValueCurrent;
    }
}