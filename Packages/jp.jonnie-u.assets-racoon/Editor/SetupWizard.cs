using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SetupWizard : EditorWindow
{
    private string selectedFBXPath = "";
    private string selectedTexturePath = "";

    [MenuItem("Tools/Setup Wizard")]
    public static void Open()
    {
        var window = GetWindow<SetupWizard>("Assets Racoon");
        window.minSize = new Vector2(400, 300);
        window.position = new Rect(
            (Screen.currentResolution.width - 400) / 2,
            (Screen.currentResolution.height - 300) / 2,
            400,
            300
        );
    }

    private void OnGUI()
    {
        // タイトル（左揃え）
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 18,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(10, 0, 10, 10), // 上下左右の余白
        };
        GUILayout.Label("Assets Racoon", titleStyle);

        GUILayout.Space(12);

        // FBXファイル選択セクション
        DrawFileSelector("FBX", ref selectedFBXPath, "fbx");

        GUILayout.Space(10);

        // テクスチャフォルダ選択セクション
        DrawFolderSelector("Textures", ref selectedTexturePath);

        GUILayout.Space(20);

        // ボタンエリア
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear", GUILayout.Height(26), GUILayout.Width(80)))
        {
            selectedFBXPath = "";
            selectedTexturePath = "";
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button(Constants.ButtonCancel, GUILayout.Height(26), GUILayout.Width(80)))
        {
            Close();
        }

        if (GUILayout.Button(Constants.ButtonNext, GUILayout.Height(26), GUILayout.Width(80)))
        {
            if (string.IsNullOrEmpty(selectedFBXPath) || string.IsNullOrEmpty(selectedTexturePath))
            {
                EditorUtility.DisplayDialog("Error", Constants.ErrorNoFBXPath, "OK");
            }
            else
            {
                // Process Textures
                TextureProcessor.Process(selectedTexturePath);

                FBXProcessor.Process(selectedFBXPath);
                Close();
            }
        }
        GUILayout.EndHorizontal();
    }

    private void DrawFileSelector(string label, ref string path, string extension)
    {
        GUILayout.BeginHorizontal();

        GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft,
            fixedWidth = 60, // ラベルの固定幅
            fixedHeight = 26,
        };

        // ラベル
        GUILayout.Label(label, labelStyle);

        // 「Choose」ボタン
        if (GUILayout.Button("Choose", GUILayout.Height(26), GUILayout.Width(80)))
        {
            string selectedPath = EditorUtility.OpenFilePanel(
                $"Select {label} File",
                "",
                extension
            );
            if (!string.IsNullOrEmpty(selectedPath))
            {
                path = selectedPath;
            }
        }

        // 選択されたパスの表示 (helpBox サイズ調整)
        GUIStyle helpBoxStyle = new GUIStyle(EditorStyles.helpBox)
        {
            fontSize = 12, // テキストサイズを調整
            alignment = TextAnchor.MiddleLeft,
        };
        GUILayout.Label(
            !string.IsNullOrEmpty(path) ? path : "No file selected",
            helpBoxStyle,
            GUILayout.Height(26), // 高さ調整
            GUILayout.ExpandWidth(true)
        // GUILayout.Width(180) // 幅を適宜調整
        );

        GUILayout.EndHorizontal();
    }

    private void DrawFolderSelector(string label, ref string path)
    {
        GUILayout.BeginHorizontal();

        GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft,
            fixedWidth = 60, // ラベルの固定幅
            fixedHeight = 26,
        };

        // ラベル
        GUILayout.Label(label, labelStyle); // ラベルの幅を調整

        // 「Choose」ボタン
        if (GUILayout.Button("Choose", GUILayout.Height(26), GUILayout.Width(80)))
        {
            string selectedPath = EditorUtility.OpenFolderPanel($"Select {label} Folder", "", "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                path = selectedPath;
            }
        }

        // 選択されたフォルダのパスの表示 (helpBox サイズ調整)
        GUIStyle helpBoxStyle = new GUIStyle(EditorStyles.helpBox)
        {
            fontSize = 12, // テキストサイズを調整
            alignment = TextAnchor.MiddleLeft,
        };
        GUILayout.Label(
            !string.IsNullOrEmpty(path) ? path : "No folder selected",
            helpBoxStyle,
            GUILayout.Height(26), // 高さ調整
            GUILayout.ExpandWidth(true)
        // GUILayout.Width(180), // 幅を適宜調整
        );

        GUILayout.EndHorizontal();
    }
}
