namespace JsonQL.Tests.QueryManager.ResultAsObject.Models;

public interface IEmployee
{
    long Id { get; }
    string FirstName { get; }
    string LastName { get; }
    IAddress? Address { get; }

    /// <summary>
    /// This property uses <see cref="IReadOnlyList{T}"/> of <see cref="Address"/> items
    /// rather than <see cref="IAddress"/> intnetionally to demonstrate that the serializer
    /// can deserialize both to interfaces as in property <see cref="Address"/> (using the default
    /// implementation if it exists), as well as to classes.
    /// </summary>
    public IReadOnlyList<Address>? EmergencyContacts { get; }
    int Salary { get; }
    int? Age { get; }
    IManager? Manager { get; }

    List<string> Phones { get; }
}
