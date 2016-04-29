using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class ExperimentUtil : MonoBehaviour {
    public static ExperimentUtil instance = null;

    private DataItemQueue queue;
    private string curExperimentPath;

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
    }
    
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.L))
        {
            this.LoadExperiment();
        } else if(Input.GetKeyDown(KeyCode.S))
        {
            this.SaveExperiment();
        } else if(Input.GetKeyDown(KeyCode.A))
        {
            this.PopFromQueue();
        }
	}

    private void PopFromQueue()
    {
        DataItem item = this.queue.Items.Dequeue();
        GameObject spawned = SelectableSpawner.Spawn(item.imagePath);
        spawned.transform.Translate(new Vector3(item.x, 0f, item.y));
    }

    private void LoadExperiment()
    {
        var path = EditorUtility.OpenFilePanel(
                    "Load experiment file",
                    "",
                    "exp");
        if (path.Length != 0)
        {
            try
            {
                DataItemQueue q = DataItemQueue.GetDataItemQueue(path);
                this.queue = q;
                this.curExperimentPath = path;
            } catch (Exception ex)
            {
                Debug.Log("Exception while loading data item queue: ");
                Debug.Log(ex);
            }
            
            
        }
    }

    private void SaveExperiment()
    {
        if(this.curExperimentPath == null)
        {
            throw new Exception("You must first load an experiment.");
        }

        try
        {
            var lines = System.IO.File.ReadAllLines(this.curExperimentPath);
            for(int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Split(',');
                GameObject gObj = GameObject.Find(line[0]);
                if(gObj != null)
                {
                    var x = gObj.transform.localPosition.x.ToString();
                    var y = gObj.transform.localPosition.z.ToString();
                    lines[i] = String.Format("{0},{1},{2}", line[0], x, y);
                }
            }
            System.IO.File.WriteAllLines(this.curExperimentPath, lines);
        } catch (Exception ex)
        {
            Debug.Log("Encountered exception while trying to read or write to " + this.curExperimentPath);
            // Rethrow exception.
            throw ex;
        }
    }
}
