public static void ParseJsonWithJsonQLExpressions()
{
    var additionalTestData = new JsonTextData(
        "AdditionalTestData",
        this.LoadExampleJsonFile("AdditionalTestData.json"));

    var countriesJsonTextData = new JsonTextData(
        "Countries",
        this.LoadExampleJsonFile("Countries.json"), additionalTestData);

    var companiesJsonTextData = new JsonTextData("Companies",
        this.LoadExampleJsonFile("Companies.json"), countriesJsonTextData);

    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!; // Create an instance of JsonQL.Compilation.JsonCompiler here.
                                    //This is normally done once on application start.
    var result = jsonCompiler.Compile(new JsonTextData("Overview",
        this.LoadExampleJsonFile("Overview.json"), companiesJsonTextData));
}