using System.Reflection;
using JsonQL.JsonToObjectConversion.NullabilityCheck.Diagnostics.TestClasses;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.JsonToObjectConversion.NullabilityCheck.Diagnostics;

public static class NullableTypeHelpersTests
{
    public static bool TestNullabilityCheckApi(IMicrosoftInternalApiBasedNullabilityCheck nullabilityCheck)
    {
        if (!DoTestNullabilityCheckApi(nullabilityCheck))
        {
            ThreadStaticLogging.Log.Error(string.Concat("Nullability checks using Microsoft undocumented API didn't succeed. Microsoft might have made some changes.",
                Environment.NewLine,
                "All preference type properties will be considered nullable.",
                Environment.NewLine,
                $"Consider either replacing implementation [{typeof(ValueNullabilityHelpers)}] of [{typeof(IValueNullabilityHelpers)}] with another implementation (for example using custom attributes),",
                Environment.NewLine,
                $"or fixing the class [{typeof(ValueNullabilityHelpers)}] to make adjustments for changes done in Microsoft library"));
            return false;
        }

        return true;
    }
    
    private static bool DoTestNullabilityCheckApi(IMicrosoftInternalApiBasedNullabilityCheck nullabilityCheck)
    {
        try
        {
            TestPropertyNullabilityCheckApi(nullabilityCheck);
            //TestGenericMethodParameterNullabilityCheckApi(nullabilityCheck);
            return true;
        }
        catch(Exception e)
        {
            ThreadStaticLogging.Log.Error($"Nullability diagnostics tests for [{typeof(IMicrosoftInternalApiBasedNullabilityCheck).FullName}] failed", e);
            return false;
        }
    }

    private static void TestPropertyNullabilityCheckApi(IMicrosoftInternalApiBasedNullabilityCheck nullabilityCheck)
    {
        // Simple property tests
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableRefValueTypeProperty), 0, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableRefValueTypeProperty), 0, true);

        // Simple collection property tests
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableList1), 0, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableList1), 1, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableList2), 0, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableList2), 1, true);

        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableList1), 0, true);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableList1), 1, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableList2), 0, true);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableList2), 1, true);

        // List of list property tests
        // Non-nullable lists
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableListOfLists1), 0, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableListOfLists1), 1, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableListOfLists1), 2, false);

        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableListOfLists2), 0, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableListOfLists2), 1, true);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableListOfLists2), 2, false);

        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableListOfLists3), 0, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableListOfLists3), 1, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableListOfLists3), 2, true);

        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableListOfLists4), 0, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableListOfLists4), 1, true);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NonNullableListOfLists4), 2, true);

        // Nullable lists
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableListOfLists1), 0, true);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableListOfLists1), 1, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableListOfLists1), 2, false);

        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableListOfLists2), 0, true);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableListOfLists2), 1, true);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableListOfLists2), 2, false);

        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableListOfLists3), 0, true);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableListOfLists3), 1, false);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableListOfLists3), 2, true);

        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableListOfLists4), 0, true);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableListOfLists4), 1, true);
        AssertPropertyValueNullability(nullabilityCheck, nameof(TestClass1.NullableListOfLists4), 2, true);
    }

    private static void AssertPropertyValueNullability(IMicrosoftInternalApiBasedNullabilityCheck nullabilityCheck, string propertyName, int valueLevelInInProperty, bool expectedNullability)
    {
        var typeOfTestClass1 = typeof(TestClass1);

        var propertyInfo = typeOfTestClass1.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo == null)
        {
            throw new ApplicationException($"Property [{propertyName}] not found in [{typeOfTestClass1}].");
        }

        if (nullabilityCheck.IsValueNullable(propertyInfo.CustomAttributes, valueLevelInInProperty) != expectedNullability)
        {
            throw new ApplicationException($"Nullability test failed for: {nameof(propertyName)}={propertyName}, {nameof(valueLevelInInProperty)}={valueLevelInInProperty}, {nameof(expectedNullability)}={expectedNullability}.");
        }
    }

    //private static void TestGenericMethodParameterNullabilityCheckApi(IMicrosoftInternalApiBasedNullabilityCheck nullabilityCheck)
    //{
    //    var genericParameterValueNullabilityTester = new GenericParameterValueNullabilityTester(nullabilityCheck);

    //    genericParameterValueNullabilityTester.AssertValueNullability<TestClass2>(0, false);
    //    genericParameterValueNullabilityTester.AssertValueNullability<TestClass2?>(0, true);
    //}
}