using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

/// <inheritdoc />
public class JsonArrayIndexesPathElement : IJsonArrayIndexesPathElement
{
    /// <summary>
    /// Constructor. 
    /// </summary>
    /// <param name="arrayIndexes">
    /// Array indexes.<br/>
    /// </param>
    /// <exception cref="ArgumentException">
    /// Throws this exception if <param name="arrayIndexes"></param> is empty.
    /// </exception>
    public JsonArrayIndexesPathElement(IReadOnlyList<IJsonArrayIndexInfo> arrayIndexes)
    {
        ArrayIndexes = arrayIndexes;

        if (ArrayIndexes.Count == 0)
            throw new ArgumentException($"List [{nameof(arrayIndexes)}] cannot be empty.", nameof(arrayIndexes));
       
        ArrayIndexes = arrayIndexes;
    }

    /// <inheritdoc />
    public IReadOnlyList<IJsonArrayIndexInfo> ArrayIndexes { get; }
   
    /// <inheritdoc />
    public IJsonLineInfo? LineInfo => ArrayIndexes[0].LineInfo;

    /// <inheritdoc />
    public override string ToString()
    {
        return $"[{string.Join(',', ArrayIndexes.Select(x => x.ToString()??"null"))}]";
    }
}