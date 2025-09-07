using FileInclude;
using OROptimizer.Diagnostics.Log;
using System.IO;

namespace JsonQL.DocumentationGenerator;

internal class DocumentsGenerator
{
    private static readonly string DocumentationGeneratorPathRelativeToSolutionFolder = $"{Path.DirectorySeparatorChar}JsonQL.DocumentationGenerator{Path.DirectorySeparatorChar}";
    private static readonly string SrcDocFilesPathRelativeToSolutionFolder = $"{Path.DirectorySeparatorChar}JsonQL.Demos{Path.DirectorySeparatorChar}DocFiles{Path.DirectorySeparatorChar}";

    /// <summary>
    /// Path of generated documentation files relative to project folder.
    /// </summary>
    private static readonly string DocsRootRelativePath = $"{Path.DirectorySeparatorChar}docs{Path.DirectorySeparatorChar}";
    private const string TemplateExtension = ".template";

    /*private readonly (string teplateFileRelativePath, string generatedFileRelativePath)[] _filesRelativePathsData =
    {
        (@"JsonQL.Demos\DocFiles\README.md.template", "README.md"),
        (@"JsonQL.Demos\DocFiles\README.md.template", @"JsonQL\README.md"),

        #region ImportantInterfaces files
        (@"JsonQL.Demos\DocFiles\ImportantInterfaces\ICompilationResult\index.rst.template", @"docs\ImportantInterfaces\ICompilationResult\index.rst"),
        (@"JsonQL.Demos\DocFiles\ImportantInterfaces\ICompiledJsonData\index.rst.template", @"docs\ImportantInterfaces\ICompiledJsonData\index.rst"),
        (@"JsonQL.Demos\DocFiles\ImportantInterfaces\IParsedValue\index.rst.template", @"docs\ImportantInterfaces\IParsedValue\index.rst"),
        (@"JsonQL.Demos\DocFiles\ImportantInterfaces\IRootParsedValue\index.rst.template", @"docs\ImportantInterfaces\IRootParsedValue\index.rst"),
        #endregion
        
        // index.rst file related
        (@"JsonQL.Demos\DocFiles\index.rst.template", @"docs\index.rst"),

        (@"JsonQL.Demos\DocFiles\Examples\json-with-json-ql-expressions.data-1.rst.template", @"docs\Examples\json-with-json-ql-expressions.data-1.rst"),
        (@"JsonQL.Demos\DocFiles\Examples\json-with-json-ql-expressions.data-2.rst.template", @"docs\Examples\json-with-json-ql-expressions.data-2.rst"),
        (@"JsonQL.Demos\DocFiles\Examples\json-with-json-ql-expressions.data-3.rst.template", @"docs\Examples\json-with-json-ql-expressions.data-3.rst"),
        (@"JsonQL.Demos\DocFiles\Examples\json-with-json-ql-expressions.data-4.rst.template", @"docs\Examples\json-with-json-ql-expressions.data-4.rst"),
        (@"JsonQL.Demos\DocFiles\Examples\json-with-json-ql-expressions.result.rst.template", @"docs\Examples\json-with-json-ql-expressions.result.rst"),

        (@"JsonQL.Demos\DocFiles\Examples\query-and-convert-json-to-csharp-objects.data-1.rst.template", @"docs\Examples\query-and-convert-json-to-csharp-objects.data-1.rst"),
        (@"JsonQL.Demos\DocFiles\Examples\query-and-convert-json-to-csharp-objects.result.rst.template", @"docs\Examples\query-and-convert-json-to-csharp-objects.result.rst"),

        (@"JsonQL.Demos\DocFiles\Examples\query-and-convert-json-to-collection-of-doubles.data-1.rst.template", @"docs\Examples\query-and-convert-json-to-collection-of-doubles.data-1.rst"),
        (@"JsonQL.Demos\DocFiles\Examples\query-and-convert-json-to-collection-of-doubles.result.rst.template", @"docs\Examples\query-and-convert-json-to-collection-of-doubles.result.rst"),

        (@"JsonQL.Demos\DocFiles\Examples\query-with-result-as-json-object-1.data.rst.template", @"docs\Examples\query-with-result-as-json-object-1.data.rst"),
        (@"JsonQL.Demos\DocFiles\Examples\query-with-result-as-json-object.result.rst.template", @"docs\Examples\query-with-result-as-json-object.result.rst"),
        
        #region MutatingJsonFiles files

        // MutatingJsonFiles\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\index.rst.template", @"docs\MutatingJsonFiles\index.rst"),

        // MutatingJsonFiles\ParsedResultDataStructure\ files
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\ParsedResultDataStructure\index.rst.template", @"docs\MutatingJsonFiles\ParsedResultDataStructure\index.rst"),

        // MutatingJsonFiles\Examples\ files
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\Examples\Companies.json", @"docs\MutatingJsonFiles\Examples\Companies.json"),
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\Examples\Countries.json", @"docs\MutatingJsonFiles\Examples\Countries.json"),

        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\Examples\Example1\Example.json", @"docs\MutatingJsonFiles\Examples\Example1\Example.json"),
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\Examples\Example1\FilteredCompanies.json", @"docs\MutatingJsonFiles\Examples\Example1\FilteredCompanies.json"),
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\Examples\Example1\Parameters.json", @"docs\MutatingJsonFiles\Examples\Example1\Parameters.json"),
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\Examples\Example1\Result.json", @"docs\MutatingJsonFiles\Examples\Example1\Result.json"),
        
        // MutatingJsonFiles\SampleFiles\ files
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\SampleFiles\index.rst.template", @"docs\MutatingJsonFiles\SampleFiles\index.rst"),
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\SampleFiles\companies.rst.template", @"docs\MutatingJsonFiles\SampleFiles\companies.rst"),
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\SampleFiles\countries.rst.template", @"docs\MutatingJsonFiles\SampleFiles\countries.rst"),

        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\SampleFiles\Example1\example.rst.template", @"docs\MutatingJsonFiles\SampleFiles\Example1\example.rst"),
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\SampleFiles\Example1\parameters.rst.template", @"docs\MutatingJsonFiles\SampleFiles\Example1\parameters.rst"),
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\SampleFiles\Example1\filtered-companies.rst.template", @"docs\MutatingJsonFiles\SampleFiles\Example1\filtered-companies.rst"),
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\SampleFiles\Example1\result.rst.template", @"docs\MutatingJsonFiles\SampleFiles\Example1\result.rst"),

        // MutatingJsonFiles\ErrorDetails\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\ErrorDetails\index.rst.template", @"docs\MutatingJsonFiles\ErrorDetails\index.rst"),

        // MutatingJsonFiles\ReusingCompiledJsonFiles\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\MutatingJsonFiles\ReusingCompiledJsonFiles\index.rst.template", @"docs\MutatingJsonFiles\ReusingCompiledJsonFiles\index.rst"),

        #endregion
        
        // QueryingJsonFiles\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\QueryingJsonFiles\index.rst.template", @"docs\QueryingJsonFiles\index.rst"),

        // QueryingJsonFiles\ResultAsCSharpObject\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\QueryingJsonFiles\ResultAsCSharpObject\index.rst.template", @"docs\QueryingJsonFiles\ResultAsCSharpObject\index.rst"),

        // QueryingJsonFiles\ResultAsCSharpObject\ErrorDetails\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\QueryingJsonFiles\ResultAsCSharpObject\ErrorDetails\index.rst.template", @"docs\QueryingJsonFiles\ResultAsCSharpObject\ErrorDetails\index.rst"),

        // QueryingJsonFiles\ResultAsJsonStructure\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\QueryingJsonFiles\ResultAsJsonStructure\index.rst.template", @"docs\QueryingJsonFiles\ResultAsJsonStructure\index.rst"),

        // QueryingJsonFiles\ResultAsJsonStructure\ErrorDetails\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\QueryingJsonFiles\ResultAsJsonStructure\ErrorDetails\index.rst.template", @"docs\QueryingJsonFiles\ResultAsJsonStructure\ErrorDetails\index.rst"),

        // QueryingJsonFiles\ReusingCompiledJsonFiles\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\QueryingJsonFiles\ReusingCompiledJsonFiles\index.rst.template", @"docs\QueryingJsonFiles\ReusingCompiledJsonFiles\index.rst"),

        // QueryingJsonFiles\LambdaFunctions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\LambdaFunctions\index.rst.template", @"docs\LambdaFunctions\index.rst"),

        // QueryingJsonFiles\SpecialKeywords\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\SpecialKeywords\index.rst.template", @"docs\SpecialKeywords\index.rst"),
       
        // QueryingJsonFiles\SpecialKeywords\Index\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\SpecialKeywords\Index\index.rst.template", @"docs\SpecialKeywords\Index\index.rst"),

        // QueryingJsonFiles\SpecialKeywords\Parent\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\SpecialKeywords\Parent\index.rst.template", @"docs\SpecialKeywords\Parent\index.rst"),

        // QueryingJsonFiles\SpecialKeywords\This\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\SpecialKeywords\This\index.rst.template", @"docs\SpecialKeywords\This\index.rst"),

        // JsonMutatorOperators\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonMutatorOperators\index.rst.template", @"docs\JsonMutatorOperators\index.rst"),

        // JsonMutatorOperators\StringInterpolation\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonMutatorOperators\StringInterpolation\index.rst.template", @"docs\JsonMutatorOperators\StringInterpolation\index.rst"),

        // JsonMutatorOperators\Value\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonMutatorOperators\Value\index.rst.template", @"docs\JsonMutatorOperators\Value\index.rst"),

        // JsonMutatorOperators\CopyFields\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonMutatorOperators\CopyFields\index.rst.template", @"docs\JsonMutatorOperators\CopyFields\index.rst"),

        // JsonMutatorOperators\MergeArray\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonMutatorOperators\MergeArray\index.rst.template", @"docs\JsonMutatorOperators\MergeArray\index.rst"),

        // JsonPathFunctions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonPathFunctions\index.rst.template", @"docs\JsonPathFunctions\index.rst"),

        // JsonPathFunctions\ArrayIndexers\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonPathFunctions\ArrayIndexers\index.rst.template", @"docs\JsonPathFunctions\ArrayIndexers\index.rst"),

        // JsonPathFunctions\At\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonPathFunctions\At\index.rst.template", @"docs\JsonPathFunctions\At\index.rst"),

        // JsonPathFunctions\First\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonPathFunctions\First\index.rst.template", @"docs\JsonPathFunctions\First\index.rst"),

        // JsonPathFunctions\Last\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonPathFunctions\Last\index.rst.template", @"docs\JsonPathFunctions\Last\index.rst"),

        // JsonPathFunctions\Flatten\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonPathFunctions\Flatten\index.rst.template", @"docs\JsonPathFunctions\Flatten\index.rst"),

        // JsonPathFunctions\Reverse\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonPathFunctions\Reverse\index.rst.template", @"docs\JsonPathFunctions\Reverse\index.rst"),

        // JsonPathFunctions\Select\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonPathFunctions\Select\index.rst.template", @"docs\JsonPathFunctions\Select\index.rst"),

        // JsonPathFunctions\Where\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\JsonPathFunctions\Where\index.rst.template", @"docs\JsonPathFunctions\Where\index.rst"),

        // functions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\index.rst.template", @"docs\Functions\index.rst"),

        // functions\ConversionFunctions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\ConversionFunctions\index.rst.template", @"docs\Functions\ConversionFunctions\index.rst"),

        // functions\ConversionFunctions\ToBoolean\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\ConversionFunctions\ToBoolean\index.rst.template", @"docs\Functions\ConversionFunctions\ToBoolean\index.rst"),

        // functions\ConversionFunctions\ToDate\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\ConversionFunctions\ToDate\index.rst.template", @"docs\Functions\ConversionFunctions\ToDate\index.rst"),

        // functions\ConversionFunctions\ToDateTime\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\ConversionFunctions\ToDateTime\index.rst.template", @"docs\Functions\ConversionFunctions\ToDateTime\index.rst"),

        // functions\ConversionFunctions\ToDouble\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\ConversionFunctions\ToDouble\index.rst.template", @"docs\Functions\ConversionFunctions\ToDouble\index.rst"),

        // functions\ConversionFunctions\ToInt\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\ConversionFunctions\ToInt\index.rst.template", @"docs\Functions\ConversionFunctions\ToInt\index.rst"),

        // functions\ConversionFunctions\ToString\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\ConversionFunctions\ToString\index.rst.template", @"docs\Functions\ConversionFunctions\ToString\index.rst"),

        // functions\NumericFunctions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\NumericFunctions\index.rst.template", @"docs\Functions\NumericFunctions\index.rst"),

        // functions\NumericFunctions\Abs\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\NumericFunctions\Abs\index.rst.template", @"docs\Functions\NumericFunctions\Abs\index.rst"),

        // functions\BooleanFunctions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\BooleanFunctions\index.rst.template", @"docs\Functions\BooleanFunctions\index.rst"),

        // functions\BooleanFunctions\HasField\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\BooleanFunctions\HasField\index.rst.template", @"docs\Functions\BooleanFunctions\HasField\index.rst"),

        // functions\BooleanFunctions\IsEven\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\BooleanFunctions\IsEven\index.rst.template", @"docs\Functions\BooleanFunctions\IsEven\index.rst"),

        // functions\BooleanFunctions\IsOdd\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\BooleanFunctions\IsOdd\index.rst.template", @"docs\Functions\BooleanFunctions\IsOdd\index.rst"),

        // functions\StringFunctions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\StringFunctions\index.rst.template", @"docs\Functions\StringFunctions\index.rst"),

        // functions\StringFunctions\Concatenate\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\StringFunctions\Concatenate\index.rst.template", @"docs\Functions\StringFunctions\Concatenate\index.rst"),

        // functions\StringFunctions\Len\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\StringFunctions\Len\index.rst.template", @"docs\Functions\StringFunctions\Len\index.rst"),

        // functions\StringFunctions\Lower\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\StringFunctions\Lower\index.rst.template", @"docs\Functions\StringFunctions\Lower\index.rst"),

        // functions\StringFunctions\Upper\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Functions\StringFunctions\Upper\index.rst.template", @"docs\Functions\StringFunctions\Upper\index.rst"),

        // AggregateFunctions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\AggregateFunctions\index.rst.template", @"docs\AggregateFunctions\index.rst"),

        // AggregateFunctions\All\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\AggregateFunctions\All\index.rst.template", @"docs\AggregateFunctions\All\index.rst"),

        // AggregateFunctions\Any\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\AggregateFunctions\Any\index.rst.template", @"docs\AggregateFunctions\Any\index.rst"),

        // AggregateFunctions\Average\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\AggregateFunctions\Average\index.rst.template", @"docs\AggregateFunctions\Average\index.rst"),

        // AggregateFunctions\Count\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\AggregateFunctions\Count\index.rst.template", @"docs\AggregateFunctions\Count\index.rst"),

        // AggregateFunctions\Max\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\AggregateFunctions\Max\index.rst.template", @"docs\AggregateFunctions\Max\index.rst"),

        // AggregateFunctions\Min\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\AggregateFunctions\Min\index.rst.template", @"docs\AggregateFunctions\Min\index.rst"),

        // AggregateFunctions\Sum\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\AggregateFunctions\Sum\index.rst.template", @"docs\AggregateFunctions\Sum\index.rst"),

        #region operators

        // operators\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\index.rst.template", @"docs\Operators\index.rst"),
        
        // operators\DefaultValue\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\DefaultValue\index.rst.template", @"docs\Operators\DefaultValue\index.rst"),

        // operators\JsonPathSeparator\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\JsonPathSeparator\index.rst.template", @"docs\Operators\JsonPathSeparator\index.rst"),

        // operators\Lambda\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\Lambda\index.rst.template", @"docs\Operators\Lambda\index.rst"),

        // operators\NamedParameter\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\NamedParameter\index.rst.template", @"docs\Operators\NamedParameter\index.rst"),

        // operators\Assert\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\Assert\index.rst.template", @"docs\Operators\Assert\index.rst"),
        
        // operators\TypeOf\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\TypeOf\index.rst.template", @"docs\Operators\TypeOf\index.rst"),

        #region arithmetic-operators
        // operators\ArithmeticOperators\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ArithmeticOperators\index.rst.template", @"docs\Operators\ArithmeticOperators\index.rst"),

        // operators\ArithmeticOperators\Add\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ArithmeticOperators\Add\index.rst.template", @"docs\Operators\ArithmeticOperators\Add\index.rst"),
        
        // operators\ArithmeticOperators\Divide\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ArithmeticOperators\Divide\index.rst.template", @"docs\Operators\ArithmeticOperators\Divide\index.rst"),
        
        // operators\ArithmeticOperators\Multiply\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ArithmeticOperators\Multiply\index.rst.template", @"docs\Operators\ArithmeticOperators\Multiply\index.rst"),
        
        // operators\ArithmeticOperators\NegativeSign\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ArithmeticOperators\NegativeSign\index.rst.template", @"docs\Operators\ArithmeticOperators\NegativeSign\index.rst"),
        
        // operators\ArithmeticOperators\Quotient\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ArithmeticOperators\Quotient\index.rst.template", @"docs\Operators\ArithmeticOperators\Quotient\index.rst"),
        
        // operators\ArithmeticOperators\Subtract\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ArithmeticOperators\Subtract\index.rst.template", @"docs\Operators\ArithmeticOperators\Subtract\index.rst"),
        #endregion
        
        #region ComparisonOperators

        // operators\ComparisonOperators\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ComparisonOperators\index.rst.template", @"docs\Operators\ComparisonOperators\index.rst"),

        // operators\ComparisonOperators\Equals\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ComparisonOperators\Equals\index.rst.template", @"docs\Operators\ComparisonOperators\Equals\index.rst"),

        // operators\ComparisonOperators\NotEquals\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ComparisonOperators\NotEquals\index.rst.template", @"docs\Operators\ComparisonOperators\NotEquals\index.rst"),

        // operators\ComparisonOperators\LessThan\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ComparisonOperators\LessThan\index.rst.template", @"docs\Operators\ComparisonOperators\LessThan\index.rst"),

        // operators\ComparisonOperators\LessThanOrEquals\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ComparisonOperators\LessThanOrEquals\index.rst.template", @"docs\Operators\ComparisonOperators\LessThanOrEquals\index.rst"),

        // operators\ComparisonOperators\GreaterThan\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ComparisonOperators\GreaterThan\index.rst.template", @"docs\Operators\ComparisonOperators\GreaterThan\index.rst"),

        // operators\ComparisonOperators\GreaterThanOrEquals\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ComparisonOperators\GreaterThanOrEquals\index.rst.template", @"docs\Operators\ComparisonOperators\GreaterThanOrEquals\index.rst"),

        #endregion
        
        #region LogicalOperators
        // operators\LogicalOperators\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\LogicalOperators\index.rst.template", @"docs\Operators\LogicalOperators\index.rst"),

        // operators\LogicalOperators\And\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\LogicalOperators\And\index.rst.template", @"docs\Operators\LogicalOperators\And\index.rst"),
        
        // operators\LogicalOperators\Or\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\LogicalOperators\Or\index.rst.template", @"docs\Operators\LogicalOperators\Or\index.rst"),

        
        // operators\LogicalOperators\Negate\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\LogicalOperators\Negate\index.rst.template", @"docs\Operators\LogicalOperators\Negate\index.rst"),

        #endregion

        #region TextMatchingOperators

        // operators\TextMatchingOperators\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\TextMatchingOperators\index.rst.template", @"docs\Operators\TextMatchingOperators\index.rst"),

        // operators\TextMatchingOperators\Contains\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\TextMatchingOperators\Contains\index.rst.template", @"docs\Operators\TextMatchingOperators\Contains\index.rst"),

        // operators\TextMatchingOperators\StartsWith\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\TextMatchingOperators\StartsWith\index.rst.template", @"docs\Operators\TextMatchingOperators\StartsWith\index.rst"),

        // operators\EndsWith\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\TextMatchingOperators\EndsWith\index.rst.template", @"docs\Operators\TextMatchingOperators\EndsWith\index.rst"),

        #endregion
        
        #region ValueIsNullOrUndefinedCheckOperators

        // operators\ValueIsNullOrUndefinedCheckOperators\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ValueIsNullOrUndefinedCheckOperators\index.rst.template", @"docs\Operators\ValueIsNullOrUndefinedCheckOperators\index.rst"),

        // operators\ValueIsNullOrUndefinedCheckOperators\IsNotNull\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ValueIsNullOrUndefinedCheckOperators\IsNotNull\index.rst.template", @"docs\Operators\ValueIsNullOrUndefinedCheckOperators\IsNotNull\index.rst"),

        // operators\ValueIsNullOrUndefinedCheckOperators\IsNotUndefined\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ValueIsNullOrUndefinedCheckOperators\IsNotUndefined\index.rst.template", @"docs\Operators\ValueIsNullOrUndefinedCheckOperators\IsNotUndefined\index.rst"),
        
        // operators\ValueIsNullOrUndefinedCheckOperators\IsNull\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ValueIsNullOrUndefinedCheckOperators\IsNull\index.rst.template", @"docs\Operators\ValueIsNullOrUndefinedCheckOperators\IsNull\index.rst"),

        // operators\ValueIsNullOrUndefinedCheckOperators\IsUndefined\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\Operators\ValueIsNullOrUndefinedCheckOperators\IsUndefined\index.rst.template", @"docs\Operators\ValueIsNullOrUndefinedCheckOperators\IsUndefined\index.rst"),

        #endregion
        #endregion
       
        // OptionalAndNamedParameters\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\OptionalAndNamedParameters\index.rst.template", @"docs\OptionalAndNamedParameters\index.rst"),

        // DependencyInjectionSetup\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\DependencyInjectionSetup\index.rst.template", @"docs\DependencyInjectionSetup\index.rst"),

        // CustomJsonQL\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\CustomJsonQL\index.rst.template", @"docs\CustomJsonQL\index.rst"),

        #region future-releases
        // future-releases\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\FutureReleases\index.rst.template", @"docs\FutureReleases\index.rst"),

        // future-releases\ComplexProjections\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\FutureReleases\ComplexProjections\index.rst.template", @"docs\FutureReleases\ComplexProjections\index.rst"),

        // future-releases\Grouping\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\FutureReleases\Grouping\index.rst.template", @"docs\FutureReleases\Grouping\index.rst"),

        // future-releases\MultilineQueries\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\FutureReleases\MultilineQueries\index.rst.template", @"docs\FutureReleases\MultilineQueries\index.rst"),

        // future-releases\Sorting\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\FutureReleases\Sorting\index.rst.template", @"docs\FutureReleases\Sorting\index.rst"),
        
        // future-releases\IfFunction\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\FutureReleases\IfFunction\index.rst.template", @"docs\FutureReleases\IfFunction\index.rst"),

        // future-releases\RoundUpFunction\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\FutureReleases\RoundUpFunction\index.rst.template", @"docs\FutureReleases\RoundUpFunction\index.rst"),
        
        // future-releases\RoundDownFunction\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\FutureReleases\RoundDownFunction\index.rst.template", @"docs\FutureReleases\RoundDownFunction\index.rst"),
        
        // future-releases\JoinFunction\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\FutureReleases\JoinFunction\index.rst.template", @"docs\FutureReleases\JoinFunction\index.rst"),
        
        // future-releases\RegexTextMatching\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\FutureReleases\RegexTextMatching\index.rst.template", @"docs\FutureReleases\RegexTextMatching\index.rst"),
        
        // future-releases\NumberFormatting\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\FutureReleases\NumberFormatting\index.rst.template", @"docs\FutureReleases\NumberFormatting\index.rst"),
        
        // future-releases\DateTimeFormatting\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\FutureReleases\DateTimeFormatting\index.rst.template", @"docs\FutureReleases\DateTimeFormatting\index.rst")
        #endregion
    };
    */
    private readonly ITemplateProcessor _templateProcessor = new TemplateProcessor();
    private readonly DocumentGenerator _documentGenerator;
    private readonly string _solutionFolderPath;

