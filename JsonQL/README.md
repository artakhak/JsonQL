```markdown
# JsonQL

JsonQL is a powerful JSON query language implementation that provides a flexible way to query and manipulate JSON data using a SQL-like syntax with rich function support.

## Features

- Rich set of built-in functions for JSON manipulation
- Support for aggregate functions (COUNT, AVG, MIN, MAX, SUM)
- Collection manipulation with ANY and ALL operations
- String operations (ToUpper, ToLower, Length, Concatenate)
- Type conversion functions (DateTime, Date, Double, Int, Boolean, String)
- Mathematical operations (Abs, IsEven, IsOdd)
- Object property inspection (HasField)
- Lambda expression support for complex queries
- Extensible function architecture

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

## License

This project is licensed under the MIT License - see the LICENSE file in the solution root for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
```


This README provides an overview of the project's main features and capabilities based on the code shown. If you'd like to add more sections such as installation instructions, usage examples, or contribution guidelines, please let me know!
