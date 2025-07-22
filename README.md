## NOTE, following is a very high level description of **JsonQL**. For more details please refer to examples in JsonQL.Demo and JsonQL.Tests projects in [https://github.com/artakhak/JsonQL](https://github.com/artakhak/JsonQL).

```markdown

# JsonQL

JsonQL is a powerful JSON query language implementation that provides a flexible way to query and manipulate JSON data using a SQL-like syntax with rich function support.

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

## Built-in Functions

### Aggregate Functions
- `Count()` - Counts elements in a collection
- `Average()` - Calculates average of numeric values
- `Min()` - Finds minimum value
- `Max()` - Finds maximum value
- `Sum()` - Calculates sum of numeric values
- `All()` - Tests if all elements match a condition
- `Any()` - Tests if any elements match a condition

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
- `contains`		 - Contains operator
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