    public DocumentsGenerator()
    {
        var assemblyFilePath = typeof(Program).Assembly.Location;

        if (assemblyFilePath == null)
            throw new Exception("Failed to get assembly location.");

        var indexOfDocumentationGenerator = assemblyFilePath.IndexOf(DocumentationGeneratorPathRelativeToSolutionFolder, StringComparison.Ordinal);

        _solutionFolderPath = assemblyFilePath.Substring(0, indexOfDocumentationGenerator);
        _documentGenerator = new DocumentGenerator(_templateProcessor, _solutionFolderPath);
    }

    void IterateFolderFiles(string folderToIterate, Action<string> fileProcessor)
    {
        // Add all filePathsToCopy in the current solutionFolderPath
        var currentFiles = System.IO.Directory.GetFiles(folderToIterate);

        foreach (var currentFile in currentFiles)
            fileProcessor(currentFile);

        // Recursively process subdirectories
        foreach (var subDirectory in Directory.GetDirectories(folderToIterate))
        {
            IterateFolderFiles(subDirectory, fileProcessor);
        }
    }

    bool IsTemplateFile(string filePath)
    {
        var fileExtension = Path.GetExtension(filePath);
        return String.Equals(fileExtension, TemplateExtension);
    }


    void CopyDocFileToDocsFolder(string srcRootFolderRelativePath, string srcFilePath)
    {
        // Example of how the values of srcFileRelativePath and generatedFileRelativePath are calculated in this method below:
        // Consider example values of DocsRootRelativePath, srcRootFolderRelativePath, and srcFilePath shown here.
        // DocsRootRelativePath="\docs" (this is the value of constant DocsRootRelativePath and is the root destination folder relative to solution folder)
        // srcRootFolderRelativePath="\JsonQL.Demos\"
        // srcFilePath = "c:\JsonQL\JsonQL.Demos\DocFiles\Examples\json-with-json-ql-expressions.data-1.rst.template"
        // We want to calculate srcFileRelativePath and generatedFileRelativePath from DocsRootRelativePath, srcRootFolderRelativePath and srcFilePath 
        // to have these values:
        // srcFileRelativePath = "JsonQL.Demos\DocFiles\Examples\json-with-json-ql-expressions.data-1.rst.template"
        // generatedFileRelativePath = "docs\DocFiles\Examples\json-with-json-ql-expressions.data-1.rst.template"

        string? srcFileRelativePath = null;
        string? generatedFileRelativePath = null;

        try
        {
            var indexOfSrcFolder = srcFilePath.IndexOf(srcRootFolderRelativePath, StringComparison.Ordinal);

            if (indexOfSrcFolder < 0)
                throw new ApplicationException($"The value of '{srcRootFolderRelativePath}' is not in '{srcFilePath}'");

            srcFileRelativePath = srcFilePath.Substring(indexOfSrcFolder);

            if (srcFileRelativePath.StartsWith(Path.DirectorySeparatorChar))
                srcFileRelativePath = srcFileRelativePath.Substring(1);

            generatedFileRelativePath = Path.Join(DocsRootRelativePath, srcFilePath.Substring(indexOfSrcFolder + srcRootFolderRelativePath.Length));

            if (generatedFileRelativePath.EndsWith(TemplateExtension))
                generatedFileRelativePath = generatedFileRelativePath.Substring(0, generatedFileRelativePath.Length - TemplateExtension.Length);

            if (!_documentGenerator.GenerateFileFromTemplate(srcFileRelativePath, generatedFileRelativePath))
            {
                throw new ApplicationException("Template generation failed.");
            }
        }
        catch
        {
            LogHelper.Context.Log.ErrorFormat("Template generation failed. Src file path: '{0}', Src file relative path: '{1}', Generated relative path: '{2}'.",
                srcFilePath, srcFileRelativePath, generatedFileRelativePath);
            throw;
        }
    }

