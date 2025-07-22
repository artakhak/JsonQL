// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Provides mapping from one class to another to deserialize objects during JSON conversion.
/// </summary>
public interface IModelClassMapper
{
    /// <summary>
    /// Tries to map a model class to another class used in deserialized objects during JSON conversion.
    /// Note, this is not a DI service container and deals only with model classes.
    /// Normally, this mapping will provide mapping from interfaces to implementations, but this interface can also
    /// map non-interface abstract or non-abstract classes to subclasses too.
    /// A typical example is when trying to find implementation of IAddress when deserializing a JSON object into IEmployee class that has
    /// IAddress property
    /// </summary>
    /// <param name="modelClassToMap">Class to map.</param>
    /// <param name="implementation">Mapped implementation.</param>
    /// <returns></returns>
    bool TryMap(Type modelClassToMap, [NotNullWhen(true)] out Type? implementation);
}

/// <summary>
/// Implementation of <see cref="IModelClassMapper"/> that maps interfaces only trying to find an implementation <br/>
/// in the same assembly where the interface is declared using the following rules.<br/>
/// -If there is only one non-abstract implementation of interface in assembly, it will be used.<br/>
/// -If there are many implementations, the one that is in the same namespace as the interface and has similar name as the interface<br/>
/// but without "I" prefix, it will be used.
/// </summary>
public class ModelClassMapper : IModelClassMapper
{
    private readonly ConcurrentDictionary<Type, Type?> _cachedMapping = new();
    
    /// <inheritdoc />
    public bool TryMap(Type modelClassToMap, [NotNullWhen(true)] out Type? implementation)
    {
        implementation = null;

        if (!modelClassToMap.IsInterface)
            return false;
      
        implementation = _cachedMapping.GetOrAdd(modelClassToMap, _ =>
        {
            var implementations = modelClassToMap.Assembly.GetTypes()
                .Where(x => x.IsClass && x.GetInterfaces().Contains(modelClassToMap)).ToList();

            if (implementations.Count == 0)
                return null;

            if (implementations.Count == 1)
                return implementations[0];

            if (modelClassToMap.Name.Length < 2 || modelClassToMap.Name[0] != 'I')
                return null;

            var expectedImplementationName = modelClassToMap.Name.Substring(1);

            return implementations.FirstOrDefault(x => x.Namespace == modelClassToMap.Namespace &&
                                                x.Name == expectedImplementationName);
        });

        return implementation != null;
    }
}
