using System.IO;
using UnityEditor;
using UnityEngine;

public static class TextureProcessor
{
    /// <summary>
    /// 指定されたフォルダ内のすべてのファイルをAssetsにコピーする
    /// </summary>
    /// <param name="folderPath">テクスチャのフォルダパス</param>
    public static void Process(string folderPath)
    {
        if (!AssetDatabase.IsValidFolder(Constants.TexturesFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Textures");
        }

        string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
        string[] allowedExtensions = { ".png", ".jpg", ".jpeg", ".tga", ".psd" };

        foreach (string file in files)
        {
            string extension = Path.GetExtension(file).ToLower();
            if (System.Array.Exists(allowedExtensions, ext => ext == extension))
            {
                string fileName = Path.GetFileName(file);
                string destinationFile = Path.Combine(Constants.TexturesFolder, fileName)
                    .Replace("\\", "/");

                try
                {
                    File.Copy(file, destinationFile, overwrite: true);
                }
                catch (IOException ex)
                {
                    Debug.LogError($"Failed to copy {fileName}: {ex.Message}");
                }
            }
        }

        AssetDatabase.Refresh();
    }
}
