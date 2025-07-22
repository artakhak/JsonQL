namespace JsonQL.Demos.Examples.DataModels;

public class Manager : Employee, IManager
{
    // Example when property can be set in constructor.
    public Manager(long id) : base(id)
    {
    }

    public IReadOnlyList<IEmployee> Employees { get; set; } = null!;
}
