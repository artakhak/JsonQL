namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

public interface IJsonComparable
{
    /// <summary>
    /// Type code.
    /// </summary>
    TypeCode TypeCode { get; }

    /// <summary>
    /// A value of one of the following types: <see cref="String"/>, <see cref="Double"/>, <see cref="Boolean"/>, <see cref="DateTime"/>
    /// </summary>
    IComparable Value { get; }
}

public static class JsonComparableExtensions
{
    /// <summary>
    /// Converts the value <see cref="IJsonComparable.Value"/> or throws <see cref="System."/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonComparable"></param>
    /// <exception cref="InvalidCastException">Throws this exception.</exception>
    public static T ConvertValueOrThrow<T>(this IJsonComparable jsonComparable)
    {
        
        switch (jsonComparable.TypeCode)
        {
            case TypeCode.Boolean:
                if (typeof(T) == typeof(bool) && jsonComparable.Value is Boolean)
                    return (T) jsonComparable.Value;
                break;

            case TypeCode.String:
                if (typeof(T) == typeof(string) && jsonComparable.Value is String)
                    return (T)jsonComparable.Value;
                break;

            case TypeCode.Double:
                if (typeof(T) == typeof(double) && jsonComparable.Value is Double)
                    return (T)jsonComparable.Value;
                break;

            case TypeCode.DateTime:
                if (typeof(T) == typeof(DateTime) && jsonComparable.Value is DateTime)
                    return (T)jsonComparable.Value;
                break;
        }
       
        throw new InvalidCastException($"Value of type [{jsonComparable.Value.GetType()}] with type code [{jsonComparable.TypeCode}] cannot be converted to [{typeof(T)}].");
    }
}