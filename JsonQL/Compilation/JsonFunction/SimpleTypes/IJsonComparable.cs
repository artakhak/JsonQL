// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Defines an interface for types that can be compared within a JSON-compatible context.
/// </summary>
public interface IJsonComparable
{
    /// <summary>
    /// Represents the type of the value encapsulated by an <see cref="IJsonComparable"/> instance.
    /// </summary>
    TypeCode TypeCode { get; }
  
    /// <summary>
    /// Represents the actual value encapsulated by an <see cref="IJsonComparable"/> instance.
    /// The value is one of the supported comparable types such as <see cref="string"/>, <see cref="double"/>,
    /// <see cref="bool"/>, or <see cref="DateTime"/>.
    /// </summary>
    IComparable Value { get; }
}

// /// <summary>
// /// Provides extension methods for working with objects implementing the <see cref="IJsonComparable"/> interface.
// /// </summary>
// public static class JsonComparableExtensions
// {
//     /// <summary>
//     /// Converts the value of the specified <see cref="IJsonComparable"/> instance to the requested type <typeparamref name="T"/>.
//     /// Throws an <see cref="InvalidCastException"/> if the conversion is not valid.
//     /// </summary>
//     /// <typeparam name="T">The target type to which the value should be converted.</typeparam>
//     /// <param name="jsonComparable">The <see cref="IJsonComparable"/> instance whose value is to be converted.</param>
//     /// <returns>The value of the specified <see cref="IJsonComparable"/> instance converted to type <typeparamref name="T"/>.</returns>
//     /// <exception cref="InvalidCastException">
//     /// Thrown when the value cannot be converted to the specified type <typeparamref name="T"/>.
//     /// </exception>
//     public static T ConvertValueOrThrow<T>(this IJsonComparable jsonComparable)
//     {
//         switch (jsonComparable.TypeCode)
//         {
//             case TypeCode.Boolean:
//                 if (typeof(T) == typeof(bool) && jsonComparable.Value is Boolean)
//                     return (T) jsonComparable.Value;
//                 break;
//
//             case TypeCode.String:
//                 if (typeof(T) == typeof(string) && jsonComparable.Value is String)
//                     return (T)jsonComparable.Value;
//                 break;
//
//             case TypeCode.Double:
//                 if (typeof(T) == typeof(double) && jsonComparable.Value is Double)
//                     return (T)jsonComparable.Value;
//                 break;
//
//             case TypeCode.DateTime:
//                 if (typeof(T) == typeof(DateTime) && jsonComparable.Value is DateTime)
//                     return (T)jsonComparable.Value;
//                 break;
//         }
//        
//         throw new InvalidCastException($"Value of type [{jsonComparable.Value.GetType()}] with type code [{jsonComparable.TypeCode}] cannot be converted to [{typeof(T)}].");
//     }
// }