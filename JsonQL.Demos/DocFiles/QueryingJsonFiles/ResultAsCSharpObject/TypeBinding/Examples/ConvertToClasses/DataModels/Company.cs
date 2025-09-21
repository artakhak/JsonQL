namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels;

public class Company
{
    // Example when property can be set in constructor.
    public Company(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public IReadOnlyList<Employee> Employees { get; set; } = null!;
}
