## NOTE, this document is a very high-level description of **JsonQL**. For better examples and documentation, please refer to these resources:

- Examples in JsonQL.Demo in [https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos](https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos).
- Unit tests in JsonQL.Tests in [https://github.com/artakhak/JsonQL/tree/main/JsonQL.Tests](https://github.com/artakhak/JsonQL/tree/main/JsonQL.Tests).
- Documentation **(under construction...)** in [https://jsonql.readthedocs.io/en/latest/](https://jsonql.readthedocs.io/en/latest/).

## Overview

**JsonQL** is a powerful JSON query language implementation that provides a flexible way to query and manipulate JSON data using a SQL/Linq-like syntax with rich function support.

- All aspects of implementations are extensible (custom operators, functions, path elements, etc. can be added). JsonQL expressions are used in JSON texts and are parsed by JsonQL library.
- Allows using JsonQL expressions in one or more JSON files to mutate JSON files. JsonQL parses the mutated JSON files with JsonQL expressions to generate a JSON structure.
- Supports executing JsonQL queries against one or more JSON files. The query result is converted either to C# model classes (depending on API used).

  **NOTE**: Model classes used for deserialization can be either C# classes or interfaces, and the properties can be of class or interface types. JsonQL will either use default implementations of interfaces or will use classes specified by the developer.

- Errors are reported in JsonQL error classes that have error position data as well as additional data.

## Features

- Rich set of built-in functions for JSON manipulation
- Support for aggregate functions (COUNT, AVG, MIN, MAX, SUM)
- Collection manipulation with ANY and ALL operations
- String operations (ToUpper, ToLower, Length, Concatenate, etc)
- Type conversion functions (DateTime, Date, Double, Int, Boolean, String)
- Mathematical operations (Abs, IsEven, IsOdd)
- Object property inspection (HasField)
- Lambda expression support for complex queries
- Extensible function architecture
- Built-in conversion of query results to C# objects  
  **NOTE: JsonQL binds interfaces to default implementations and also supports binding interfaces to any class via configuration**
- Mutating JSON files by replacing JSON field values by evaluated JSON values
- Extending the API to provide custom operators, functions, as well as customizing any part of JsonQL implementation

## JSON Path Elements (custom JSON path elements can be implemented)

```markdown
- `Array indexes`   
  Examples: "$(Object1.Array1[0])", "$(parent.Object1.Array2[4, 1])"

- `Where`           - Applied to JSON objects to filter out JSON objects.
  Examples: "$merge(Object1.Array1.Where(x => x >= 2 && x <= 6))"

- `Select`          - Applied to map JSON objects to other JSON objects.
  Examples: "$merge(Object1.Select(x => x.Object2.Where(y => HasField(y, 'Value'))))"

- `Flatten`         - Applied to flatten multidimensional arrays.
  Examples: "$merge(Object1.Array1.Flatten().Where(x => x >= 4 && x <= 19))"

- `First`           - Applied to select the first item in collection.
  Examples: "$(Object1.Array1.First())" or "$(Object1.Array1.First(x => x > 1))"

- `Last`            - Applied to select the last item in collection.
  Examples: "$(Object1.Array1.Where(x => x.Value > 10).Last())" or "$(Object1.Array1.Last(x => x > 1))"

- `Reverse`         - Applied to reverse the collection.
  Examples: "$merge(Object1.Array1.Flatten().Where(x => x >= 2 && x <= 6).Reverse())"
```

## Mutation operators

```markdown
- `$copyFields`     - Copies fields in one JSON object into another JSON object.
  Examples: {"Object1": {"replaceWithCopiedFields": "$copyFields(parent.Examples.Object1)", "Field2": 1 }}

- `$merge`          - Merges items in one array into another array.
  Examples: { "Array1": [1, "$merge(parent.Where(x => Count(x) >= 2 && Any(x, y => y.Capitalization > 300)).Flatten().Where(x => x.Age > 60))", 3] }

- `$value`          - Replaces a JSON field value with evaluated value.
  Examples: {"Employees": "$value(Example.Employees.Where(x => x.Salary > 100000))"}

- `$`               - String interpolation mutator operator.
  Examples: { "MyCalculatedValue": "$(parent.Array1[1, 1000]:parent.Array1[1, 2]) is 6"}
```

## Functions (custom functions can be implemented)

### Aggregate Functions

```markdown
- `Count()` - Counts elements in a collection
  Examples: {"array1": [1, "$merge(parent.Object1.Array2.Where(x => Count(x) > 3)), 2]}

- `Average()` - Calculates average of numeric values
  Examples: {"array1": [1, $merge(parent.Object1.Array2.Where(x => Average(x, y => typeof y == 'Number' && y % 2 == 0) >= 8))", 2]}

- `Min()` - Finds minimum value
  Examples: {"field1": "$(Min(Object1.Array1.Where(x => typeof x == 'Number'), x => x % 2 == 0)) is 2"}

- `Max()` - Finds maximum value
  Examples: {"field1": "$(Max(parent.Object1.Companies.Flatten().Where(x => HasField(x, 'EmployeeId')), x => x.Salary < 120000, x => x.Salary)) is 110000"}

- `Sum()` - Calculates sum of numeric values
  Examples: {"field1": "$(Sum(parent.Object1.Array2.Flatten(), x => x < 19))"}

- `All()` - Tests if all elements match a condition
  Examples: {"AllEmployeesEarnMoreThan_60000": "$(All(parent.Object1.Companies.Flatten().Where(x => HasField(x, 'EmployeeId')), x => x.Salary > 60000))"}

- `Any()` - Tests if any elements match a condition
  Examples: {"CompaniesWithEmployeeWithSalaryOf_88000": [ "$merge(Companies.Where(x => Any(x.Employees, y => y.Salary == 88000)))" ]}
```

### String Functions
```markdown
- `Lower()` - Converts text to lowercase
- `Upper()` - Converts text to uppercase
- `Length()` - Returns text length
- `Concatenate()` - Joins multiple strings
```

### Conversion Functions
```markdown
- `ConvertToDateTime()` - Converts to DateTime
- `ConvertToDate()` - Converts to Date
- `ConvertToDouble()` - Converts to Double
- `ConvertToInt()` - Converts to Integer
- `ConvertToBoolean()` - Converts to Boolean
- `ConvertToString()` - Converts to String
```

### Mathematical Functions
```markdown
- `Abs()` - Returns absolute value
- `IsEven()` - Checks if number is even
- `IsOdd()` - Checks if number is odd

### Object Functions
- `HasField()` - Checks if JSON object has specified field
```

## Operators (custom operators can be implemented)
```markdown
- `.`				 - Accesses field value
- `!`				 - Negate operator
- `==`				 - Equals operator
- `!=`				 - Is not equal operator
- `>`				 - Greater than operator
- `>=`				 - Greater than or equal operator
- `<`				 - Less than operator
- `<=`				 - Less than or equal operator
- `&&`				 - Logical 'and' operator
- `*`				 - Multiply operator
- `/`				 - Divide operator
- `+`				 - Add operator
- `-`				 - Subtract binary operator or negative number unary operator based on where it is used. Examples: "-5", "a.Age-5".
- `%`				 - Quotient operator
- `->`				 - Named parameter specification operator. Allows changing the order of parameters. Useful with some parameters being optional. Example: ReverseTextAndAddMarkers(addMarkers->false, value->TestData[4])
- `=>`				 - Lambda operator
- `contains`		 - Contains operator
- `starts with`		 - Starts with operator
- `ends with`		 - Ends with operator
- `contains`		 - Contains operator
- `is null`			 - 'is null' operator
- `is not null`		 - 'is not null' operator
- `is undefined`	 - 'is undefined' operator
- `is not undefined` - 'is not undefined' operator
- `typeof`			 - 'typeof' operator. Example "typeof person.Age"
```

## Using JsonQL Expressions to Mutate JSON Files

- JsonQL expressions are used in one or many JSON files. JsonQL evaluates JsonQL expressions and loads the parsed JSON files with expressions replaced with calculated JSON objects into an instance of [JsonQL.Compilation.ICompilationResult](https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs).
- The property **CompiledJsonFiles** contains a collection of [JsonQL.Compilation.ICompiledJsonData](https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompiledJsonData.cs): one per loaded file. 
- [JsonQL.Compilation.ICompiledJsonData](https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompiledJsonData.cs) represents mutated JSON files (i.e., mutated by using JsonQL expressions).  
- The property **CompilationErrors** contains a collection of [JsonQL.Compilation.ICompilationErrorItem](https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationErrorItem.cs) with error details if any. 
- If many JSON files are specified, the following rules and techniques are used:
  - Parent/child relationships between JSON files are maintained, and parent JSON files are evaluated before child JSON files are evaluated.
  - Lookup of JSON values specified in JsonQL expressions starts in JSON containing the expression first, and then in parent JSON files.


### Example: JsonQL expressions to mutate JSON Files

An overview example of mutating multiple JSON files is [Overview](https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview).

In this example the following JSON files are processed with JSON files appearing earlier being processed as parents JSON files appearing later:

  - [AdditionalTestData.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview/AdditionalTestData.json)
  - [Countries.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview/Countries.json)
  - [Companies.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview/Companies.json)
  - [Overview.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview/Overview.json)

**The file [Overview.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview/Overview.json) file with JsonQL expressions is shown below.**

```json
{
  "CountryNamesWithPopulationOf80MlnOrMore": "$value(Countries.Where(x => x.Population >= 80000000).Select(x => x.Name))",
  "CompanyDetailsWithAnyEmployeeWithNullAddress": "$value(Companies.Where(x => Any(x.Employees, x => x.Address is null)).Select(x => x.CompanyData))",
  "EmployeesWithNonEmptyLoginsList": "$value(Companies.Select(x => x.Employees.Where(x => Count(x.Logins) > 0)))",
  "FirstEmployeeWithLoginsField": "$value(Companies.Select(x => x.Employees.Where(x => HasField(x, 'Logins'))).First(x => x.Id != 100000005))",
  "LastEmployeeWithLoginsField": "$value(Companies.Select(x => x.Employees.Where(x => HasField(x, 'Logins'))).Last())",
  "FlattenExamples": {
    // Flattening arrays can be applied multiple times to execute sophisticated queries
    "FlattenArraysAndFilterObjects": "$value(MultiDimensionalMatrix.Flatten().Where(x => x > 3 || x.Name == 'John' || x.CompanyName starts with 'Sherwood'))",
    "GetAllNumericValuesInArraysFiltered": "$value(MultiDimensionalMatrix.Flatten().Flatten(x => typeof x != 'JsonArray' || All(x, y => y != 12)).Where(x => typeof x == 'Number'))"
  },
  // If CompanyData.CEO field is missing or value is null for second company, compilation will fail.
  "SelectSecondCompanyCeoNameWithAssertionThatCeoNameFieldIsPresetAndNotNull": "$(Companies[1].CompanyData.CEO assert)",

  // Example of merging fields into a JSON object. EnhancedCompanyData has fields in CompanyData for company with Name that starts with text 'Sherwood'
  // enhanced with additonal fields
  "EnhancedCompanyData": {
    "Capitalization": 1000000,
    "CompanyDataCopied": "$copyFields(Companies.First(x => x.CompanyData.Name starts with 'Sherwood').CompanyData)",
    "NumberOfEmployees": "$(Count(Companies.First(x => x.CompanyData.Name starts with 'Sherwood').Employees))"
  },

  // Example of merging array items into an array
  // List of company data for companies with  max salary of employees at least 110000 will be merged into array ListOfCompaniesEnhancedWithNewCompanies
  "ListOfCompaniesEnhancedWithNewCompanies": [
    {
      "Name": "Some company 1",
      "CEO": "Napoleon Bonaparte"
    },
    // Merge data of companies with max salary of employees at least 110000
    "$merge(Companies.Where(x => Max(x.Employees, y => true, y => y.Salary) > 110000).Select(x => x.CompanyData))",
    {
      "Name": "Some company 2",
      "CEO": "Georges Danton"
    }
  ],
  // Example of simple value using mutator function "$". In contrast to "$value" it generates a simple value (string, numeroc, etc) and not an object.
  "AverageSalaryOfAllEmployeesInFilteredCompanies": "$(Average(Companies.Where(x => !(x.CompanyData.Name starts with 'Sherwood')).Select(x => x.Employees.Select(x => x.Salary))))",
  "AddressingObjectsInMultiDimensionalMatrix": "$value(MultiDimensionalMatrix[1, 2].Address)",

  // Examples of using contextual data such a 'index' as well as example of referencing parent lamdba function parameter
  "EmployeeNamesInSecondCompanyExcludingFirstAndLastEmployee": "$value(Companies[1].Select(x => x.Employees.Where(y => index != 0 && index != Count(x.Employees) - 1).Select(x => x.Name)))",
  // We can use named parameters to change the order of parameters, as well as not specified optional parameter that are before parameters we pass
  // In this example both criteria and value are optional, and criteria is before value parameter. By using named parameter value, we can ommit criteria parameter
  "MaxRaisedSalaryAccrossAllCompaniesUsingNamedParameter": "$value(Max(Companies.Select(x => x.Employees).Flatten(), value->x=>x.Salary*1.05))",

  // isReverseSearch named parameter does not have to be used here, and we can use true here, since this parameter comes after the predicate. 
  // However, it makes the meaning clear.
  "ThirdEmployeeFromEndWithSalaryGreaterThan_100000_AcrossAllCompanies": "$value(Companies.Select(x => x.Employees).Flatten().At(2, x => x.Salary >= 100000 && x.Id != 100000008, isReverseSearch->true))",

  "UseOf_this_ToEnsureObjectIsLookedUpInCurrentFileAndNotInParentFiles": "$(this.CountryNamesWithPopulationOf80MlnOrMore[1])",
  "UseOf_parent_ToEnsureObjectIsLookedUpInParentFilesAndSearchInCurrentFilesIsSkipped": "$value(Count(parent.Companies, x => x.CompanyData.CEO != 'John Malkowich'))",
  "EmployeeWithNonNullIdAndNullAddress": "$value(Companies.Select(x => x.Employees).Flatten().Where(x => x.Address is null && x.Id is not null))",
  "NumberOfEmployeesWithout_Logins_Field": "$(Count(Companies.Select(x => x.Employees).Flatten().Where(x => x.Logins is undefined)))",
  "NumberOfEmployeesWith_Logins_Field": "$(Count(Companies.Select(x => x.Employees).Flatten().Where(x => x.Logins is not undefined)))",
  "InvalidValuesEvaluateToUndefined": "$(Companies[0].Employees[1000].Salary is undefined) = true",
  "ConversionExample_Datetime": "'2022-05-22T18:25:43.511Z' < '2022-05-23T18:25:43.511Z'=$(ToDateTime('2022-05-22T18:25:43.511Z') < ToDateTime('2022-05-23T18:25:43.511Z'))",
  "DefaultValue": "Defaulted salary to $(Companies[0].Employees[1000].Salary:100000)=100000"
}

```

**C# code example that parses the JSON files above**

```csharp
var additionalTestData = new JsonTextData(
    "AdditionalTestData",
    this.LoadExampleJsonFile("AdditionalTestData.json"));

var companiesJsonTextData = new JsonTextData("Companies",
    this.LoadExampleJsonFile("Companies.json"), countriesJsonTextData);

// Set the value of queryManager to an instance of JsonQL.Compilation.JsonCompiler here.
// The value of JsonQL.Compilation.JsonCompiler is normally created by Dependency Injection container 
// and it is normally configured as a singleton.
JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

var countriesJsonTextData = new JsonTextData(
    "Countries",
    this.LoadExampleJsonFile("Countries.json"), additionalTestData);
    
var result = jsonCompiler.Compile(new JsonTextData("Overview",
    this.LoadExampleJsonFile("Overview.json"), companiesJsonTextData));

```

- Result of a query above for can be found here: [Result.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview/Result.json)

**Note** The serialized result in [Result.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview/Result.json) was formatted to show only mutated JSON for [Overview.json](https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview/Overview.json) to make the file smaller.

## JsonQL queries of JSON Files with Result Converted to C# objects

- The overloaded methods **QueryObject** in interface [JsonQL.Query.IQueryManager](https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>) and similarly named overloaded extension methods with generic parameter **QueryObject<T>** in [JsonQL.Query.QueryManagerExtensions](https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/QueryManagerExtensions.cs) can be used to to query one or more JSON files using a JsonQL query expressions.

  **NOTE**: The extension methods with generic parameter **T** are easier to use. The methods in [JsonQL.Query.IQueryManager](https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs) might be easier to use with reflection. Moving forward the extension methods will be discussed.

- The result is converted to [JsonQL.Query.IObjectQueryResult<T>](https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs) a C# interface of class specified in generic parameter where **T** is the generic type argument, which implement used in call to **IJsonQL.Query.QueryManager.QueryObject<T>** method.
- The result stores the query result converted to type **T** as well as data about errors encountered during execution of the query.
- The type parameter **T** specified in query method specifies the return object type from query. It can be any class (value of reference type) including collection types.
- If collection type is used for type parameter **T** in in call to **IJsonQL.Query.QueryManager.QueryObject<T>** method, the collection item parameters can be interfaces or classes as well  (value of reference type). 
- Nullable syntax '?' can be specified for return type (including collection item types, if return type is a collection).
- One ore more JSON files can be specified as parameters to be used when looking up JSON values referenced by JsonQL expressions.
- If many JSON files are specified the following rules and techniques are used:
  - Parent/child relationships between JSON files is maintained and parent JSON files are evaluated before child JSON files are evaluated.
  - Lookup of JSON values specified in JsonQL expressions starts in JSON containing the expression first, and then in parent JSON files.

### Example: Query and convert JSON to C# objects

```csharp
// Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
// The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
// and it is normally configured as a singleton.
IQueryManager queryManager = null!; 

// NOTE: Data.json has a root JSON with a collection of employees. 
// If the JSON had a JSON object with the "Employees" field, the
// query would be: "Employees.Where(...)" instead of "Where(...)"
var query = "Where(e => e.Id==100000006 || e.Id==100000007 || Any(EmployeeIds, p => p == e.Id))";

// We can call _queryManager.QueryObject<T> with the following values for "T" generic parameter
// -Class (value or reference type). We can use '?' for nullable values. Examples:
//      "_queryManager.QueryObject<Manager?>(...)",
//      "_queryManager.QueryObject<Manager>(...)"
// -Interface. We can use '?' for nullable values. Examples:
//      "_queryManager.QueryObject<IManager?>(...)",
//      "_queryManager.QueryObject<IManager>(...)"
// The following collection types:
//          IReadOnlyList<T>, IEnumerable<T>, IList<T>, 
//          ICollection<T>, IReadOnlyCollection<T>
// -Any type that implements ICollection<T>. Example: List<T>, Array T[]
// If collection type is used for "T", "T" can be either an object (value or reference type)
// or another collection listed above. Also, nullability keyword "?" can be used for
// collection items as well as for collection type itself.
// The result "employeesResult" is of type "JsonQL.Query.IObjectQueryResult<IReadOnlyList<IEmployee>>".
var employeesResult =
    queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json"),
                    new JsonTextData("Parameters", this.LoadExampleJsonFile("Parameters.json"))),
        [false, false], new JsonConversionSettingsOverrides
        {
            TryMapJsonConversionType = (type, parsedJson) =>
            {
                // If we always return null, or just do not set the value, of TryMapJsonConversionType
                // IEmployee will always be bound to Employee
                // In this example, we ensure that if parsed JSON has "Employees" field,
                // then the default implementation of IManager (i.e., Manager) is used to
                // deserialize the JSON.
                // We can also specify Manager explicitly.
                if (parsedJson.HasKey(nameof(IManager.Employees)))
                    return typeof(IManager);
                return null;
            }
        });

// This example is copied from https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/ResultAsNonNullableEmployeesList/Example.cs
```
- Files evaluated in JsonQL query above are listed here:
   - [Data.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/ResultAsNonNullableEmployeesList/Data.json)

- Result of query above can be found here: [Result.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/ResultAsNonNullableEmployeesList/Result.json)
- Example classes and JSON files for this example can be found [here](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/ResultAsNonNullableEmployeesList)

### Example: Query and convert JSON to collection of double values

```csharp
// Set the value of queryManager to an instance of JsonQL.Compilation.JsonCompiler here.
// The value of JsonQL.Compilation.JsonCompiler is normally created by Dependency Injection container 
// and it is normally configured as a singleton.
IQueryManager queryManager = null!;

var salariesOfAllEmployeesOlderThan35InAllCompaniesQuery = 
    "Companies.Select(x => x.Employees.Where(x => x.Age > 35).Select(x => x.Salary))";
    
var salariesResult =
    queryManager.QueryObject<IReadOnlyList<double>>(salariesOfAllEmployeesOlderThan35InAllCompaniesQuery,
        new JsonTextData("Data",
            this.LoadExampleJsonFile("Data.json")), null);
```
- Files evaluated in JsonQL query above are listed here:
   - [Data.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/SalariesOfAllEmployeesInAllCompaniesAsReadOnlyListOfDoubles/Data.json)

- Result of query above can be found here: [Result.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/SalariesOfAllEmployeesInAllCompaniesAsReadOnlyListOfDoubles/Result.json)
- Example classes and JSON files for this example can be found [here](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/SalariesOfAllEmployeesInAllCompaniesAsReadOnlyListOfDoubles)

## JsonQL queries of JSON Files with Result Converted to JSON structure

- The interface [JsonQL.Query.IQueryManager](https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs) and its extensions are used to query one or more JSON files using a JsonQL query expression.
- The result is converted to [JsonQL.Query.IJsonValueQueryResult](https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IJsonValueQueryResult.cs).
- The result stores the query result as a JSON structure as well as data about errors encountered during execution of the query.
- One ore more JSON files can be specified as parameters to be used when looking up JSON values referenced by JsonQL expressions.
- If many JSON files are specified the the following rules and techniques are used:
  - Parent/child relationships between JSON files is maintained and parent JSON files are evaluated before child JSON files are evaluated.
  - Lookup of JSON values specified in JsonQL expressions starts in JSON containing the expression first, and then in parent JSON files.

### Example: Query JSON files with result as JSON structure

```csharp
// Set the value of queryManager to an instance of JsonQL.Compilation.JsonCompiler here.
// The value of JsonQL.Compilation.JsonCompiler is normally created by Dependency Injection container 
// and it is normally configured as a singleton.
IQueryManager queryManager = null!;

var salariesOfAllEmployeesOlderThan35InAllCompaniesQuery = 
    "Companies.Select(x => x.Employees.Where(x => x.Age > 35).Select(x => x.Salary))";
    
var salariesResult =
    queryManager.QueryObject<IReadOnlyList<double>>(salariesOfAllEmployeesOlderThan35InAllCompaniesQuery,
        new JsonTextData("Data",
            this.LoadExampleJsonFile("Data.json")), null);
```
- Files evaluated in JsonQL query above are listed here:
   - [Data.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsParsedJsonValue/CompaniesWithLimitOnMaxSalary/Data.json)

- Result of query above can be found here: [Result.json](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsParsedJsonValue/CompaniesWithLimitOnMaxSalary/Result.json)
- Example classes and JSON files for this example can be found [here](https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsParsedJsonValue/CompaniesWithLimitOnMaxSalary)

## License

This project is licensed under the MIT License - see the LICENSE file in the solution root for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
