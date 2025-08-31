using FileInclude;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.DocumentationGenerator;

internal class DocumentsGenerator
{
    private readonly (string teplateFileRelativePath, string generatedFileRelativePath)[] _filesRelativePathsData = 
    {
        (@"JsonQL.Demos\DocFiles\README.md.template", "README.md"),
        (@"JsonQL.Demos\DocFiles\README.md.template", @"JsonQL\README.md"),
        (@"JsonQL.Demos\DocFiles\index.rst.template", @"docs\index.rst"),
        
        (@"JsonQL.Demos\DocFiles\index-rst-files\query-and-convert-json-to-csharp-objects.data-1.rst.template", @"docs\index-rst-files\query-and-convert-json-to-csharp-objects.data-1.rst"),
        (@"JsonQL.Demos\DocFiles\index-rst-files\query-and-convert-json-to-csharp-objects.result.rst.template", @"docs\index-rst-files\query-and-convert-json-to-csharp-objects.result.rst"),
        
        (@"JsonQL.Demos\DocFiles\index-rst-files\query-and-convert-json-to-collection-of-doubles.data-1.rst.template", @"docs\index-rst-files\query-and-convert-json-to-collection-of-doubles.data-1.rst"),
        (@"JsonQL.Demos\DocFiles\index-rst-files\query-and-convert-json-to-collection-of-doubles.result.rst.template", @"docs\index-rst-files\query-and-convert-json-to-collection-of-doubles.result.rst"),
        
        (@"JsonQL.Demos\DocFiles\index-rst-files\query-with-result-as-json-object-1.data.rst.template", @"docs\index-rst-files\query-with-result-as-json-object-1.data.rst"),
        (@"JsonQL.Demos\DocFiles\index-rst-files\query-with-result-as-json-object.result.rst.template", @"docs\index-rst-files\query-with-result-as-json-object.result.rst")
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
