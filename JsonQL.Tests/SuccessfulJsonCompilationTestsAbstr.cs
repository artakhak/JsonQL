using JsonQL.Compilation;
using JsonQL.Diagnostics;
using JsonQL.Utilities;
using OROptimizer.ServiceResolver;
using OROptimizer.ServiceResolver.DefaultImplementationBasedObjectFactory;

namespace JsonQL.Tests;

public abstract class SuccessfulJsonCompilationTestsAbstr : JsonCompilationTestsAbstr
{
    private readonly IJsonSerializer _jsonSerializer = new JsonSerializer();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="folderNames">
    /// List of folder names that specifies path relative to "JsonFiles" folder of the folder containing tested json files.
    /// For example if <param name="folderNames"></param> is ["SimpleValueReferences", "Examples1"], then the files will
    /// be in folder "JsonFiles/SimpleValueReferences/SimpleExamples".
    /// </param>
    /// <param name="compiledFileName">Compiled file name.</param>
    /// <param name="parentFileName">If the value is not null, files used as a parent for <param name="compiledFileName"></param></param>
    /// <param name="parentParentFileName">If the value is not null, files used as a parent for <param name="compiledFileName"></param></param>
    protected async Task DoSuccessfulTest(IReadOnlyList<string> folderNames, string compiledFileName, string? parentFileName = null, string? parentParentFileName = null)
    {
        var jsonTextDataLoader = new JsonTextDataLoader(folderNames);

        var jsonTextData = jsonTextDataLoader
            .GetJsonTextData(compiledFileName, parentFileName, parentParentFileName);

        await DoSuccessfulTest(jsonTextData, folderNames);
    }
    
    protected async Task DoSuccessfulTest(ResourcePath compiledFilePath, params ResourcePath[] parentFilePaths)
    {
        IJsonTextData? parentJsonTextData = null;

        if (parentFilePaths.Length > 0)
        {
            for (var i = parentFilePaths.Length - 1; i >= 0; --i)
            {
                var parentFilePath = parentFilePaths[i];
                var currentParentJsonTextData = new JsonTextData(parentFilePath.GetFilePath(),
                    ResourceFileLoader.LoadJsonFile(parentFilePath), parentJsonTextData);

                parentJsonTextData = currentParentJsonTextData;
            }
        }

        await DoSuccessfulTest(new JsonTextData(compiledFilePath.GetFilePath(),
            ResourceFileLoader.LoadJsonFile(compiledFilePath), parentJsonTextData), 
            compiledFilePath.PathFolderNames);
    }

    private async Task DoSuccessfulTest(IJsonTextData jsonTextData, IReadOnlyList<string> expectedCompiledJsonFilePathFolderNames)
    {
        // Init
        DiBasedObjectFactoryParametersContext.Context = new DiBasedObjectFactoryParameters
        {
            LogDiagnosticsData = true
        };

        var jsonParser = JsonQLDefaultImplementationBasedObjectFactory.CreateInstance<IJsonParser>();

        // Act
        var compilationResult = JsonCompiler.Compile(jsonTextData);

        // Assert
        var compiledJsonData = compilationResult.CompiledJsonFiles.FirstOrDefault(x => x.TextIdentifier == jsonTextData.TextIdentifier);

        Assert.That(compiledJsonData, Is.Not.Null);

        await SaveCompiledJsonFileAsync(compiledJsonData!);
        var expectedCompiledJsonFile = ResourceFileLoader.LoadJsonFile(
            new ResourcePath("ExpectedCompiledJson.json", expectedCompiledJsonFilePathFolderNames));

        ParsedJsonValidator.ValidateRootParsedJson(compiledJsonData!.CompiledParsedValue, jsonParser.Parse(expectedCompiledJsonFile, jsonTextData.TextIdentifier));
    }
    
    private async Task<string> SaveCompiledJsonFileAsync(ICompiledJsonData compiledJsonData)
    {
        var serializedJson = _jsonSerializer.Serialize(compiledJsonData.CompiledParsedValue);
        await ResourceFileHelpers.SaveAsync(serializedJson, "CompiledTestJson.json");
        return serializedJson;
    }
}
