// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonObjects;

/// <summary>
/// Represents <see cref="IParsedValue"/> for which there is no real json object in evaluated JSON files.
/// Used for generating JSON objects dynamically. For example in expression
/// {"value": "$value(Employees.Select(x => 0.1* x.Salary)).Where(x => x > 100000)"}
/// the expression "0.1* x.Salary" is converted to <see cref="ParsedCalculatedSimpleValue"/> which implements <see cref="IParsedCalculatedValue"/>
/// </summary>
public interface IParsedCalculatedValue: IParsedValue
{

}