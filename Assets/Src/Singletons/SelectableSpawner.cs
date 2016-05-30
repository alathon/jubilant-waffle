using UnityEngine;
using System;

/**
 * Singleton for spawning items. You can set the prefab to use,
 * the default location for spawned items to appear at,
 * and the data folder path
 */
public class SelectableSpawner : MonoBehaviour {
    public class SpawnEvent : LogEvent
    {
        public readonly string imageName;
        public readonly int id;

        public SpawnEvent(int id, string imageName) : base()
        {
            this.id = id;
            this.imageName = imageName;
        }
        
        public static LogEvent FromParts(string[] parts)
        {
            var id = int.Parse(parts[3]);
            var path = parts[4];
            return new SpawnEvent(id, path);
        }

        public override string ToString()
        {
            return string.Format("Spawn item {0} {1}", id, imageName);
        }
    }

    public string dataFolderPath = "Data\\";
    public static SelectableSpawner instance = null;
    public GameObject prefab;
    public Vector3 defaultLocation;
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
    
    public static GameObject Spawn(int id, string imageName, bool log = true)
    {
        var filePath = Application.dataPath + "\\" + instance.dataFolderPath + imageName;
        if (System.IO.File.Exists(filePath))
        {
            GameObject spawned = (GameObject)Instantiate(instance.prefab, instance.defaultLocation, Quaternion.identity);
            spawned.name = id + "";

            // Create material for image.
            Material newMat = new Material(Shader.Find("Standard"));
            var bytes = System.IO.File.ReadAllBytes(filePath);
            var tex = new Texture2D(1, 1);
            tex.LoadImage(bytes);
            newMat.mainTexture = tex;
            spawned.GetComponentInChildren<MeshRenderer>().material = newMat;

            // Move to proper location.
            spawned.transform.SetParent(instance.spawnParent.transform);

            if(log) Logging.Log(new SpawnEvent(id, imageName));
            return spawned;
        }
        else throw new Exception("No image at " + filePath);
    }
}
