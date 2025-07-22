using System.Text;

namespace JsonQL.Diagnostics;

public static class ResourceFileHelpers
{
    /// <summary>
    /// Saves the file <paramref name="fileContents"/> in program output folder under the name <paramref name="fileName"/>
    /// </summary>
    /// <param name="fileContents"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static async Task SaveAsync(string fileContents, string fileName)
    {
        await using var generatedJsonFile = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        var jsonByteArray = Encoding.ASCII.GetBytes(fileContents);

        await generatedJsonFile.WriteAsync(jsonByteArray);
    }
}
