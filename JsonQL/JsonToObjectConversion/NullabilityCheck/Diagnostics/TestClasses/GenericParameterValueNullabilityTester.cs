//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//namespace JsonQL.JsonToObjectConversion.NullabilityCheck.Diagnostics.TestClasses;

//public sealed class GenericParameterValueNullabilityTester
//{
//    private readonly IMicrosoftInternalApiBasedNullabilityCheck _nullabilityCheck;

//    public GenericParameterValueNullabilityTester(IMicrosoftInternalApiBasedNullabilityCheck nullabilityCheck)
//    {
//        _nullabilityCheck = nullabilityCheck;
//    }
//    public void AssertValueNullability<T>(int valueLevelInType, bool expectedNullability)
//    {
//        var thisType = this.GetType();
//        var convertMethods = thisType.GetMethods().Where(x =>
//            // Make sure we don't peek a method in a subclass
//            x.DeclaringType == thisType &&
//            x.Name.StartsWith(nameof(AssertValueNullability)) && x.IsPublic).ToList();

       
//        IReadOnlyList<CustomAttributeData> customAttributes = convertMethods.Count == 1 ? 
            
//            CustomAttributeData.GetCustomAttributes(convertMethods[0]).ToList() : Array.Empty<CustomAttributeData>();

//        var nullability = _nullabilityCheck.IsValueNullable(customAttributes, valueLevelInType);

//        if (nullability != expectedNullability)
//        {
//            throw new ApplicationException($"Method generic parameter value nullability test failed for type: {nameof(T)}={typeof(T)}, {nameof(valueLevelInType)}={valueLevelInType}, {nameof(expectedNullability)}={expectedNullability}.");
//        }

//    }
//}