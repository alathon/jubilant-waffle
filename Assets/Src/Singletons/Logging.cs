using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System;
using System.Collections.Generic;

abstract public class LogEvent
{
    public readonly double Time;

    public LogEvent()
    {
        double ms = DateTime.UtcNow.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        this.Time = ms;
    }
}

public class Logging : MonoBehaviour {
    private static Logging instance;
    public string filePath;
    private LinkedList<LogEvent> events = new LinkedList<LogEvent>();
    private static Mutex mut = new Mutex();
    private StringBuilder sb = new StringBuilder();
    private int counter = 0;
    public static LinkedList<LogEvent> Events { get { return instance.events; } }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        // Clear current log.
        File.Delete(Application.dataPath + "/" + Logging.instance.filePath);
    }

    void OnApplicationQuit()
    {
        this.flush();
    }

    void FixedUpdate()
    {
        counter += 1;
        if(counter % 1000 == 0)
        {
            this.flush();
        }
    }
    

    private void flush()
    {
        mut.WaitOne();
        using (StreamWriter w = new StreamWriter(Application.dataPath + "/" + filePath, true))
        {
            w.Write(sb);
            sb.Length = 0;
        }
        mut.ReleaseMutex();
    }

    public static void LogText(string data)
    {
        double ms = DateTime.UtcNow.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        mut.WaitOne();
        var text = string.Format("[{0}] {1}\n", ms, data);
        Logging.instance.sb.Append(text);
        mut.ReleaseMutex();
    }

    public static void Log(LogEvent data)
    {
        mut.WaitOne();
        var text = string.Format("[{0}] {1}\n", data.Time, data);
        Logging.instance.sb.Append(text);
        Logging.instance.events.AddLast(data);
        mut.ReleaseMutex();
    }

    public static void CopyLogTo(string otherPath)
    {
        Logging.instance.flush();
        mut.WaitOne();
        File.Copy(Application.dataPath + "/" + Logging.instance.filePath, Application.dataPath + "/" + otherPath);
        mut.ReleaseMutex();
    }

    public static void LoadLog(string[] lines, LinkedList<LogEvent> events)
    {
        mut.WaitOne();
        // Clear current log.
        File.Delete(Application.dataPath + "/" + Logging.instance.filePath);
        // Append lines.
        Logging.instance.sb.Length = 0;
        foreach(var line in lines)
        {
            Logging.instance.sb.Append(line);
        }
        // Set events.
        Logging.instance.events = events;
        // Release hold.
        mut.ReleaseMutex();
    }
}