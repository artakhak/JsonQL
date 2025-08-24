## NOTE, following is a very high level description of **JsonQL**. For more details please refer to examples in JsonQL.Demo and JsonQL.Tests projects in [https://github.com/artakhak/JsonQL](https://github.com/artakhak/JsonQL).

```markdown

# JsonQL

JsonQL is a powerful JSON query language implementation that provides a flexible way to query and manipulate JSON data using a SQL/Linq-like syntax with rich function support.

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
  -Also, allows auto-binding interfaces to default implementations or non-default implementations via configuration
- Mutating JSON files by replacing Json field values by evaluated Json values
- Extending the API to provide custom operators, functions, as well as customizing any part of JsonQL implementation

## Json Path Elements
### Built-in Path Elements (new functions can be added)
- `Array indexes`   
  Examples: "$(Object1.Array1[0])", "$(parent.Object1.Array2[4, 1])"

- `Where`           - Applied to json objects to filter out Json objects.
  Examples: "$merge(Object1.Array1.Where(x => x >= 2 && x <= 6))"

- `Select`          - Applied to map Json objects to other json objects.
  Examples: "$merge(Object1.Select(x => x.Object2.Where(y => HasField(y, 'Value'))))"

- `Flatten`         - Applied to flatten multidimensional arrays.
  Examples: "$merge(Object1.Array1.Flatten().Where(x => x >= 4 && x <= 19))"

- `First`           - Applied to select the first item in collection.
  Examples: "$(Object1.Array1.First())" or "$(Object1.Array1.First(x => x > 1))"

- `Last`            - Applied to select the last item in collection.
  Examples: "$(Object1.Array1.Where(x => x.Value > 10).Last())" or "$(Object1.Array1.Last(x => x > 1))"

- `Reverse`         - Applied to reverse the collection.
  Examples: "$merge(Object1.Array1.Flatten().Where(x => x >= 2 && x <= 6).Reverse())"

## Mutation operators.
- `$copyFields`     - Copies fields in one Json object into another Json object.
  Examples: {"Object1": {"replaceWithCopiedFields": "$copyFields(parent.Examples.Object1)", "Field2": 1 }}

- `$merge`          - Merges items in one array into another array.
  Examples: { "Array1": [1, "$merge(parent.Where(x => Count(x) >= 2 && Any(x, y => y.Capitalization > 300)).Flatten().Where(x => x.Age > 60))", 3] }

- `$value`          - Replaces a Json field value with evaluated value.
  Examples: {"Employees": "$value(Example.Employees.Where(x => x.Salary > 100000))"}

- `$`               - String interpolation mutator operator.
  Examples: { "MyCalculatedValue": "$(parent.Array1[1, 1000]:parent.Array1[1, 2]) is 6"}


## Built-in Functions

### Aggregate Functions
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

### String Functions
- `Lower()` - Converts text to lowercase
- `Upper()` - Converts text to uppercase
- `Length()` - Returns text length
- `Concatenate()` - Joins multiple strings

### Conversion Functions
- `ConvertToDateTime()` - Converts to DateTime
- `ConvertToDate()` - Converts to Date
- `ConvertToDouble()` - Converts to Double
- `ConvertToInt()` - Converts to Integer
- `ConvertToBoolean()` - Converts to Boolean
- `ConvertToString()` - Converts to String

### Mathematical Functions
- `Abs()` - Returns absolute value
- `IsEven()` - Checks if number is even
- `IsOdd()` - Checks if number is odd

### Object Functions
- `HasField()` - Checks if JSON object has specified field


## Built-in Operators
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
- `-`				 - Subtract operator or negative number operator depedning where it is used. Exampels: "-5", "a.Age-5".
- `%`				 - Quotient operator
- `->`				 - Named parameter specification operator. Allows changing the odrer of parameters. Usefull with some parameters being optional. Example: ReverseTextAndAddMarkers(addMarkers->false, value->TestData[4])
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

NOTE: Documentation will be improved in near future to demonstrate good exanples. Before that is done, reference examples in unit tests in project JosnQL.Tests as well as examples in JsonQL.Demos.

## License

This project is licensed under the MIT License - see the LICENSE file in the solution root for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
```

## Here are some examples
***Below is an example of Json file with JsonQL expressions that the C# code below evaluates***
***NOTE: The JSON objects referenced in this JSON file are in parent JSON files Countries.json, Companies.json, and AdditionalTestData.json***
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

***Here is a C# code example that evaluates the Json in file above***
```csharp
 
var additionalTestData = new JsonTextData(
     "AdditionalTestData",
     this.LoadExampleJsonFile("AdditionalTestData.json"));

var countriesJsonTextData = new JsonTextData(
     "Countries",
     this.LoadExampleJsonFile("Countries.json"), additionalTestData);

var companiesJsonTextData = new JsonTextData("Companies",
     this.LoadExampleJsonFile("Companies.json"), countriesJsonTextData);

JsonQL.Compilation.IJsonCompiler jsonCompiler = null!; // Create an instance of JsonQL.Compilation.JsonCompiler here.
                                    //This is normally done once on application start.
var result = jsonCompiler.Compile(new JsonTextData("Overview",
     this.LoadExampleJsonFile("Overview.json"), companiesJsonTextData));
```

***Below is an example of querying a JSON data in one or more JSON files and converting the result to C# objects***

```csharp
var query = "Where(x => x.Id==100000006 || x.Id==100000007)";

IQueryManager queryManager = null!; // Create an instance of JsonQL.Query.QueryManager here.
                                    //This is normally done once on application start.

// We can convert to the following collection types:
// -One of the following interfaces: IReadOnlyList<T>, IEnumerable<T>, IList<T>, ICollection<T>, IReadOnlyCollection<T>
// -Any type that implements ICollection<T>. Example: List<T>,
// -Array T[],
// In these examples T is either an object (value or reference type), or another collection type (one of the listed here). 
var employeesResult =
    queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
        new JsonTextData("Data",
            this.LoadExampleJsonFile("Data.json")),
        [false, false], new JsonConversionSettingsOverrides
        {
            TryMapJsonConversionType = (type, parsedJson) =>
            {   
                // If we always return null, or just do not set the value of TryMapJsonConversionType
                // IEmployee will always be bound to Employee
                // In this example, we make sure that in some cases th default implementation of IManager 
                // is used (we can also specify Manager)
                if (parsedJson.HasKey(nameof(IManager.Employees)))
                    return typeof(IManager);
                return null;
            }
        });
```

***Below is an example of querying a JSON data in one or more JSON files and converting the result of collection of double values***

```csharp
var salariesOfAllEmployeesOlderThan35InAllCompaniesQuery = "Companies.Select(x => x.Employees.Where(x => x.Age > 35).Select(x => x.Salary))";

IQueryManager queryManager = null!; // Create an instance of JsonQL.Query.QueryManager here.
                                    //This is normally done once on application start.


var salariesResult =
    queryManager.QueryObject<IReadOnlyList<double>>(salariesOfAllEmployeesOlderThan35InAllCompaniesQuery,
        new JsonTextData("Data",
            this.LoadExampleJsonFile("Data.json")), null);
```
