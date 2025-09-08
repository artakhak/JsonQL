using FileInclude;
using OROptimizer.Diagnostics.Log;
using OROptimizer.Utilities;
using System.Text;

namespace JsonQL.DocumentationGenerator;

internal class DocumentGenerator
{
    private readonly ITemplateProcessor _templateProcessor;
    private readonly string _solutionFolderPath;

    internal DocumentGenerator(ITemplateProcessor templateProcessor, string solutionFolderPath)
    {
        _templateProcessor = templateProcessor;
        _solutionFolderPath = solutionFolderPath;
    }

    internal bool GenerateFileFromTemplate(string templateFileRelativePath, string generatedFileRelativePath)
    {
        var templateAbsoluteFilePathResult = FilePathHelpers.TryGetAbsoluteFilePath(_solutionFolderPath, templateFileRelativePath);

        if (!templateAbsoluteFilePathResult.isSuccess)
        {
            LogHelper.Context.Log.Error(templateAbsoluteFilePathResult.errorMessage);
            return false;
        }

        var generatedFileAbsolutePathResult = FilePathHelpers.TryGetAbsoluteFilePath(_solutionFolderPath, generatedFileRelativePath);

        if (!generatedFileAbsolutePathResult.isSuccess)
        {
            LogHelper.Context.Log.Error(generatedFileAbsolutePathResult.errorMessage);
            return false;
        }

        var errors = _templateProcessor.GenerateFileFromTemplateAndSave(templateAbsoluteFilePathResult.absoluteFilePath,
            generatedFileAbsolutePathResult.absoluteFilePath);

        if (errors.Count > 0)
        {
            LogHelper.Context.Log.InfoFormat("Generation of file '{0}' from template file '{1}' completed with errors!",
                generatedFileAbsolutePathResult.absoluteFilePath, templateFileRelativePath);

            foreach (var errorData in errors)
                LogError(errorData);

            return false;
        }

        LogHelper.Context.Log.InfoFormat("File '{0}' was successfully generated from template file '{1}'!",
            generatedFileAbsolutePathResult.absoluteFilePath, templateAbsoluteFilePathResult.absoluteFilePath);

        return true;
    }

    private void LogError(IErrorData errorData)
    {
        var contextData = new StringBuilder();

        contextData.Append($"Context data: [{nameof(IErrorData.ErrorCode)}:{errorData.ErrorCode}");

        if (errorData.ErrorPosition != null)
            contextData.Append($", {nameof(IErrorData.ErrorPosition)}:{errorData.ErrorPosition}");

        contextData.Append($", {nameof(IErrorData.SourceFilePath)}:'{errorData.SourceFilePath}']");

        log4net.GlobalContext.Properties["context"] = contextData.ToString();

        if (errorData.Exception != null)
            LogHelper.Context.Log.Error(errorData.ErrorMessage, errorData.Exception);
        else
            LogHelper.Context.Log.Error(errorData.ErrorMessage);
    }
}
