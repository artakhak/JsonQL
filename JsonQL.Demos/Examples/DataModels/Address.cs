namespace JsonQL.Demos.Examples.DataModels;

public class Address : IAddress
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string? County { get; set; }
}
