//using System;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using JsonQL.JsonObjects;

//namespace JsonQL.Extensions.JsonToObjectConversion;

///// <summary>
///// Maps type used to convert <see cref="IParsedJson"/> to an object to a different type, such as interface implementation or a subclass.
///// </summary>
///// <param name="defaultTypeToConvertParsedJsonTo">
///// Type to which <paramref name="convertedParsedJson"/> is converted to.
///// </param>
///// <param name="convertedParsedJson">Parsed json object being converted to ann instance of <paramref name="defaultTypeToConvertParsedJsonTo"/>.</param>
///// <param name="mappedType">Mapped type. Not null if returned value is true. Otherwise, the value is null.</param>
///// <returns>
///// Returns true, if type <paramref name="defaultTypeToConvertParsedJsonTo"/> should be replaced with <paramref name="mappedType"/> when converting <paramref name="convertedParsedJson"/>
///// to an instance of <paramref name="defaultTypeToConvertParsedJsonTo"/>.
///// </returns>
//public delegate bool TryMapDelegate(Type defaultTypeToConvertParsedJsonTo, IParsedJson convertedParsedJson, [NotNullWhen(true)] out Type? mappedType);


///// <summary>
///// Maps type used to convert <see cref="IParsedJson"/> to an object to a different type, such as interface implementation or a subclass.
///// </summary>
//public interface IParsedJsonBasedModelClassMapper
//{
//    /// <summary>
//    /// Maps type used to convert <see cref="IParsedJson"/> to an object to a different type, such as interface implementation or a subclass.
//    /// </summary>
//    /// <param name="defaultTypeToConvertParsedJsonTo">
//    /// Type to which <paramref name="convertedParsedJson"/> is converted to.
//    /// </param>
//    /// <param name="convertedParsedJson">Parsed json object being converted to ann instance of <paramref name="defaultTypeToConvertParsedJsonTo"/>.</param>
//    /// <param name="mappedType">Mapped type. Not null if returned value is true. Otherwise, the value is null.</param>
//    /// <returns>
//    /// Returns true, if type <paramref name="defaultTypeToConvertParsedJsonTo"/> should be replaced with <paramref name="mappedType"/> when converting <paramref name="convertedParsedJson"/>
//    /// to an instance of <paramref name="defaultTypeToConvertParsedJsonTo"/>.
//    /// </returns>
//    bool TryMap(Type defaultTypeToConvertParsedJsonTo, IParsedJson convertedParsedJson, [NotNullWhen(true)] out Type? mappedType);
//}

//public class ParsedJsonBasedModelClassMapper : IParsedJsonBasedModelClassMapper
//{
//    private readonly Dictionary<Type, IInterfaceToImplementationMapping> _interfaceToImplementationMappings = new();
//    private readonly TryMapDelegate? _customMapper;

//    public ParsedJsonBasedModelClassMapper(IReadOnlyList<IInterfaceToImplementationMapping> interfaceToImplementationMappings,
//        TryMapDelegate? customMapper = null)
//    {
//        foreach (var interfaceToImplementationMapping in interfaceToImplementationMappings)
//            _interfaceToImplementationMappings[interfaceToImplementationMapping.Interface] = interfaceToImplementationMapping;
        
//        _customMapper = customMapper;
//    }

//    public bool TryMap(Type defaultTypeToConvertParsedJsonTo, IParsedJson convertedParsedJson, [NotNullWhen(true)] out Type? mappedType)
//    {
//        if (_customMapper != null && _customMapper(defaultTypeToConvertParsedJsonTo, convertedParsedJson, out mappedType))
//            return true;

//        if (_interfaceToImplementationMappings.TryGetValue(defaultTypeToConvertParsedJsonTo, out var interfaceToImplementationMapping)) // ? interfaceToImplementationMapping.Implementation : null
//        {
//            mappedType = interfaceToImplementationMapping.Implementation;
//            return true;
//        }

//        mappedType = null;
//        return false;
//    }
//}