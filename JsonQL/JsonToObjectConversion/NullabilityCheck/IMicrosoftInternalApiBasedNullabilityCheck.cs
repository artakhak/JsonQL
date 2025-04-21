using System.Collections.ObjectModel;
using System.Reflection;
using JsonQL.JsonToObjectConversion.NullabilityCheck.Diagnostics;

namespace JsonQL.JsonToObjectConversion.NullabilityCheck;

/// <summary>
/// Nullability checks based on Microsoft internal API. Currently, there is no other better way to check property value nullability.
/// </summary>
public interface IMicrosoftInternalApiBasedNullabilityCheck
{
    /// <summary>
    /// Checks if value in property is nullable.
    /// </summary>
    /// <param name="valueCustomAttributes">Value custom attributes. This can be custom attributes of property, function parameter, generic type parameter, etc.</param>
    /// <param name="valueLevelInType">
    /// Level of value in type.<br/>
    /// For property value <paramref name="valueLevelInType"/> is 0.<br/>
    /// Example is property <b>Employees</b> in <b>IEnumerable&lt;IReadOnlyList&lt;IEmployee?&gt;?&gt; Employees {get; set}</b>.<br/>
    /// For collection item in property the value of <paramref name="valueLevelInType"/> is 1.<br/>
    /// Example is when checking nullability of <b>IReadOnlyList&lt;IEmployee?&gt;</b> in <b>IEnumerable&lt;IReadOnlyList&lt;IEmployee?&gt;?&gt; Employees {get; set}</b>.<br/>
    /// For collection item in another collection in property the value of <paramref name="valueLevelInType"/> is 2.<br/>
    /// Example is when checking nullability of <b>IEmployee</b> in <b>IEnumerable&lt;IReadOnlyList&lt;IEmployee?&gt;?&gt; Employees {get; set}</b>.<br/> 
    /// </param>
    /// <returns></returns>
    bool IsValueNullable(IEnumerable<CustomAttributeData> valueCustomAttributes, int valueLevelInType);
}

// Some Methods in this class use code modified
// from code in this post https://stackoverflow.com/questions/58453972/how-to-use-net-reflection-to-check-for-nullable-reference-type
/// <inheritdoc />
public sealed class MicrosoftInternalApiBasedNullabilityCheck: IMicrosoftInternalApiBasedNullabilityCheck
{
    private static readonly bool NullabilityChecksAreReliable = true;
    
    static MicrosoftInternalApiBasedNullabilityCheck()
    {
        var microsoftInternalApiBasedNullabilityCheck = new MicrosoftInternalApiBasedNullabilityCheck();
        NullabilityChecksAreReliable = NullableTypeHelpersTests.TestNullabilityCheckApi(microsoftInternalApiBasedNullabilityCheck);
    }

    /// <inheritdoc />
    public bool IsValueNullable(IEnumerable<CustomAttributeData> valueCustomAttributes, int valueLevelInType)
    {
        if (!NullabilityChecksAreReliable)
            return true;
       
        var nullableAttribute = valueCustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");

        if (nullableAttribute == null || nullableAttribute.ConstructorArguments.Count != 1)
            return false;

        var attributeArgument = nullableAttribute.ConstructorArguments[0];

        if (attributeArgument.ArgumentType == typeof(byte))
        {
            // Either the property is for non-array, or nullability for all level values is the same (either nullable or non-nullable)
            return (byte)attributeArgument.Value! == 2;
        }

        if (attributeArgument.ArgumentType != typeof(byte[]))
            return false;

        var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value!;

        // The first value is for collection property itself. 
        if (args.Count < valueLevelInType + 1)
            return false;

        return args[valueLevelInType].ArgumentType == typeof(byte) && (byte)args[valueLevelInType].Value! == 2;
    }

    //private bool? CheckIsNullableUsingNullableContextAttribute(IEnumerable<CustomAttributeData> customAttributes)
    //{
    //    var context = customAttributes.FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");

    //    if (context == null)
    //        return null;

    //    if (context is { ConstructorArguments.Count: 1 } &&
    //        context.ConstructorArguments[0].ArgumentType == typeof(byte))
    //    {
    //        return (byte)context.ConstructorArguments[0].Value! == 2;
    //    }

    //    return false;
    //}
}