namespace JsonQL.Utilities;

public interface IJsonSerializerParameters
{
    /// <summary>
    /// Indicates whether the serialized output should be minified.
    /// If set to true, formatting options such as <see cref="IndentationFromParent"/> and <see cref="NewLineIndentation"/> are ignored.
    /// </summary>
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

/// <inheritdoc />
public class JsonSerializerParameters : IJsonSerializerParameters
{
    /// <inheritdoc />
    public bool Minify { get; set; }
    
    /// <inheritdoc />
    public string IndentationFromParent { get; set; } = "  ";
    
    /// <inheritdoc />
    public string NewLineIndentation { get; set; } = string.Empty;
}