// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using BindingFlags = System.Reflection.BindingFlags;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Provides utility methods to assist with operations on collection types.
/// </summary>
public interface ICollectionTypeHelpers
{
    bool TryGetItemType(Type collectionType, [NotNullWhen(true)] out Type? collectionItemType);

    /// <summary>
    /// Converts collection in <param name="convertedCollection"></param> to collection of type <param name="convertedToCollectionType"></param>>
    /// </summary>
    /// <param name="convertedCollection">Collection to convert to type <param name="convertedToCollectionType"></param></param>
    /// <param name="convertedToCollectionType">Type to convert a collection to.</param>
    /// <param name="convertedCollectionItemType">Item type of collection in collection <paramref name="convertedCollection"/>.</param>
    /// <exception cref="JsonConversionException">Throws this exception if conversion fails.</exception>
    object ConvertToCollection(IEnumerable<object?> convertedCollection, Type convertedToCollectionType, Type convertedCollectionItemType);
}

/// <inheritdoc />
public class CollectionTypeHelpers : ICollectionTypeHelpers
{
    /// <inheritdoc />
    public bool TryGetItemType(Type collectionType, [NotNullWhen(true)] out Type? collectionItemType)
    {
        collectionItemType = null;

        bool TryGetItemTypeLocal(Type enumerableType, [NotNullWhen(true)] out Type? collectionItemType2)
        {
            collectionItemType2 = null;
            if (enumerableType.IsGenericType && enumerableType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                collectionItemType2 = enumerableType.GetGenericArguments()[0];
                return true;
            }

            return false;
        }

        // Check if the type implements IEnumerable
        if (typeof(IEnumerable).IsAssignableFrom(collectionType))
        {
            if (TryGetItemTypeLocal(collectionType, out collectionItemType))
                return true;
        }

        // Check for implemented interfaces
        var interfaces = collectionType.GetInterfaces();
        foreach (var implementedInterface in interfaces)
        {
            if (TryGetItemTypeLocal(implementedInterface, out collectionItemType))
                return true;
        }

        return false;
    }

    /// <inheritdoc />
    public object ConvertToCollection(IEnumerable<object?> convertedCollection, Type convertedToCollectionType, Type convertedCollectionItemType)
    {
        Exception GetConversionErrorException(object item, int index)
        {
            return new JsonConversionException($"Conversion to [{convertedToCollectionType}] failed since some items in source collection are not convertible to [{item}]. Collection item index is {index}");
        }
        
        void AddSourceCollectionItemsToGeneratedCollection(object destinationCollection, Type convertedToCollectionItemType)
        {
            var addItemMethodInfo = destinationCollection.GetType().GetMethod("Add", BindingFlags.Public | BindingFlags.Instance, new Type[]
            {
                convertedToCollectionItemType
            });

            if (addItemMethodInfo == null)
                throw new JsonConversionException($"Conversion to [{convertedToCollectionType}] failed. Invalid type. Should implement System.Collections.Generic.ICollection<{convertedToCollectionItemType.FullName}>.");

            int index = 0;

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var collectionItem in convertedCollection)
            {
                if (collectionItem == null)
                {
                    addItemMethodInfo.Invoke(destinationCollection, new object?[] {null});
                }
                else if (convertedToCollectionItemType.IsInstanceOfType(collectionItem))
                {
                    addItemMethodInfo.Invoke(destinationCollection, new[] { collectionItem });
                }
                else
                {
                    throw GetConversionErrorException(collectionItem, index);
                }

                ++index;
            }
        }

        // Case when convertedToCollectionType is an interface
        if (convertedToCollectionType.IsInterface)
        {
            if (!convertedToCollectionType.IsGenericType)
                throw new JsonConversionException($"Type [{convertedToCollectionType}] is an interface but is not a generic type.");

            var genericArguments = convertedToCollectionType.GetGenericArguments();

            if (genericArguments.Length != 1)
                throw new JsonConversionException($"Type [{convertedToCollectionType}] is an interface and is a generic type but does not have exactly one generic argument.");

            var collectionItemType = genericArguments[0];

            if (!collectionItemType.IsAssignableFrom(convertedCollectionItemType))
                throw new JsonConversionException($"Conversion to [{convertedToCollectionType}] failed since collection item type of [{convertedCollectionItemType}] is not of type [{convertedToCollectionType}].");
            
            var getGenericTypeDefinition = convertedToCollectionType.GetGenericTypeDefinition();

            if (!(getGenericTypeDefinition == typeof(IEnumerable<>) || 
                  getGenericTypeDefinition == typeof(IReadOnlyList<>) ||
                  getGenericTypeDefinition == typeof(IList<>) ||
                  getGenericTypeDefinition == typeof(ICollection<>) ||
                  getGenericTypeDefinition == typeof(IReadOnlyCollection<>)
                  ))
            {
                throw new JsonConversionException(string.Format("Type [{0}] is expected to be one of the following: [{1}]",
                    convertedToCollectionType, string.Join(",", new List<Type>
                    {
                        typeof(IEnumerable<>), typeof(IReadOnlyList<>)
                    }.Select(x => x.FullName))));
            }

            var values = CreateListInstance(collectionItemType);
            AddSourceCollectionItemsToGeneratedCollection(values, collectionItemType); //, itemsAreNullable);

            return values;
        }

        // Case when convertedToCollectionType ins not an interface
        if (convertedToCollectionType.IsAbstract)
            throw new JsonConversionException($"Conversion to [{convertedToCollectionType}] failed. The type should be either interface or should be non-abstract class.");

        if (convertedToCollectionType.IsArray)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            var collectionItemsList = new List<object?>(convertedCollection);

            var array = Array.CreateInstance(convertedCollectionItemType, collectionItemsList.Count);

            for (var i = 0; i < collectionItemsList.Count; ++i)
            {
                var collectionItem = collectionItemsList[i];

                if (collectionItem == null || convertedCollectionItemType.IsInstanceOfType(collectionItem))
                {
                    array.SetValue(collectionItem, i);
                }
                else
                {
                    throw GetConversionErrorException(collectionItem, i);
                }
            }

            return array;
        }
        
        var defaultConstructor =
            convertedToCollectionType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, Type.EmptyTypes);

        if (defaultConstructor == null)
            throw new JsonConversionException($"Conversion to [{convertedToCollectionType}] failed. The type should has no default public constructor.");
        
        foreach (var implementedInterface in convertedToCollectionType.GetInterfaces())
        {
            var genericTypeDefinition = implementedInterface.GetGenericTypeDefinition();

            if (genericTypeDefinition != typeof(ICollection<>))
                continue;

            var genericArguments = convertedToCollectionType.GetGenericArguments();

            if (genericArguments.Length != 1)
                continue;

            var collectionItemType = genericArguments[0];

            if (!collectionItemType.IsAssignableFrom(convertedCollectionItemType))
                continue;
           
            var collectionInstance = defaultConstructor.Invoke(Array.Empty<object>());
            AddSourceCollectionItemsToGeneratedCollection(collectionInstance, collectionItemType); //, itemsAreNullable);
            return collectionInstance;
        }

        throw new JsonConversionException($"Conversion to [{convertedToCollectionType}] failed.");
    }

    private static object CreateListInstance(Type itemType)
    {
        Type listType = typeof(List<>).MakeGenericType(itemType);

        var list = Activator.CreateInstance(listType);

        if (list == null)
            throw new InvalidOperationException($"Failed to create an instance of list of [{nameof(itemType)}]=[{itemType}]");

        return list;
    }
}