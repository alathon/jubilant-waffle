using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System;
using System.Collections.Generic;

public class Logging : MonoBehaviour {
    private static Logging instance;

    public class LogEvent
    {
        public readonly object Source;
        public readonly double Time;

        public LogEvent(object source, double time)
        {
            this.Source = source;
            this.Time = time;
        }
    }

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
    }

    public string filePath;
    private LinkedList<LogEvent> events = new LinkedList<LogEvent>();

    private static Mutex mut = new Mutex();
    private StringBuilder sb = new StringBuilder();
    private int counter = 0;

    public LinkedList<LogEvent> Events
    {
        get
        {
            return events;
        }
    }

    void OnApplicationQuit()
    {
        this.flush();
    }

    void FixedUpdate()
    {
        counter += 1;
        if(counter % 5000 == 0)
        {
            this.flush();
        }
    }

    private void flush()
    {
        mut.WaitOne();
        using (StreamWriter w = new StreamWriter(filePath))
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
        Logging.instance.sb.Append(string.Format("[{0}]",ms)).Append(data);
        mut.ReleaseMutex();
    }

    public static void Log(object data)
    {
        double ms = DateTime.UtcNow.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        mut.WaitOne();
        Logging.instance.sb.Append(string.Format("[{0}]", ms)).Append(data);
        Logging.instance.events.AddLast(new LogEvent(data, ms));
        mut.ReleaseMutex();
    }
}