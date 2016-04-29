using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DataItem
{
    public readonly float x;
    public readonly float y;
    public readonly string imagePath;

    public DataItem(float x, float y, string imagePath) {
        this.x = x;
        this.y = y;
        this.imagePath = imagePath;
    }
}

public class NewFormatDataItemQueue : DataItemQueue
{
    protected override DataItem newDataItem(string line)
    {
        string[] parts = line.Split(',');
        string filename = parts[0];
        float x = 0f;
        float y = 0f;
        if(parts.Length >= 3)
        {
            x = float.Parse(parts[1]);
            y = float.Parse(parts[2]);
        }
        return new DataItem(x, y, filename);
    }
}
public class OldFormatDataItemQueue : DataItemQueue
{
    protected override DataItem newDataItem(string line)
    {
        string[] parts = line.Split(',');
        float x = float.Parse(parts[9]);
        float y = float.Parse(parts[12]);
        //string filename = parts[36]; // NOT USED
        //string id = parts[37]; // NOT USED
        string image = parts[39];
        return new DataItem(x, y, image);
    }
}

abstract public class DataItemQueue {
    protected Queue<DataItem> items;

    public Queue<DataItem> Items { get { return items;  } }

    protected abstract DataItem newDataItem(string line);

    private Queue<DataItem> loadFileContents(string filePath)
    {
        Queue<DataItem> lst = new Queue<DataItem>();
        string[] lines = System.IO.File.ReadAllLines(filePath);
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            var item = newDataItem(line);
            if (item != null)
            {
                lst.Enqueue(item);
            }
        }
        return lst;
    }

    public static DataItemQueue GetOldDataItemQueue(string filePath)
    {
        DataItemQueue q = new OldFormatDataItemQueue();
        q.items = q.loadFileContents(filePath);
        return q;
    }

    public static DataItemQueue GetDataItemQueue(string filePath)
    {
        DataItemQueue q = new NewFormatDataItemQueue();
        q.items = q.loadFileContents(filePath);
        return q;
    }
}
