#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(OctaneBaker))]
public class OctaneBakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        OctaneBaker myScript = (OctaneBaker)target;

        if (GUILayout.Button("Setup"))
        {
            myScript.Setup();
        }

        if (GUILayout.Button("Generate PBR Targets"))
        {
            myScript.GenerateTargets();
        }

        if (GUILayout.Button("Create materials"))
        {
            MaterialController.createMaterials();
        }

        if (GUILayout.Button("Apply Materials"))
        {
            MaterialController.applyMaterials();
        }

        if (GUILayout.Button("Toggle Materials"))
        {
            MaterialController.toggleMaterials();
        }

        /*
        if (GUILayout.Button("Bake empty"))
        {
            myScript.Bake(false);
        }
  
        if (GUILayout.Button("Bake all (overwrite)"))
        {
            myScript.Bake(true);
        }
        */
    }
}
#endif
