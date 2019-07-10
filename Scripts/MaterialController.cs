#if (UNITY_EDITOR)

using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System;


public class MaterialController {
    public TextAsset imageAsset;

    public static bool createMaterials()
    {
        DirectoryInfo dir = new DirectoryInfo(@"Assets/Baked Images/");
        foreach (FileInfo file in dir.GetFiles("*.png"))
        {
            string matPath = "Assets/Resources/Materials/" + file.Name.Split('.')[0] + ".mat";

            Material material = new Material(Shader.Find("Unlit/Texture"));
            AssetDatabase.CreateAsset(material, matPath);

            byte[] imageBytes = File.ReadAllBytes(@"Assets/Baked Images/" + file.Name);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);

            string texPath = "Assets/Resources/Textures/" + file.Name.Split('.')[0] + ".tex";
            AssetDatabase.CreateAsset(tex, texPath);

            material.mainTexture = tex;
            material.EnableKeyword("_DETAIL_MULX2");
            //material.SetTexture("_DetailAlbedoMap", tex);
            material.SetFloat("_Glossiness", 0); //"smoothness"
            AssetDatabase.SaveAssets();
        }

        return true;
    }

    public static bool applyMaterials()
    {
        DirectoryInfo dir = new DirectoryInfo(@"Assets/Resources/Materials/");
        foreach (FileInfo file in dir.GetFiles("*.mat"))
        {
            //GameObject obj = GameObject.Find(file.Name.Split('.')[0]);
            GameObject[] objs = FindGameObjectsWithName(file.Name.Split('.')[0]);

            foreach (GameObject obj in objs)
            {
                if (obj.transform.parent && obj.transform.parent.name == "PBR") continue;
                if (obj.name == "PBR Render Target") continue;

                ShellMaterial shellMaterial;
                shellMaterial = obj.GetComponent<ShellMaterial>();
                if (!shellMaterial)
                {
                    shellMaterial = obj.AddComponent<ShellMaterial>();
                }

                if (!shellMaterial.getStatus())
                {
                    Material candidateMaterial = obj.GetComponent<Renderer>().sharedMaterial;
                    // bakedMaterials have same name as the GameObject
                    // only update base material if candidate material doesn't look like a bakedMaterial
                   
                    if (candidateMaterial && candidateMaterial.name != obj.name)
                    {
                        shellMaterial.baseMaterial = candidateMaterial;
                    }
                }


                Material bakedMaterial = Resources.Load("Materials/" + file.Name.Split('.')[0], typeof(Material)) as Material;
                shellMaterial.bakedMaterial = bakedMaterial;
                shellMaterial.toggle(true);
            }
        }
        return true;
    }

    static GameObject[] FindGameObjectsWithName(string name)
    {
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        GameObject[] arr = new GameObject[gameObjects.Length];
        int FluentNumber = 0;
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].name == name)
            {
                arr[FluentNumber] = gameObjects[i];
                FluentNumber++;
            }
        }
        Array.Resize(ref arr, FluentNumber);
        return arr;
    }

    public static bool toggleMaterials()
    {
        ShellMaterial[] mats = GameObject.FindObjectsOfType<ShellMaterial>();
        foreach (ShellMaterial mat in mats)
        {
            mat.toggle();
        }
        return true;
    }
}
#endif
