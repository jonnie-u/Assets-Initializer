using System.IO;
using UnityEditor;
using UnityEngine;

public static class FBXProcessor
{
    /// <summary>
    /// 指定されたファイルをAsettsにコピーしVRChat向けの設定を行う
    /// </summary>
    /// <param name="sourcePath">FBXファイルのパス</param>
    public static void Process(string fbxPath)
    {
        string destinationDirectory = Constants.FBXFolder;
        string materialsDirectory = Constants.MaterialsFolder;

        DirectoryUtility.CreateIfNotExists(destinationDirectory);
        DirectoryUtility.CreateIfNotExists(materialsDirectory);

        string fileName = Path.GetFileName(fbxPath);
        string destinationPath = Path.Combine(destinationDirectory, fileName);

        if (File.Exists(destinationPath))
        {
            Debug.LogWarning($"同じ名前のFBXが既に存在します: {destinationPath}");
        }
        else
        {
            File.Copy(fbxPath, destinationPath, true);
        }

        AssetDatabase.Refresh();
        ConfigureFBXImporter(destinationPath);
        ExtractMaterials(destinationPath, materialsDirectory);
        Debug.Log("FBXとマテリアルの処理が完了しました。");
    }

    private static void ConfigureFBXImporter(string assetPath)
    {
        ModelImporter importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
        if (importer != null)
        {
            // カメラをインポートしない
            importer.importCameras = false;
            // ライトをインポートしない
            importer.importLights = false;
            // メッシュの読み取りを許可する
            importer.isReadable = true;
            // Animation TypeをHumanoidにする
            importer.animationType = ModelImporterAnimationType.Human;
            // Blend Shape NormalをNoneにする
            importer.importBlendShapeNormals = ModelImporterNormals.None;

            importer.SaveAndReimport();
            Debug.Log($"FBXのインポート設定を変更しました: {assetPath}");
        }
        else
        {
            Debug.LogError($"ModelImporterが取得できませんでした: {assetPath}");
        }
    }

    private static void ExtractMaterials(string assetPath, string destinationPath)
    {
        foreach (Object material in AssetDatabase.LoadAllAssetsAtPath(assetPath))
        {
            if (material is Material mat)
            {
                string path = Path.Combine(destinationPath, mat.name) + ".mat";
                path = AssetDatabase.GenerateUniqueAssetPath(path);
                string result = AssetDatabase.ExtractAsset(material, path);

                if (!string.IsNullOrEmpty(result))
                {
                    Debug.LogError($"マテリアルの抽出に失敗しました: {result}");
                }
            }
        }

        AssetDatabase.Refresh();
        Debug.Log($"マテリアルの抽出が完了しました: {destinationPath}");
    }
}
