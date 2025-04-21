using System;

namespace JsonQL.JsonToObjectConversion;

public class CollectionItemTypeData
{
    public CollectionItemTypeData(Type itemType, bool isNullable)
    {
        ItemType = itemType;
        IsNullable = isNullable;
    }

    public Type ItemType { get; }

    public bool IsNullable { get; }
}