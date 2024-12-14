using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Startup
{
    static Startup()
    {
        if (Application.isBatchMode)
        {
            return;
        }
        if (EditorApplication.isPlaying)
        {
            return;
        }
        EditorApplication.delayCall += SetupWizard.Open;
    }
}
