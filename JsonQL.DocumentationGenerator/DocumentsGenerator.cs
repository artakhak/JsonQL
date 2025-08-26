using FileInclude;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.DocumentationGenerator;

internal class DocumentsGenerator
{
    private readonly (string teplateFileRelativePath, string generatedFileRelativePath)[] _filesRelativePathsData = 
    {
        (@"JsonQL.Tests\doc-files\README.md.template", "README.md"),
        (@"JsonQL.Tests\doc-files\README.md.template", @"JsonQL\README.md"),
        (@"JsonQL.Tests\doc-files\index.rst.template", @"docs\index.rst"),
        (@"JsonQL.Tests\doc-files\json-file-mutation\index.rst.template", @"docs\json-file-mutation\index.rst")
        /*("README.md.template", "README.md"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\index.rst.template", @"docs\index.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\summary.rst.template", @"docs\summary.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\Literals\literals.rst.template", @"docs\literals.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\FunctionsAndBraces\functions-and-braces.rst.template", @"docs\functions-and-braces.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\Operators\operators.rst.template", @"docs\operators.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\NumericValues\numeric-values.rst.template", @"docs\numeric-values.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\Texts\texts.rst.template", @"docs\texts.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\CodeSeparatorsAndCodeBlocks\code-separators-and-code-blocks.rst.template", @"docs\code-separators-and-code-blocks.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\Keywords\keywords.rst.template", @"docs\keywords.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\Prefixes\prefixes.rst.template", @"docs\prefixes.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\Postfixes\postfixes.rst.template", @"docs\postfixes.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\CustomExpressionItemParsers\custom-expression-item-parsers.rst.template", @"docs\custom-expression-item-parsers.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\Comments\comments.rst.template", @"docs\comments.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\ErrorReporting\error-reporting.rst.template", @"docs\error-reporting.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\ParsingSectionInText\parsing-section-in-text.rst.template", @"docs\parsing-section-in-text.rst"),
        (@"UniversalExpressionParser.Tests\Demos\DemoExpressions\CaseSensitivityAndNonStandardLanguageFeatures\case-sensitivity-and-non-standard-language-features.rst.template", @"docs\case-sensitivity-and-non-standard-language-features.rst")*/
    };

    private readonly ITemplateProcessor _templateProcessor = new TemplateProcessor();
    private readonly DocumentGenerator _documentGenerator;

    public DocumentsGenerator()
    {
        var assemblyFilePath = typeof(Program).Assembly.Location;

        if (assemblyFilePath == null)
            throw new Exception("Failed to get assembly location.");

        var indexOfDocumentationGenerator = assemblyFilePath.IndexOf(@"\JsonQL.DocumentationGenerator\", StringComparison.Ordinal);

        var solutionFolderPath = assemblyFilePath.Substring(0, indexOfDocumentationGenerator);

        _documentGenerator = new DocumentGenerator(_templateProcessor, solutionFolderPath);
    }

    public bool GenerateDocumentsFromTemplates()
    {
        foreach (var filesRelativePathData in _filesRelativePathsData)
        {
            if (!_documentGenerator.GenerateFileFromTemplate(filesRelativePathData.teplateFileRelativePath, filesRelativePathData.generatedFileRelativePath))
            {
                LogHelper.Context.Log.Error("Template generation failed.");
                return false;
            }
        }

        return true;
    }
}