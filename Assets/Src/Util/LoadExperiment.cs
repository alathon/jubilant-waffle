using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class LoadExperiment {
    
    public static IEnumerator LoadExperimentByPath(string path, bool log)
    {
        // Step: Reload scene
        SceneManager.LoadScene("DefaultScene", LoadSceneMode.Single);

        // Delay, to let the scene load.
        yield return new WaitForSeconds(1f);

        // Step: Read experiment config file.
        string[] lines = File.ReadAllLines(path);

        Dictionary<string, string> expValues = new Dictionary<string, string>();
        foreach (var line in lines)
        {
            var parts = line.Split('=');
            expValues.Add(parts[0].Trim().ToLower(), parts[1].Trim());
        }
        
        // Step: Set movement and camera mode, and load the
        // catalog file.
        var movement = expValues["movement"];
        var camera = expValues["camera"];
        var catalog = expValues["catalog"];

        ExperimentUtil.instance.SetMovementMethod(movement);
        ExperimentUtil.instance.SetCameraMethod(camera);
        ExperimentUtil.instance.LoadCatalog(catalog);

        Debug.Log("Loaded experiment " + path);
        Debug.Log("Movement: " + movement);
        Debug.Log("Camera: " + camera);
        Debug.Log("Catalog: " + catalog);

        if (log)
        {
            Logging.LogText("Loaded experiment " + path);
            Logging.LogText("Movement: " + movement);
            Logging.LogText("Camera: " + camera);
            Logging.LogText("Catalog: " + catalog);
        }
    }
}
