// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Provides constant values for the names of various JSON functions used in the JsonQL compilation process.
/// </summary>
public static class JsonFunctionNames
{
    // Special functions
    /// <summary>
    /// Represents the JSON function name "index", which is used to retrieve the index of an item within a collection
    /// during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string ContextValueIndex = "index";

    // Custom functions
    /// <summary>
    /// Represents the JSON function name "Lower", which is utilized to convert a string to lowercase
    /// during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string StringToLowerCase = "Lower";

    /// <summary>
    /// Represents the JSON function name "Upper", which is used to convert a given string to its uppercase equivalent
    /// during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string StringToUpperCase = "Upper";

    /// <summary>
    /// Represents the JSON function name "Concatenate", which is used to combine multiple string values
    /// into a single string during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string Concatenate = "Concatenate";

    /// <summary>
    /// Represents the JSON function name "Len", which is used to determine the length of a text value
    /// during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string TextLength = "Len";

    // Implement 'If' in future release
    //public const string If = "If";

    #region Conversion functions

    /// <summary>
    /// Represents the JSON function name "ToDateTime", which is used to convert a value to a DateTime format
    /// during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string ConvertToDateTime = "ToDateTime";

    /// <summary>
    /// Represents the JSON function name "ToDate", which is used to convert a value into a date format
    /// during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string ConvertToDate = "ToDate";

    /// <summary>
    /// Represents the JSON function name "ToDouble", which is used to convert a value to a double
    /// during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string ConvertToDouble = "ToDouble";

    /// <summary>
    /// Represents the JSON function name "ToInt", which is used to convert a value to an integer during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string ConvertToInt = "ToInt";

    /// <summary>
    /// Represents the JSON function name "ToBoolean", which is used to convert a value into its boolean equivalent
    /// during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string ConvertToBoolean = "ToBoolean";

    /// <summary>
    /// Represents the JSON function name "ToString", which is used to convert a value
    /// into its string representation during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string ConvertToString = "ToString";
    #endregion

    #region Aggregate functions

    /// <summary>
    /// Represents the JSON function name "Any", which is used to determine if any elements in a collection
    /// satisfy a specified condition during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string AnyAggregateLambdaExpressionFunction = "Any";

    /// <summary>
    /// Represents the JSON function name "All", which is used to evaluate whether all elements
    /// in a collection satisfy a specified condition during JSON function execution
    /// in the JsonQL compilation process.
    /// </summary>
    public const string AllAggregateLambdaExpressionFunction = "All";

    /// <summary>
    /// Represents the JSON function name "Count", which is used to calculate the count of items in a collection
    /// during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string CountAggregateLambdaExpressionFunction = "Count";

    /// <summary>
    /// Represents the JSON function name "Average", which is utilized for performing an average
    /// aggregation operation within JSON function evaluation during the JsonQL compilation process.
    /// </summary>
    public const string AverageAggregateLambdaExpressionFunction = "Average";

    /// <summary>
    /// Represents the JSON function name "Min", which is used to compute the minimum value
    /// from a collection of elements during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string MinAggregateLambdaExpressionFunction = "Min";

    /// <summary>
    /// Represents the JSON function name "Max", which is utilized to compute the maximum value
    /// within a specified collection during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string MaxAggregateLambdaExpressionFunction = "Max";

    /// <summary>
    /// Represents the JSON function name "Sum", which is used to compute the sum of elements in a collection
    /// during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string SumAggregateLambdaExpressionFunction = "Sum";

    #endregion

    #region Math functions

    /// <summary>
    /// Represents the JSON function name "Abs", which is used to calculate the absolute value of a specified numeric value
    /// during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string Abs = "Abs";
    #endregion
    
    #region Boolean functions
    /// <summary>
    /// Represents the JSON function name "HasField", utilized to determine whether a specific field exists
    /// during the JSON function evaluation within the JsonQL compilation process.
    /// </summary>
    public const string HasField = "HasField";
    
    /// <summary>
    /// Represents the JSON function name "IsEven", used to determine whether a given numeric value is an even number
    /// during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string IsEven = "IsEven";

    /// <summary>
    /// Represents the JSON function name "IsOdd", which is used to determine if a numerical value
    /// is an odd number during JSON function evaluation in the JsonQL compilation process.
    /// </summary>
    public const string IsOdd = "IsOdd";
    #endregion

    #region Expressions converted to JSON Values
    /// <summary>
    /// Function name for expressions converted to <see cref="IParsedSimpleValue"/>
    /// </summary>
    public const string ExpressionConvertedToParsedSimpleValue = "ExpressionConvertedToParsedSimpleValue";
    #endregion
}
