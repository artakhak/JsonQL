namespace JsonQL.Tests.QueryManager.ResultAsObject.JsonConversionSettingsOverrides.Models;

public class Employee : IEmployee
{
    // Example when property can be set in constructor.
    public Employee(long id)
    {
        Id = id;
    }

    public long Id { get; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public IAddress Address { get; set; } = null!;
    public int Salary { get; set; }
    public int Age { get; set; }
    public IManager? Manager { get; set; }

    /// <summary>
    /// Example of <see cref="List{T}"/> property. Other examples use <see cref="IReadOnlyList{T}"/>
    /// </summary>
    public List<string> Phones { get; set; } = null!;
}