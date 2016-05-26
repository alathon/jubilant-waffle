using UnityEngine;
using System.Collections;
//using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

public class ExperimentUtil : MonoBehaviour {
    public static ExperimentUtil instance = null;

    public string catalogPath;
    private DataItemList dataItems;
    private int dataItemCursor = 0;
    

    // File loading.
    private GUIStyle style;
    private bool windowOpen;
    private string path = "";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            this.windowOpen = false;
            this.style = new GUIStyle();
            this.style.fontSize = 40;
            this.style.normal.textColor = Color.white;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void DetailView()
    {
        GameObject selected = SelectionSingleton.Selected;
        if(selected != null)
        {
            // TODO: Detail view here.
        }
    }
    
    internal void LoadExperimentFile()
    {
        FileSelector.GetFile((status, path) =>
        {
            this.windowOpen = false;
            if (status == FileSelector.Status.Successful)
            {
                StartCoroutine(LoadExperiment.LoadExperimentByPath(path, true));
            }
        }, ".exp");
        this.windowOpen = true;
    }

    public void SetCameraMethod(string method)
    {
        var mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<FollowPlayer>().enabled = method.ToLower().Equals("topdown");
    }

    internal void ToggleTopDownView()
    {
        var mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<FollowPlayer>().enabled = !mainCamera.GetComponent<FollowPlayer>().enabled;
    }

    public void SetMovementMethod(string method)
    {
        Debug.Log("SetMovementMethod " + method);
        var player = GameObject.Find("Player");
        var rigidbodyMovement = player.GetComponent<RigidbodyMovement>();
        var mouseMovement = player.GetComponent<MouseMovement>();
        if (method.Equals("mouse"))
        {
            rigidbodyMovement.enabled = false;
            mouseMovement.enabled = true;
        } else if(method.Equals("rigidbody"))
        {
            rigidbodyMovement.enabled = true;
            mouseMovement.enabled = false;
        }
    }

    // If both are enabled/disabled, enable mouse movement as only one
    // Otherwise, toggle both (i.e. disable one, enable other).
    internal void ToggleMovementMethod()
    {
        Debug.Log("ToggleMovementMethod");
        var player = GameObject.Find("Player");
        var rigidbodyMovement = player.GetComponent<RigidbodyMovement>();
        var mouseMovement = player.GetComponent<MouseMovement>();

        if((mouseMovement.enabled && rigidbodyMovement.enabled) ||
            (mouseMovement.enabled == false && rigidbodyMovement.enabled == false))
        {
            rigidbodyMovement.enabled = false;
            mouseMovement.enabled = true;
        } else
        {
            rigidbodyMovement.enabled = !rigidbodyMovement.enabled;
            mouseMovement.enabled = !mouseMovement.enabled;
        }
    }

    private DataItem GetCurrentItem()
    {
        return this.dataItems.Items[this.dataItemCursor];
    }

    public void SpawnNext()
    {
        if (this.dataItems == null)
        {
            return;
        }

        if (this.dataItemCursor >= this.dataItems.Items.Count)
        {
            Debug.Log("At the end of catalog. Cannot spawn more items.");
            return;
        }

        var item = SelectableSpawner.Spawn(this.GetCurrentItem().id, this.GetCurrentItem().imagePath);
        Debug.Log("Spawned " + item);

        this.dataItemCursor++;
        // TODO: Activate detail view for item.
    }
    
    public void LoadStateFromLog()
    {
        FileSelector.GetFile((status, path) =>
        {
            this.windowOpen = false;
            if (status == FileSelector.Status.Successful)
            {
                RestoreFromLog.ByPath(path);
            }
        }, ".log");
        this.windowOpen = true;
    }
    
    public void LoadCatalog(string path)
    {
        var realPath = path == null ? catalogPath : path;
        if (realPath.Length != 0)
        {
            try
            {
                var filePath = Application.dataPath + "/" + catalogPath;
                DataItemList q = DataItemList.GetDataItemQueue(filePath);
                this.dataItems = q;
                Debug.Log("Loaded catalog at " + filePath);
            }
            catch (Exception ex)
            {
                Debug.Log("Exception while loading data item queue: ");
                Debug.Log(ex);
            }
        }
    }
    
    public void SaveExperiment()
    {
        var time = DateTime.UtcNow.ToUniversalTime();
        var timeStr = time.Year + "-" + time.Month + "-" + time.Day + "_" + time.Hour + "-" + time.Minute + "-" + time.Second;
        string otherPath = "Logs/" + timeStr + ".log";
        Debug.Log("Saving experiment log to " + otherPath);
        Logging.CopyLogTo(otherPath);
    }
}
