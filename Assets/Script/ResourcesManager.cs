using System.IO;
using System.Text.RegularExpressions;
using System;

public class ResourcesManager
{
    static public void WriteToJsonFile(string JsonFileName, string jsonString)
    {
        StreamWriter writer = new StreamWriter(JsonFileName, false);

        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        var ss = reg.Replace(jsonString, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });

        writer.WriteLine(ss);

        writer.Flush();

        writer.Close();
    }
}