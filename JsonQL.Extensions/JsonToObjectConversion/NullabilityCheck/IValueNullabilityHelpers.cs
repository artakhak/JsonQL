using System;
using System.Collections.Generic;
using System.Reflection;

namespace JsonQL.Extensions.JsonToObjectConversion.NullabilityCheck;

public interface IValueNullabilityHelpers
{
    /// <summary>
    /// Checks nullability of type.
    /// </summary>
    /// <param name="valueType">Value type.</param>
    /// <param name="valueCustomAttributes">Value custom attributes. This can be custom attributes of property, function parameter, generic type parameter, etc.</param>
    bool IsValueNullable(Type valueType, IEnumerable<CustomAttributeData> valueCustomAttributes);

    /// <summary>
    /// Checks nullability of item in collection type value.
    /// </summary>
    /// <param name="collectionItemType">Collection item type.</param>
    /// <param name="collectionItemLevel">Collection item level in property.<br/>
    /// For example to check the nullability of item <b>ReadOnlyList&lt;IEmployee?&gt;</b> in type <b>IEnumerable&lt;IReadOnlyList&lt;IEmployee?&gt;?&gt; Employees {get; set}</b> use 0.<br/>
    /// and to check the nullability of item <b>IEmployee</b> in type <b>IEnumerable&lt;IReadOnlyList&lt;IEmployee?&gt;?&gt; Employees {get; set}</b> use 1.<br/>
    /// </param>
    /// <param name="valueCustomAttributes">Value custom attributes. This can be custom attributes of property, function parameter, generic type parameter, etc.</param>
    bool AreCollectionItemsNullable(Type collectionItemType, int collectionItemLevel, IEnumerable<CustomAttributeData> valueCustomAttributes);
}

// Some Methods in this class use code modified
// from code in this post https://stackoverflow.com/questions/58453972/how-to-use-net-reflection-to-check-for-nullable-reference-type
/// <inheritdoc />
public class ValueNullabilityHelpers : IValueNullabilityHelpers
{
    private readonly IMicrosoftInternalApiBasedNullabilityCheck _microsoftInternalApiBasedNullabilityCheck;

    public ValueNullabilityHelpers(IMicrosoftInternalApiBasedNullabilityCheck microsoftInternalApiBasedNullabilityCheck)
    {
        _microsoftInternalApiBasedNullabilityCheck = microsoftInternalApiBasedNullabilityCheck;
    }

    /// <inheritdoc />
    public bool IsValueNullable(Type valueType, IEnumerable<CustomAttributeData> valueCustomAttributes)
    {
        if (valueType.IsValueType)
            // return propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            return Nullable.GetUnderlyingType(valueType) != null;

        return _microsoftInternalApiBasedNullabilityCheck.IsValueNullable(valueCustomAttributes, 0);
    }

    /// <inheritdoc />
    public bool AreCollectionItemsNullable(Type collectionItemType, int collectionItemLevel, IEnumerable<CustomAttributeData> valueCustomAttributes)
    {
        if (collectionItemType.IsValueType)
            return Nullable.GetUnderlyingType(collectionItemType) != null;

        return _microsoftInternalApiBasedNullabilityCheck.IsValueNullable(valueCustomAttributes, collectionItemLevel + 1);
    }
}