    public bool GenerateDocumentsFromTemplates()
    {
        try
        {
            
            // Copy non-template files first
            IterateFolderFiles(
                Path.Join(_solutionFolderPath, SrcDocFilesPathRelativeToSolutionFolder), srcFilePath =>
            {
                if (IsTemplateFile(srcFilePath))
                    return;

                CopyDocFileToDocsFolder(SrcDocFilesPathRelativeToSolutionFolder, srcFilePath);
            });

            // Copy template files next
            IterateFolderFiles(Path.Join(_solutionFolderPath, SrcDocFilesPathRelativeToSolutionFolder), srcFilePath =>
            {
                if (!IsTemplateFile(srcFilePath))
                    return;

                CopyDocFileToDocsFolder(SrcDocFilesPathRelativeToSolutionFolder, srcFilePath);
            });

            //foreach (var filesRelativePathData in _filesRelativePathsData)
            //{
            //    if (!_documentGenerator.GenerateFileFromTemplate(filesRelativePathData.teplateFileRelativePath, filesRelativePathData.generatedFileRelativePath))
            //    {
            //        LogHelper.Context.Log.Error("Template generation failed.");
            //        return false;
            //    }
            //}

            return true;
        }
        catch (Exception e)
        {
            LogHelper.Context.Log.Error("Failed to generate documents", e);
            return false;
        }
    }
}