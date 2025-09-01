using FileInclude;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.DocumentationGenerator;

internal class DocumentsGenerator
{
    private readonly (string teplateFileRelativePath, string generatedFileRelativePath)[] _filesRelativePathsData = 
    {
        (@"JsonQL.Demos\DocFiles\README.md.template", "README.md"),
        (@"JsonQL.Demos\DocFiles\README.md.template", @"JsonQL\README.md"),
        
        // index.rst file related
        (@"JsonQL.Demos\DocFiles\index.rst.template", @"docs\index.rst"),
        
        (@"JsonQL.Demos\DocFiles\index-rst-files\json-with-json-ql-expressions.data-1.rst.template", @"docs\index-rst-files\json-with-json-ql-expressions.data-1.rst"),
        (@"JsonQL.Demos\DocFiles\index-rst-files\json-with-json-ql-expressions.data-2.rst.template", @"docs\index-rst-files\json-with-json-ql-expressions.data-2.rst"),
        (@"JsonQL.Demos\DocFiles\index-rst-files\json-with-json-ql-expressions.data-3.rst.template", @"docs\index-rst-files\json-with-json-ql-expressions.data-3.rst"),
        (@"JsonQL.Demos\DocFiles\index-rst-files\json-with-json-ql-expressions.data-4.rst.template", @"docs\index-rst-files\json-with-json-ql-expressions.data-4.rst"),
        (@"JsonQL.Demos\DocFiles\index-rst-files\json-with-json-ql-expressions.result.rst.template", @"docs\index-rst-files\json-with-json-ql-expressions.result.rst"),
        
        (@"JsonQL.Demos\DocFiles\index-rst-files\query-and-convert-json-to-csharp-objects.data-1.rst.template", @"docs\index-rst-files\query-and-convert-json-to-csharp-objects.data-1.rst"),
        (@"JsonQL.Demos\DocFiles\index-rst-files\query-and-convert-json-to-csharp-objects.result.rst.template", @"docs\index-rst-files\query-and-convert-json-to-csharp-objects.result.rst"),
        
        (@"JsonQL.Demos\DocFiles\index-rst-files\query-and-convert-json-to-collection-of-doubles.data-1.rst.template", @"docs\index-rst-files\query-and-convert-json-to-collection-of-doubles.data-1.rst"),
        (@"JsonQL.Demos\DocFiles\index-rst-files\query-and-convert-json-to-collection-of-doubles.result.rst.template", @"docs\index-rst-files\query-and-convert-json-to-collection-of-doubles.result.rst"),
        
        (@"JsonQL.Demos\DocFiles\index-rst-files\query-with-result-as-json-object-1.data.rst.template", @"docs\index-rst-files\query-with-result-as-json-object-1.data.rst"),
        (@"JsonQL.Demos\DocFiles\index-rst-files\query-with-result-as-json-object.result.rst.template", @"docs\index-rst-files\query-with-result-as-json-object.result.rst"),
        
        // mutating-json-files\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\mutating-json-files\index.rst.template", @"docs\mutating-json-files\index.rst"),
        
        // mutating-json-files\error-details\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\mutating-json-files\error-details\index.rst.template", @"docs\mutating-json-files\error-details\index.rst"),

        // mutating-json-files\reusing-compiled-json-files\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\mutating-json-files\reusing-compiled-json-files\index.rst.template", @"docs\mutating-json-files\reusing-compiled-json-files\index.rst"),
        
        // querying-json-files\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\querying-json-files\index.rst.template", @"docs\querying-json-files\index.rst"),
        
        // querying-json-files\result-as-csharp-object\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\querying-json-files\result-as-csharp-object\index.rst.template", @"docs\querying-json-files\result-as-csharp-object\index.rst"),
        
        // querying-json-files\result-as-csharp-object\error-details\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\querying-json-files\result-as-csharp-object\error-details\index.rst.template", @"docs\querying-json-files\result-as-csharp-object\error-details\index.rst"),
        
        // querying-json-files\result-as-json-structure\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\querying-json-files\result-as-json-structure\index.rst.template", @"docs\querying-json-files\result-as-json-structure\index.rst"),
        
        // querying-json-files\result-as-json-structure\error-details\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\querying-json-files\result-as-json-structure\error-details\index.rst.template", @"docs\querying-json-files\result-as-json-structure\error-details\index.rst"),
        
        // querying-json-files\reusing-compiled-json-files\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\querying-json-files\reusing-compiled-json-files\index.rst.template", @"docs\querying-json-files\reusing-compiled-json-files\index.rst"),
        
        // querying-json-files\lambda-functions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\lambda-functions\index.rst.template", @"docs\lambda-functions\index.rst"),
        
        // querying-json-files\special-keywords\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\special-keywords\index.rst.template", @"docs\special-keywords\index.rst"),
        
        // querying-json-files\special-keywords\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\special-keywords\index.rst.template", @"docs\special-keywords\index.rst"),
        
        // querying-json-files\special-keywords\index\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\special-keywords\index\index.rst.template", @"docs\special-keywords\index\index.rst"),
        
        // querying-json-files\special-keywords\parent\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\special-keywords\parent\index.rst.template", @"docs\special-keywords\parent\index.rst"),
        
        // querying-json-files\special-keywords\this\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\special-keywords\this\index.rst.template", @"docs\special-keywords\this\index.rst"),
        
        // json-mutator-operators\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-mutator-operators\index.rst.template", @"docs\json-mutator-operators\index.rst"),

        // json-mutator-operators\string-interpolation\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-mutator-operators\string-interpolation\index.rst.template", @"docs\json-mutator-operators\string-interpolation\index.rst"),

        // json-mutator-operators\value\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-mutator-operators\value\index.rst.template", @"docs\json-mutator-operators\value\index.rst"),

        // json-mutator-operators\copy-fields\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-mutator-operators\copy-fields\index.rst.template", @"docs\json-mutator-operators\copy-fields\index.rst"),

        // json-mutator-operators\merge-array\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-mutator-operators\merge-array\index.rst.template", @"docs\json-mutator-operators\merge-array\index.rst"),
        
        // json-path-functions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-path-functions\index.rst.template", @"docs\json-path-functions\index.rst"),
        
        // json-path-functions\array-indexers\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-path-functions\array-indexers\index.rst.template", @"docs\json-path-functions\array-indexers\index.rst"),

        // json-path-functions\at\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-path-functions\at\index.rst.template", @"docs\json-path-functions\at\index.rst"),
        
        // json-path-functions\first\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-path-functions\first\index.rst.template", @"docs\json-path-functions\first\index.rst"),

        // json-path-functions\last\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-path-functions\last\index.rst.template", @"docs\json-path-functions\last\index.rst"),
        
        // json-path-functions\flatten\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-path-functions\flatten\index.rst.template", @"docs\json-path-functions\flatten\index.rst"),
        
        // json-path-functions\reverse\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-path-functions\reverse\index.rst.template", @"docs\json-path-functions\reverse\index.rst"),
        
        // json-path-functions\select\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-path-functions\select\index.rst.template", @"docs\json-path-functions\select\index.rst"),
        
        // json-path-functions\where\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\json-path-functions\where\index.rst.template", @"docs\json-path-functions\where\index.rst"),
        
        // functions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\index.rst.template", @"docs\functions\index.rst"),
        
        // functions\conversion-functions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\conversion-functions\index.rst.template", @"docs\functions\conversion-functions\index.rst"),
        
        // functions\conversion-functions\to-boolean\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\conversion-functions\to-boolean\index.rst.template", @"docs\functions\conversion-functions\to-boolean\index.rst"),

        // functions\conversion-functions\to-date\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\conversion-functions\to-date\index.rst.template", @"docs\functions\conversion-functions\to-date\index.rst"),
        
        // functions\conversion-functions\to-date-time\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\conversion-functions\to-date-time\index.rst.template", @"docs\functions\conversion-functions\to-date-time\index.rst"),
        
        // functions\conversion-functions\to-double\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\conversion-functions\to-double\index.rst.template", @"docs\functions\conversion-functions\to-double\index.rst"),
        
        // functions\conversion-functions\to-int\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\conversion-functions\to-int\index.rst.template", @"docs\functions\conversion-functions\to-int\index.rst"),
        
        // functions\conversion-functions\to-string\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\conversion-functions\to-string\index.rst.template", @"docs\functions\conversion-functions\to-string\index.rst"),
        
        // functions\numeric-functions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\numeric-functions\index.rst.template", @"docs\functions\numeric-functions\index.rst"),
        
        // functions\numeric-functions\abs\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\numeric-functions\abs\index.rst.template", @"docs\functions\numeric-functions\abs\index.rst"),
        
        // functions\boolean-functions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\boolean-functions\index.rst.template", @"docs\functions\boolean-functions\index.rst"),
        
        // functions\boolean-functions\has-field\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\boolean-functions\has-field\index.rst.template", @"docs\functions\boolean-functions\has-field\index.rst"),
        
        // functions\boolean-functions\is-even\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\boolean-functions\is-even\index.rst.template", @"docs\functions\boolean-functions\is-even\index.rst"),
        
        // functions\boolean-functions\is-odd\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\boolean-functions\is-odd\index.rst.template", @"docs\functions\boolean-functions\is-odd\index.rst"),

        // functions\string-functions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\string-functions\index.rst.template", @"docs\functions\string-functions\index.rst"),
        
        //------------------------------------
        // functions\string-functions\concatenate\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\string-functions\concatenate\index.rst.template", @"docs\functions\string-functions\concatenate\index.rst"),

        // functions\string-functions\len\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\string-functions\len\index.rst.template", @"docs\functions\string-functions\len\index.rst"),
        
        // functions\string-functions\lower\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\string-functions\lower\index.rst.template", @"docs\functions\string-functions\lower\index.rst"),
        
        // functions\string-functions\upper\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\functions\string-functions\upper\index.rst.template", @"docs\functions\string-functions\upper\index.rst"),
        //------------------------------------
        // aggregate-functions\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\aggregate-functions\index.rst.template", @"docs\aggregate-functions\index.rst"),
        
        // aggregate-functions\all\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\aggregate-functions\all\index.rst.template", @"docs\aggregate-functions\all\index.rst"),
        
        // aggregate-functions\any\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\aggregate-functions\any\index.rst.template", @"docs\aggregate-functions\any\index.rst"),
        
        // aggregate-functions\average\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\aggregate-functions\average\index.rst.template", @"docs\aggregate-functions\average\index.rst"),
        
        // aggregate-functions\count\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\aggregate-functions\count\index.rst.template", @"docs\aggregate-functions\count\index.rst"),

        // aggregate-functions\max\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\aggregate-functions\max\index.rst.template", @"docs\aggregate-functions\max\index.rst"),

        // aggregate-functions\min\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\aggregate-functions\min\index.rst.template", @"docs\aggregate-functions\min\index.rst"),

        // aggregate-functions\sum\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\aggregate-functions\sum\index.rst.template", @"docs\aggregate-functions\sum\index.rst"),
        
        // operators\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\operators\index.rst.template", @"docs\operators\index.rst"),
        
        // optional-and-named-parameters\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\optional-and-named-parameters\index.rst.template", @"docs\optional-and-named-parameters\index.rst"),
        
        // dependency-injection-setup\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\dependency-injection-setup\index.rst.template", @"docs\dependency-injection-setup\index.rst"),

        // custom-json-ql\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\custom-json-ql\index.rst.template", @"docs\custom-json-ql\index.rst"),
        
        // future-releases\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\future-releases\index.rst.template", @"docs\future-releases\index.rst"),
        
        // future-releases\complex-projections\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\future-releases\complex-projections\index.rst.template", @"docs\future-releases\complex-projections\index.rst"),
        
        // future-releases\grouping\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\future-releases\grouping\index.rst.template", @"docs\future-releases\grouping\index.rst"),
        
        // future-releases\multiline-queries\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\future-releases\multiline-queries\index.rst.template", @"docs\future-releases\multiline-queries\index.rst"),
        
        // future-releases\sorting\index.rst.template file related
        (@"JsonQL.Demos\DocFiles\future-releases\sorting\index.rst.template", @"docs\future-releases\sorting\index.rst"),
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
