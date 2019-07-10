#if (UNITY_EDITOR)

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ShellMaterial : MonoBehaviour
{
    public Material baseMaterial;
    public Material bakedMaterial;

    private Boolean showBaked = false;

    public Boolean toggle()
    {
        showBaked = !showBaked;
        var renderer = GetComponentInParent<Renderer>();
        if (showBaked)
        {
            renderer.material = bakedMaterial;
        }
        else
        {
            renderer.material = baseMaterial;
        }
        return showBaked;
    }

    public Boolean toggle(Boolean forceBaked)
    {
        var renderer = GetComponentInParent<Renderer>();

        if (forceBaked)
        {
            showBaked = true;
            renderer.material = bakedMaterial;
        }

        return showBaked;
    }


    public Boolean getStatus()
    {
        return showBaked;
    }
}


[CustomEditor(typeof(ShellMaterial))]
public class ShellMaterialEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ShellMaterial myScript = (ShellMaterial)target;
        if (GUILayout.Button("Toggle"))
        {
            myScript.toggle();
        }
    }
}

#endif
