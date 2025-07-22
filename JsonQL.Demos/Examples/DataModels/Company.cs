namespace JsonQL.Demos.Examples.DataModels;

public class Company : ICompany
{
    // Example when property can be set in constructor.
    public Company(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public IReadOnlyList<IEmployee> Employees { get; set; } = null!;
}
