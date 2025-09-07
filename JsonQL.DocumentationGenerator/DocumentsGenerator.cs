using FileInclude;
using OROptimizer.Diagnostics.Log;

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

    public bool GenerateDocumentsFromTemplates()
    {
        try
        {
            void GenerateFileFromTemplate(string templateFileRelativePath, string generatedFileRelativePath)
            {
                if (!_documentGenerator.GenerateFileFromTemplate(templateFileRelativePath, generatedFileRelativePath))
                {
                    throw new ApplicationException("Template generation failed.");
                }
            }

            string demoDocsFolder = Path.Join(_solutionFolderPath, SrcDocFilesPathRelativeToSolutionFolder);
              
            // Copy non-template files first
            IterateFolderFiles(
                demoDocsFolder, SrcDocFilesPathRelativeToSolutionFolder, filesData =>
                {
                    if (!IsTemplateFile(filesData.srcFilePathRelativeToSolutionFolder))
                        GenerateFileFromTemplate(filesData.srcFilePathRelativeToSolutionFolder, filesData.generatedFilePathRelativeToSolutionFolder);
                });
         
            #region Copy README.md files separately

            var readMeTemplateRelativePath = $"JsonQL.Demos{Path.DirectorySeparatorChar}DocFiles{Path.DirectorySeparatorChar}README.md.template";
            GenerateFileFromTemplate(readMeTemplateRelativePath, "README.md");
            GenerateFileFromTemplate(readMeTemplateRelativePath, $"JsonQL{Path.DirectorySeparatorChar}README.md"); 
            #endregion

            // Copy template files next
            IterateFolderFiles(
                demoDocsFolder, SrcDocFilesPathRelativeToSolutionFolder, filesData =>
                {
                    if (string.Equals(filesData.srcFilePathRelativeToSolutionFolder, readMeTemplateRelativePath, StringComparison.OrdinalIgnoreCase))
                        return;

                    if (IsTemplateFile(filesData.srcFilePathRelativeToSolutionFolder))
                        GenerateFileFromTemplate(filesData.srcFilePathRelativeToSolutionFolder, filesData.generatedFilePathRelativeToSolutionFolder);
                });

            return true;
        }
        catch (Exception e)
        {
            LogHelper.Context.Log.Error("Failed to generate documents", e);
            return false;
        }
    }

    private bool IsTemplateFile(string filePath)
    {
        var fileExtension = Path.GetExtension(filePath);
        return String.Equals(fileExtension, TemplateExtension, StringComparison.OrdinalIgnoreCase);
    }

    private void IterateFolderFiles(string folderToIterate, string srcRootFolderPathRelativeToSolutionFolder, 
        Action<(string srcAbsoluteFilePath, string srcFilePathRelativeToSolutionFolder, string generatedFilePathRelativeToSolutionFolder)> fileProcessor)
    {
        // Add all filePathsToCopy in the current solutionFolderPath
        foreach (var srcFilePath in Directory.GetFiles(folderToIterate))
        {
            var relativeFilePaths = GetFileRelativePathsRelativeToSolutionFolder(srcRootFolderPathRelativeToSolutionFolder, srcFilePath);
            fileProcessor((srcFilePath, relativeFilePaths.srcFilePathRelativeToSolutionFolder, relativeFilePaths.generatedFilePathRelativeToSolutionFolder));
        }

        // Recursively process subdirectories
        foreach (var subDirectory in Directory.GetDirectories(folderToIterate))
        {
            IterateFolderFiles(subDirectory, srcRootFolderPathRelativeToSolutionFolder,  fileProcessor);
        }
    }

    private (string srcFilePathRelativeToSolutionFolder, string generatedFilePathRelativeToSolutionFolder) GetFileRelativePathsRelativeToSolutionFolder(string srcRootFolderRelativePath, string srcFilePath)
    {
        // Example of how the values of srcFilePathRelativeToSolutionFolder and generatedFilePathRelativeToSolutionFolder are calculated in this method below:
        // Consider example values of DocsRootRelativePath, srcRootFolderRelativePath, and srcFilePath shown here.
        // DocsRootRelativePath="\docs" (this is the value of constant DocsRootRelativePath and is the root destination folder relative to solution folder)
        // srcRootFolderRelativePath="\JsonQL.Demos\"
        // srcFilePath = "c:\JsonQL\JsonQL.Demos\DocFiles\Examples\json-with-json-ql-expressions.data-1.rst.template"
        // We want to calculate srcFilePathRelativeToSolutionFolder and generatedFilePathRelativeToSolutionFolder from DocsRootRelativePath, srcRootFolderRelativePath and srcFilePath 
        // to have these values:
        // srcFilePathRelativeToSolutionFolder = "JsonQL.Demos\DocFiles\Examples\json-with-json-ql-expressions.data-1.rst.template"
        // generatedFilePathRelativeToSolutionFolder = "docs\DocFiles\Examples\json-with-json-ql-expressions.data-1.rst.template"

        string? srcFilePathRelativeToSolutionFolder = null;
        string? generatedFilePathRelativeToSolutionFolder = null;

        try
        {
            var indexOfSrcFolder = srcFilePath.IndexOf(srcRootFolderRelativePath, StringComparison.Ordinal);

            if (indexOfSrcFolder < 0)
                throw new ApplicationException($"The value of '{srcRootFolderRelativePath}' is not in '{srcFilePath}'");

            srcFilePathRelativeToSolutionFolder = srcFilePath.Substring(indexOfSrcFolder);

            if (srcFilePathRelativeToSolutionFolder.StartsWith(Path.DirectorySeparatorChar))
                srcFilePathRelativeToSolutionFolder = srcFilePathRelativeToSolutionFolder.Substring(1);

            generatedFilePathRelativeToSolutionFolder = Path.Join(DocsRootRelativePath, srcFilePath.Substring(indexOfSrcFolder + srcRootFolderRelativePath.Length));

            var generatedFileRelativePathStartIndex = 0;
            var generatedFileRelativePathLength = generatedFilePathRelativeToSolutionFolder.Length;

            if (generatedFilePathRelativeToSolutionFolder.StartsWith(Path.DirectorySeparatorChar))
            {
                generatedFileRelativePathStartIndex = 1;
                --generatedFileRelativePathLength;
            }

            if (generatedFilePathRelativeToSolutionFolder.EndsWith(TemplateExtension))
                generatedFileRelativePathLength -= TemplateExtension.Length;

            if (generatedFileRelativePathStartIndex > 0 || generatedFileRelativePathLength != generatedFilePathRelativeToSolutionFolder.Length)
                generatedFilePathRelativeToSolutionFolder = generatedFilePathRelativeToSolutionFolder.Substring(generatedFileRelativePathStartIndex, generatedFileRelativePathLength);

            return (srcFilePathRelativeToSolutionFolder, generatedFilePathRelativeToSolutionFolder);
        }
        catch
        {
            LogHelper.Context.Log.ErrorFormat("Template generation failed. Src file path: '{0}', Src file relative path: '{1}', Generated relative path: '{2}'.",
                srcFilePath, srcFilePathRelativeToSolutionFolder, generatedFilePathRelativeToSolutionFolder);
            throw;
        }
    }
}
