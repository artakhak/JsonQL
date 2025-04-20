using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction;

public static class FunctionReturnAndParameterTypes
{
    public static Guid Any = Guid.Parse("AD263677-8E5C-488E-82E7-3F675B5ADB2B");
    public static Guid Boolean = Guid.Parse("A1FF5282-A63E-4805-8651-6AED57ED7746");
    public static Guid Double = Guid.Parse("C2B146A7-EFCA-44A4-B47C-E8723BAEADF1");
    public static Guid DateTime = Guid.Parse("F37C675D-45B7-4A81-B659-395B204C2DE9");
    public static Guid String = Guid.Parse("BE9DD307-BE24-4E81-9A6E-83B45E396206");
    
    /// <summary>
    /// An instance of <see cref="IParsedValue"/> referenced by <see cref="IJsonValuePath"/>
    /// </summary>
    public static Guid ReferencedParsedJsonValue = Guid.Parse("830E819E-41BE-42C7-8F90-58895903537F");

    /// <summary>
    /// An instance of <see cref="IParsedValue"/> referenced by <see cref="IJsonValuePath"/>
    /// </summary>
    public static Guid ArrayOfReferencedParsedJsonValues = Guid.Parse("50C8AB33-E9CE-4FC9-84F9-CFE22BFA8AF0");
}