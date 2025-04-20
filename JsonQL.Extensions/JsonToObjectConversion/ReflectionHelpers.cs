using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonQL.Extensions.JsonToObjectConversion;

public static class ReflectionHelpers
{
    // this.GetType().GetMethods()[0].GetGenericArguments()[0].GetCustomAttributes()
    // 
    //public static IReadOnlyList<CustomAttributeData> GetMethodGenericArguments(MethodInfo methodInfo, int genericArgumentIndex)
    //{
    //    var genericArguments = methodInfo.GetGenericArguments();

    //    if (genericArguments.Length <= genericArgumentIndex)
    //        throw new ArgumentException($"Method [{methodInfo.Name}] has less than [{genericArgumentIndex + 1}] generic arguments", nameof(genericArguments));

    //    return genericArguments[genericArgumentIndex].GetCustomAttributes().Select(x => CustomAttributeData.GetCustomAttributes());
    //}
}