namespace JsonQL.JsonToObjectConversion.NullabilityCheck.Diagnostics.TestClasses;

internal class TestClass1
{
    internal TestClass2 NonNullableRefValueTypeProperty { get; set; } = null!;
    internal TestClass2? NullableRefValueTypeProperty { get; set; } = null!;

    #region lists with reference type items
    internal IReadOnlyList<TestClass2> NonNullableList1 { get; set; } = null!;
    internal IReadOnlyList<TestClass2?> NonNullableList2 { get; set; } = null!;
    internal IReadOnlyList<TestClass2>? NullableList1 { get; set; }
    internal IReadOnlyList<TestClass2?>? NullableList2 { get; set; } 
    #endregion

    #region Lists of lists with reference type items
    internal IReadOnlyList<IEnumerable<TestClass2>> NonNullableListOfLists1 { get; set; } = null!;
    internal IReadOnlyList<IEnumerable<TestClass2>?> NonNullableListOfLists2 { get; set; } = null!;
    internal IReadOnlyList<IEnumerable<TestClass2?>> NonNullableListOfLists3 { get; set; } = null!;
    internal IReadOnlyList<IEnumerable<TestClass2?>?> NonNullableListOfLists4 { get; set; } = null!;

    internal IReadOnlyList<IEnumerable<TestClass2>>? NullableListOfLists1 { get; set; }
    internal IReadOnlyList<IEnumerable<TestClass2>?>? NullableListOfLists2 { get; set; }
    internal IReadOnlyList<IEnumerable<TestClass2?>>? NullableListOfLists3 { get; set; }
    internal IReadOnlyList<IEnumerable<TestClass2?>?>? NullableListOfLists4 { get; set; }
    #endregion
}