namespace JsonQL.Utilities;

public interface IJsonSerializerParameters
{
    bool Minify { get; }

    /// <summary>
    /// Indention from parent. Ignored if <see cref="Minify"/> is true.
    /// </summary>
    string IndentationFromParent { get; }

    /// <summary>
    /// Indention applied to every line. Ignored if <see cref="Minify"/> is true.
    /// </summary>
    string NewLineIndentation { get; }
}

public class JsonSerializerParameters : IJsonSerializerParameters
{
    public bool Minify { get; set; } = false;
    public string IndentationFromParent { get; set; } = "  ";
    public string NewLineIndentation { get; set; } = string.Empty;
}