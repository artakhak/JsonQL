namespace JsonQL.Query;

public static class Constants
{
    //private const string OpeningBrace = "{";

    public const string QueryKey = "query";
    public const string QueryTextIdentifier = "Query_849E0817-3256-483D-8E97-01744EBC3F76";
    public static readonly string QueryPrefix = $"\"{QueryKey}\": \"$value ";
    public static readonly string QuerySuffix = "\"";
    // jsonTextStrBldr.Append($"\"{queryKey}\":").Append(Constants.QueryPrefix).Append(query).AppendLine("\"");
}