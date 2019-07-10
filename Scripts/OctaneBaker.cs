#if (UNITY_EDITOR)
using UnityEngine;
using System;
using System.Reflection;
using OctaneUnity;
using System.IO;

public class OctaneBaker : MonoBehaviour
{
    public GameObject PbrRenderTargetPrefab;
    private string PbrRenderDestinationName = "PBR";
    // string PROJECT_NAME = "Salvo";

    public UnityEngine.Object[] FindTargets()
    {
        Assembly design = Assembly.LoadFile(@"Assets/Plugins/Octane/Editor.dll");
        Type propertyType = design.GetType("OctaneUnity.PBRInstanceProperties");
        UnityEngine.Object[] list = GameObject.FindObjectsOfType(propertyType);
        return list;
    }


    private GameObject findObjectByName(string name)
    {
        UnityEngine.Object[] gameObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));

        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name == name) return gameObject;
        }
        return null;
    }

    public void Setup()
    {
        Directory.CreateDirectory(@"Assets/Baked Images/");
        Directory.CreateDirectory(@"Assets/Resources/Materials/");
        Directory.CreateDirectory(@"Assets/Resources/Textures/");

        var pbrTarget = findObjectByName("PBR Render Target");
        if (!pbrTarget)
        {
            pbrTarget = new GameObject();
            pbrTarget.AddComponent<PBRRenderTargetComponent>();
            pbrTarget.name = "PBR Render Target";
        }
        this.PbrRenderTargetPrefab = pbrTarget;
        pbrTarget.SetActive(false);
    }

    public void GenerateTargets()
    {
        UnityEngine.Object[] list = FindTargets();
        Debug.Log("Found " + list.Length + " targets.");

        for (int i = 1; i <= list.Length; i++)
        {
            GameObject obj = GameObject.Find(list[i - 1].name);
            CreatePBRTarget(obj, (i + 1));
        }

        Debug.Log("Done.");
    }

    /*
    private void ClearFile(string path) {
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("");
        writer.Close();
    }

    public void WriteCSV()
    {
        string path = "Assets/" + PROJECT_NAME + "/data/gameobjects.csv";
        ClearFile(path);
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine("name,id");

        UnityEngine.Object[] list = FindTargets();
        Debug.Log("Found " + list.Length + " targets.");
 
        for (int i = 1; i <= list.Length; i++)
        {
            GameObject obj = GameObject.Find(list[i-1].name);
            writer.WriteLine(list[i-1].name + "," + (i + 1));
            CreatePBRTarget(obj, (i + 1));
        }
 
        writer.Close();
        Debug.Log("Done.");
    }*/

    private void CreatePBRTarget(GameObject targetObject, int number)
    {

        string numberStr = number + "";
        if (number < 10)
        {
            numberStr = "000" + numberStr;
        } else if (number < 100)
        {
            numberStr = "00" + numberStr;
        } else if (number < 1000)
        {
            numberStr = "0" + numberStr;
        }

        string name = targetObject.name;

        GameObject obj = GameObject.Find(name);
        GameObject destination = GameObject.Find(PbrRenderDestinationName);

        if (destination == null)
        {
            destination = new GameObject(PbrRenderDestinationName);
        }


        PBRRenderTargetComponent pbrComponent = null;

        if (obj)
        {
            pbrComponent = obj.GetComponent<PBRRenderTargetComponent>();
        }

        if (pbrComponent == null)
        {
            //GameObject newPBRobject = new GameObject(name);
            GameObject newPBRobject = GameObject.Instantiate(PbrRenderTargetPrefab, targetObject.transform);
            newPBRobject.name = name;

            PBRRenderTargetComponent prefabComponent = PbrRenderTargetPrefab.GetComponent<PBRRenderTargetComponent>();
            pbrComponent = newPBRobject.GetComponent<PBRRenderTargetComponent>();

            if (UnityEditorInternal.ComponentUtility.CopyComponent(prefabComponent))
            {
                if (!UnityEditorInternal.ComponentUtility.PasteComponentValues(pbrComponent))
                {
                    Debug.Log("Problem settings component values on " + name);
                }
            } else
            {
                Debug.Log("Problem getting component values on " + name);
            }

            //pbrComponent.CameraMode = Octane.RenderTarget.CameraSelectionType.Custom;
            pbrComponent.transform.parent = destination.transform;

            PBRInstanceProperties PBRInstanceProps = targetObject.GetComponent<PBRInstanceProperties>();
            PBRInstanceProps.BakingGroupID = number;

            pbrComponent.CreateNewRenderTarget();
            var RT = pbrComponent.RenderTarget;
            //Create a new baking camera node for the rendertarget
            RT.CreateInternal(Octane.PinId.P_CAMERA, Octane.NodeType.NT_CAM_BAKING);
            RT.Camera.SetPinInt(Octane.PinId.P_BAKING_GROUP_ID, number);

            int[] resolution = getResolutionFromName(name);
            RT.FilmSettings.SetPinInt2(Octane.PinId.P_RESOLUTION, resolution[0], resolution[1]);
            //OctaneUnity.Scene.Instance.RenderTarget = RT;
            pbrComponent.Save();
        }
        else
        {
            Debug.Log("PBRRenderTarget seems to exist for " + targetObject.name + ". (Probably identically named GameObjects)");
        }
    }

    private int[] getResolutionFromName(string name)
    {
        // 0 -> width, 1 -> height
        int[] resolution = { 2048, 2048 };

        var hyphenated = name.Split('-');
        if (hyphenated.Length < 1)
        {
            return resolution;
        }
        var resolutionSufix = hyphenated[hyphenated.Length - 1]; //last hyphen should be resolution

        switch (resolutionSufix)
        {
            case ".25k":
                resolution[0] = 256;
                resolution[1] = 256;
                break;
            case ".5k":
                resolution[0] = 512;
                resolution[1] = 512;
                break;
            case "1k":
                resolution[0] = 1024;
                resolution[1] = 1024;
                break;
            case "2k":
                resolution[0] = 2048;
                resolution[1] = 2048;
                break;
            case "4k":
                resolution[0] = 4096;
                resolution[1] = 4096;
                break;
            case "8k":
                resolution[0] = 8192;
                resolution[1] = 8192;
                break;
            case "16k":
                resolution[0] = 16384;
                resolution[1] = 16384;
                break;
        }
        return resolution;
    }
}
#endif
