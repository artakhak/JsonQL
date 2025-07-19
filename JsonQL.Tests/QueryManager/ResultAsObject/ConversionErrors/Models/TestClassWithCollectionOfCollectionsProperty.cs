namespace JsonQL.Tests.QueryManager.ResultAsObject.ConversionErrors.Models;

public class TestClassWithCollectionOfCollectionsProperty
{
    public List<IReadOnlyList<IEmployee[]>> CollectionOfCollections { get; set; } = null!;
}