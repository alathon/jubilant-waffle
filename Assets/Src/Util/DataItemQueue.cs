using System.Collections.Generic;

/**
 * A DataItem is basically an ID tied to an image file path.
 */
public class DataItem
{
    public readonly int id;
    public readonly string imagePath;

    public DataItem(int id, string imagePath) {
        this.id = id;
        this.imagePath = imagePath;
    }
}

/** The new format is simply the image path as the only
 * entry. */
public class NewFormatDataItemList : DataItemList
{
    protected override DataItem newDataItem(int id, string line)
    {
        return new DataItem(id, line);
    }
}

/** The old format is based on the .CSV file format Yvonne delivered,
 * where column 40 is the image path. */
public class OldFormatDataItemList : DataItemList
{
    protected override DataItem newDataItem(int id, string line)
    {
        string[] parts = line.Split(',');
        string image = parts[39];
        return new DataItem(id, image);
    }
}

/**
 * A DataItemList is just a wrapper around some number of items,
 * that we can load from a file; either based on the old format or
 * the new format. Use by calling the static methods at the bottom.
 */
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
