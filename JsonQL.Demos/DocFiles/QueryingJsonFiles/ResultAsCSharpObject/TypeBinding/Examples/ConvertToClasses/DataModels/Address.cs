namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels;

public class Address
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string? County { get; set; }
}
