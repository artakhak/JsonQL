namespace JsonQL.JsonObjects;

/// <inheritdoc />
public class JsonLineInfo : IJsonLineInfo

{
    
    public JsonLineInfo(int lineNumber, int linePosition)
    {
        LineNumber = lineNumber;
        LinePosition = linePosition;
    }

    /// <inheritdoc />
    public int LineNumber { get; }
    
    /// <inheritdoc />
    public int LinePosition { get; }
    
    /// <inheritdoc />
    public override string ToString()
    {
        return $"({this.LineNumber},{this.LinePosition}), {base.ToString()}";
    }
}