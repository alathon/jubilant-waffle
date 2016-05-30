using UnityEngine;
using System;

/**
 * A utility class for a number of different purposes,
 * including keeping track of which data item the experiment
 * is currently on.
 * 
 * TODO: The data item state should be moved out of this class
 * for modularity/simplicity. Not strictly
 * necessary, just cleaner.
 */
public class ExperimentUtil : MonoBehaviour {
    public static ExperimentUtil instance = null;

    public string catalogPath;
    private DataItemList dataItems;
    private int dataItemCursor = 0;

    // File loading.
    private GUIStyle style;
    private bool experimentWindowOpen;
    private string path = "";
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            this.experimentWindowOpen = false;
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

    public void DetailView(GameObject selectable)
    {
        GameObject selected = selectable == null ? SelectionSingleton.Selected : selectable;

        if (selected != null)
        {
            selected.GetComponent<DetailViewMode>().GrowOrShrink();
        }
    }
    
    internal void StartNewExperiment()
    {
        if (this.experimentWindowOpen) return;

        FileSelector.GetFile((status, path) =>
        {
            this.experimentWindowOpen = false;
            if (status == FileSelector.Status.Successful)
            {
                StartCoroutine(LoadExperiment.LoadExperimentByPath(path, true));
            }
        }, ".exp");
        this.experimentWindowOpen = true;
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
        var movement = player.GetComponent<Movement>();
        if (method.Equals("mouse"))
        {
            movement.movementType = Movement.MovementType.MOUSE;
        } else if(method.Equals("rigidbody"))
        {
            movement.movementType = Movement.MovementType.RIGIDBODY;
        }
    }

    // If both are enabled/disabled, enable mouse movement as only one
    // Otherwise, toggle both (i.e. disable one, enable other).
    internal void ToggleMovementMethod()
    {
        Debug.Log("ToggleMovementMethod");
        var player = GameObject.Find("Player");
        var movement= player.GetComponent<Movement>();

        if(movement.movementType == Movement.MovementType.MOUSE)
        {
            movement.movementType = Movement.MovementType.RIGIDBODY;
        } else
        {
            movement.movementType = Movement.MovementType.MOUSE;
        }
    }

    private DataItem GetCurrentItem()
    {
        return this.dataItems.Items[this.dataItemCursor];
    }

    public GameObject SpawnNext()
    {
        if (this.dataItems == null)
        {
            return null;
        }

        if (this.dataItemCursor >= this.dataItems.Items.Count)
        {
            Debug.Log("At the end of catalog. Cannot spawn more items.");
            return null;
        }

        var item = SelectableSpawner.Spawn(this.GetCurrentItem().id, this.GetCurrentItem().imagePath);
        Debug.Log("Spawned " + item);

        this.dataItemCursor++;
        this.DetailView(item);
        return item;
    }
    
    public void LoadExperimentFromLog()
    {
        FileSelector.GetFile((status, path) =>
        {
            this.experimentWindowOpen = false;
            if (status == FileSelector.Status.Successful)
            {
                RestoreFromLog.ByPath(path, (numItems) =>
                {
                    this.dataItemCursor = numItems;
                });
            }
        }, ".log");
        this.experimentWindowOpen = true;
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
