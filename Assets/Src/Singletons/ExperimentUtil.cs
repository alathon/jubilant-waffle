using UnityEngine;
using System.Collections;
//using UnityEditor;
using System;

public class ExperimentUtil : MonoBehaviour {
    public static ExperimentUtil instance = null;

    public string catalogPath;
    private DataItemList dataItems;
    private int dataItemCursor = 0;

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

    public void LoadCatalog()
    {
        if (catalogPath.Length != 0)
        {
            try
            {
                var filePath = Application.dataPath + "\\" + catalogPath;
                DataItemList q = DataItemList.GetDataItemQueue(filePath);
                this.dataItems = q;
                Debug.Log("Loaded catalog at " + filePath);
                // TODO: Reset any existing state.
            }
            catch (Exception ex)
            {
                Debug.Log("Exception while loading data item queue: ");
                Debug.Log(ex);
            }
        }
    }

    // TODO: Save a copy of current log.
    public void SaveExperiment()
    {

    }
}
