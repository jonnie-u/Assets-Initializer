using System.IO;

public static class DirectoryUtility
{
    public static void CreateIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
