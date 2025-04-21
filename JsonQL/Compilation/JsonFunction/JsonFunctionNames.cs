namespace JsonQL.Compilation.JsonFunction;

public static class JsonFunctionNames
{
    // Special functions
    public const string ContextValueIndex = "index";

    // Custom functions
    public const string StringToLowerCase = "Lower";
    public const string Concatenate = "Concatenate";
    public const string StringToUpperCase = "Upper";
    public const string TextLength = "Len";

    public const string HasField = "HasField";

    // Implement 'If' in future release
    //public const string If = "If";

    #region Conversionj functions
    public const string ConvertToDateTime = "ToDateTime";
    public const string ConvertToDate = "ToDate";
    public const string ConvertToDouble = "ToDouble";
    public const string ConvertToInt = "ToInt";
    public const string ConvertToBoolean = "ToBoolean";
    public const string ConvertToString = "ToString";
    #endregion


    #region Aggregate functions
    public const string AnyAggregateLambdaExpressionFunction = "Any";
    public const string AllAggregateLambdaExpressionFunction = "All";
    public const string CountAggregateLambdaExpressionFunction = "Count";
    public const string AverageAggregateLambdaExpressionFunction = "Average";
    public const string MinAggregateLambdaExpressionFunction = "Min";
    public const string MaxAggregateLambdaExpressionFunction = "Max";
    public const string SumAggregateLambdaExpressionFunction = "Sum";

    #endregion

    #region Math functions
    public const string Abs = "Abs";
    public const string IsEven = "IsEven";
    public const string IsOdd = "IsOdd";
    #endregion
}