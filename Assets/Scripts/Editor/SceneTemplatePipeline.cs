using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.SceneTemplate;

/*
    A scene template pipeline class.
    Automatically deletes unnecessary clone folder data that is generated when creating a new scene from a template.
*/
[assembly: UnityEngine.AssemblyIsEditorAssembly]
public class SceneTemplatePipeline : ISceneTemplatePipeline {

    // Tell Unity that scene template is valid for instantiation.
    public virtual bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset) { return true; }

    // Called before template is instantiated.
    // Not needed for this template, but is a required interface member.
    public virtual void BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive, string sceneName) { }

    // Called after template is instantiated.
    public virtual void AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, Scene scene, bool isAdditive, string sceneName) {
        string cloneFolderPath = scene.path.Replace(".unity", ""); // Locate clone folder path.

        try {
            Directory.Delete(cloneFolderPath, true);    // Delete clone folder and contents.
            File.Delete(cloneFolderPath + ".meta");     // Delete clone folder meta file.
        }
        catch (IOException e) {

            // Display warning message in debug console if deletion fails.
            const string message = "Exception thrown when attempting to delete unneeded dependency assets after creating scene. " +
                "If there is a dependency folder associated with the new scene, you may safely delete it.";

            UnityEngine.Debug.LogWarning(message + "\n" + e.Message);
        }

        UnityEditor.AssetDatabase.Refresh(); // Update asset database to show changes.
    }
}