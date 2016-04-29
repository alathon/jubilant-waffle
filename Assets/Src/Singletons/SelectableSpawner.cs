using UnityEngine;
using System.Collections;
using System;

public class SelectableSpawner : MonoBehaviour {
    public static SelectableSpawner instance = null;
    public string dataFolderPath = "\\Data\\";

    private GameObject spawnParent;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        instance.spawnParent = GameObject.Find("Environment/Objects");
    }

    public GameObject prefab;

    public Vector3 defaultLocation;

    public static GameObject Spawn(string imageName)
    {
        var filePath = Application.dataPath + instance.dataFolderPath + imageName;
        if (System.IO.File.Exists(filePath))
        {
            GameObject spawned = (GameObject)Instantiate(instance.prefab, instance.defaultLocation, Quaternion.identity);
            spawned.name = imageName;

            // Create material for image.
            Material newMat = new Material(Shader.Find("Standard"));
            var bytes = System.IO.File.ReadAllBytes(filePath);
            var tex = new Texture2D(1, 1);
            tex.LoadImage(bytes);
            newMat.mainTexture = tex;
            spawned.GetComponentInChildren<MeshRenderer>().material = newMat;

            // Move to proper location.
            spawned.transform.SetParent(instance.spawnParent.transform);

            Logging.LogText(String.Format("Spawned {0}", spawned.name));
            return spawned;
        }
        else throw new Exception("No image at " + filePath);
    }
}
