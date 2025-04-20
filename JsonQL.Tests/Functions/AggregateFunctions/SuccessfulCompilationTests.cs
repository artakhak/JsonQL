namespace JsonQL.Tests.Functions.AggregateFunctions;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task AggregateFunctions_Count()
    {
        await DoSuccessfulTest(["Functions", "AggregateFunctions", "Count"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task AggregateFunctions_All()
    {
        await DoSuccessfulTest(["Functions", "AggregateFunctions", "All"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task AggregateFunctions_Any()
    {
        await DoSuccessfulTest(["Functions", "AggregateFunctions", "Any"], "JsonFile2.json", "JsonFile1.json",
            "Companies.json");
    }

    [Test]
    public async Task AggregateFunctions_Average()
    {
        await DoSuccessfulTest(["Functions", "AggregateFunctions", "Average"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task AggregateFunctions_Min()
    {
        await DoSuccessfulTest(["Functions", "AggregateFunctions", "Min"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task AggregateFunctions_Max()
    {
        await DoSuccessfulTest(["Functions", "AggregateFunctions", "Max"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task AggregateFunctions_Sum()
    {
        await DoSuccessfulTest(["Functions", "AggregateFunctions", "Sum"], "JsonFile2.json", "JsonFile1.json");
    }
}