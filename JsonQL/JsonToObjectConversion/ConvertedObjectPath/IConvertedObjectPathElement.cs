// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// <summary>
/// Represents an element in the path of a converted object.
/// This interface defines the core properties required to describe
/// an object path during JSON to object conversion.
/// </summary>
public interface IConvertedObjectPathElement
{
    /// <summary>
    /// Represents the name associated with a specific element in the path of a converted object.
    /// This typically refers to the property or field name that the path element points to.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Type of object selected by a path element.<br/>
    /// For example, in "Employees[1].Name",<br/>
    /// the first element "Employees" might have a type <see cref="IReadOnlyList{T}"/> where T is IEmployee,<br/>
    /// - the second element is index 1 with <see cref="ObjectType"/> equal to a type of IEmployee, since Employees[1] selects an employee of type IEmployee<br/>
    /// - the third element "Name" has a value <see cref="string"/> for <see cref="ObjectType"/>
    /// </summary>
    Type ObjectType { get; }
}