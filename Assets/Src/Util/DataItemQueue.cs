using System.Collections.Generic;

public class DataItem
{
    public readonly int id;
    public readonly string imagePath;

    public DataItem(int id, string imagePath) {
        this.id = id;
        this.imagePath = imagePath;
    }
}

public class NewFormatDataItemList : DataItemList
{
    protected override DataItem newDataItem(int id, string line)
    {
        return new DataItem(id, line);
    }
}

public class OldFormatDataItemList : DataItemList
{
    protected override DataItem newDataItem(int id, string line)
    {
        string[] parts = line.Split(',');
        string image = parts[39];
        return new DataItem(id, image);
    }
}

abstract public class DataItemList {
    protected List<DataItem> items;

    public List<DataItem> Items { get { return items;  } }
    
    protected abstract DataItem newDataItem(int id, string line);

    private List<DataItem> loadFileContents(string filePath)
    {
        List<DataItem> lst = new List<DataItem>();
        string[] lines = System.IO.File.ReadAllLines(filePath);
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            var item = newDataItem(i, line);
            if (item != null)
            {
                lst.Add(item);
            }
        }
        return lst;
    }

    public static DataItemList GetOldDataItemQueue(string filePath)
    {
        DataItemList q = new OldFormatDataItemList();
        q.items = q.loadFileContents(filePath);
        return q;
    }

    public static DataItemList GetDataItemQueue(string filePath)
    {
        DataItemList q = new NewFormatDataItemList();
        q.items = q.loadFileContents(filePath);
        return q;
    }
}
