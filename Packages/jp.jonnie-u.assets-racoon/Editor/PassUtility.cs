using System.IO;

public static class PathUtility
{
    public static string NormalizePath(string path)
    {
        return Path.GetFullPath(path).Replace("\\", "/");
    }
}
