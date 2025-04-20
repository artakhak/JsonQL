using System.Collections.Generic;

namespace JsonQL.Extensions.JsonToObjectConversion.NullabilityCheck.Diagnostics.TestClasses;

internal class TestClass1
{
    public TestClass2 NonNullableRefValueTypeProperty { get; set; } = null!;
    public TestClass2? NullableRefValueTypeProperty { get; set; } = null!;

    #region lists with reference type items
    public IReadOnlyList<TestClass2> NonNullableList1 { get; set; } = null!;
    public IReadOnlyList<TestClass2?> NonNullableList2 { get; set; } = null!;
    public IReadOnlyList<TestClass2>? NullableList1 { get; set; }
    public IReadOnlyList<TestClass2?>? NullableList2 { get; set; } 
    #endregion

    #region Lists of lists with reference type items
    public IReadOnlyList<IEnumerable<TestClass2>> NonNullableListOfLists1 { get; set; } = null!;
    public IReadOnlyList<IEnumerable<TestClass2>?> NonNullableListOfLists2 { get; set; } = null!;
    public IReadOnlyList<IEnumerable<TestClass2?>> NonNullableListOfLists3 { get; set; } = null!;
    public IReadOnlyList<IEnumerable<TestClass2?>?> NonNullableListOfLists4 { get; set; } = null!;

    public IReadOnlyList<IEnumerable<TestClass2>>? NullableListOfLists1 { get; set; }
    public IReadOnlyList<IEnumerable<TestClass2>?>? NullableListOfLists2 { get; set; }
    public IReadOnlyList<IEnumerable<TestClass2?>>? NullableListOfLists3 { get; set; }
    public IReadOnlyList<IEnumerable<TestClass2?>?>? NullableListOfLists4 { get; set; }
    #endregion
}