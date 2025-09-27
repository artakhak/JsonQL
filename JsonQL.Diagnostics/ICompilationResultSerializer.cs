using System.Runtime.ExceptionServices;
using System.Text;
using JsonQL.Compilation;
using JsonQL.JsonObjects;
using JsonQL.Query;
using JsonQL.Utilities;

namespace JsonQL.Diagnostics;

public interface ICompilationResultSerializer
{
    string Serialize(ICompilationResult compilationResult,
        Func<ICompiledJsonData, bool> compiledJsonDataShouldBeIncluded);
    string Serialize(IJsonValueQueryResult jsonValueQueryResult);
    string Serialize(IObjectQueryResult objectQueryResult);
}

public class CompilationResultSerializer : ICompilationResultSerializer
{
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IClassSerializer _classSerializer;

    private static readonly JsonSerializerParameters _jsonSerializerParameters = new()
    {
        IndentationFromParent = "  ",
        Minify = false,
        NewLineIndentation = string.Empty
    };

    public CompilationResultSerializer(IJsonSerializer jsonSerializer, IClassSerializer classSerializer)
    {
        _jsonSerializer = jsonSerializer;
        _classSerializer = classSerializer;
    }


    private string IndentJsonValue(string jsonVale, string indention)
    {
        var lines = jsonVale.Split(Environment.NewLine);

        var indentedValue = new StringBuilder();

        for (var i = 0; i < lines.Length; ++i)
        {
            indentedValue.Append(indention).Append(lines[i]);

            if (i < lines.Length - 1)
                indentedValue.AppendLine();
        }

        return indentedValue.ToString();
    }

    private JsonSerializerParameters CreateJsonSerializerParameters()
    {
        return new JsonSerializerParameters
        {
            Minify = false,
            IndentationFromParent = _jsonSerializerParameters.IndentationFromParent
        };
    }

    /// <inheritdoc />
    public string Serialize(ICompilationResult compilationResult, Func<ICompiledJsonData, bool> compiledJsonDataShouldBeIncluded)
    {
        string indention = _jsonSerializerParameters.IndentationFromParent;
        var jsonSerializerParameters = CreateJsonSerializerParameters();

        var serializedText = new StringBuilder();
        serializedText.AppendLine("{");
        serializedText.Append(indention)
            .Append("\"")
            .Append(nameof(ICompilationResult.CompiledJsonFiles))
            .Append("\":")
            .Append("[");

        var compiledJsonFilesData = compilationResult.CompiledJsonFiles.Where(compiledJsonDataShouldBeIncluded).ToList();
        
        for (var currentFileIndex = 0; currentFileIndex < compiledJsonFilesData.Count; ++currentFileIndex)
        {
            var compiledJsonData = compiledJsonFilesData[currentFileIndex];

            var level1Indention = string.Concat(indention, indention);
            var level2Indention = string.Concat(indention, indention, indention);

            serializedText.AppendLine();
            serializedText.Append(level1Indention)
                .AppendLine("{")
                .AppendLine();

            serializedText
                .Append(level2Indention)
                .Append("\"")
                .Append(nameof(ICompiledJsonData.TextIdentifier))
                .Append("\": \"")
                .Append(compiledJsonData.TextIdentifier)
                .AppendLine("\",");

            serializedText
                .Append(level2Indention)
                .Append("\"")
                .Append(nameof(ICompiledJsonData.CompiledParsedValue))
                .Append("\": ");

            jsonSerializerParameters.NewLineIndentation = level2Indention;
            serializedText.AppendLine();
            serializedText.Append(level2Indention);
            serializedText.Append(this._jsonSerializer.Serialize(compiledJsonData.CompiledParsedValue, jsonSerializerParameters));

            serializedText.AppendLine()
                .Append(level1Indention)
                .Append("}");

            if (currentFileIndex < compiledJsonFilesData.Count - 1)
                serializedText.AppendLine(",");

            serializedText.AppendLine();
        }

        serializedText.AppendLine();
        serializedText
            .Append(indention)
            .AppendLine("],");

        serializedText.Append(indention)
            .Append("\"")
            .Append(nameof(IJsonValueQueryResult.CompilationErrors))
            .AppendLine("\": ")
            .AppendLine(IndentJsonValue(_classSerializer.Serialize(compilationResult.CompilationErrors), indention));

        serializedText.AppendLine("}");
        return serializedText.ToString();
    }

    /// <inheritdoc />
    public string Serialize(IJsonValueQueryResult jsonValueQueryResult)
    {
        string indention = _jsonSerializerParameters.IndentationFromParent;
        var jsonSerializerParameters = CreateJsonSerializerParameters();

        var serializedText = new StringBuilder();
        serializedText.AppendLine("{");

        serializedText.Append(indention)
            .Append("\"")
            .Append(nameof(IJsonValueQueryResult.ParsedValue))
            .Append("\": ");

        if (jsonValueQueryResult.ParsedValue == null)
        {
            serializedText.Append("null");
        }
        else if (jsonValueQueryResult.ParsedValue is IParsedSimpleValue)
        {
            serializedText.Append(this._jsonSerializer.Serialize(jsonValueQueryResult.ParsedValue, jsonSerializerParameters));
        }
        else
        {
            jsonSerializerParameters.NewLineIndentation = indention;
            serializedText.AppendLine();
            serializedText.Append(indention);
            serializedText.Append(this._jsonSerializer.Serialize(jsonValueQueryResult.ParsedValue, jsonSerializerParameters));
        }

        serializedText.AppendLine(",");

        serializedText.Append(indention)
            .Append("\"")
            .Append(nameof(IJsonValueQueryResult.CompilationErrors))
            .AppendLine("\": ")
            .AppendLine(IndentJsonValue(_classSerializer.Serialize(jsonValueQueryResult.CompilationErrors), indention));

        serializedText.AppendLine("}");
        return serializedText.ToString();
    }


    /// <inheritdoc />
    public string Serialize(IObjectQueryResult objectQueryResult)
    {
        return ClassSerializerAmbientContext.Context.Serialize(objectQueryResult);
    }
}
