using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

public static class RestoreFromLog {

    private static LinkedList<LogEvent> RecreateEvents(string[] lines)
    {
        LinkedList<LogEvent> lst = new LinkedList<LogEvent>();
        foreach (var line in lines)
        {
            var timeStart = line.IndexOf('[');
            var timeEnd = line.IndexOf(']');
            var time = double.Parse(line.Substring(timeStart+1, timeEnd-1));
            var eventText = line.Substring(timeEnd + 1);
            Debug.Log("Event: " + eventText);
            var parts = eventText.Split(' ');
            LogEvent eventObj = null;
            switch (parts[1])
            {
                case "Spawn":
                    eventObj = SelectableSpawner.SpawnEvent.FromParts(parts);
                    break;
                case "MoveSelectable":
                    eventObj = MoveSelectable.MoveSelectableEvent.FromParts(parts);
                    break;
                default:
                    Debug.Log("Ignoring " + eventText);
                    break;
            }
            if (eventObj != null)
            {
                lst.AddLast(eventObj);
            }
        }
        return lst;
    }

    private static string GetExperimentPath(string[] lines)
    {
        foreach(var line in lines)
        {
            if (!line.Contains("Loaded experiment")) continue;

            var endOfTime = lines[0].IndexOf(']');
            var offset = " Loaded experiment ".Length;
            return lines[0].Substring(endOfTime + offset);
        }
        return null;
    }

    public static void ByPath(string path)
    {
        // Step: Load log file at path.
        Debug.Log("Loading log file: " + path);
        string[] lines = File.ReadAllLines(path);

        // Step: Grab experiment path
        
        string experimentPath = GetExperimentPath(lines);
        
        
        if (experimentPath == null)
        {
            Debug.Log("ERROR: No experiment was loaded in the log file. Aborting.");
            return;
        }

        // Step: Load experiment.
        Debug.Log("Loading experiment at " + experimentPath);
        LoadExperiment.LoadExperimentByPath(experimentPath, false);

        // Step: Recreate all events.
        Debug.Log("Recreating events.");
        LinkedList<LogEvent> events = RecreateEvents(lines);

        Debug.Log("Re-loading log based on events and prior log.");
        //Logging.LoadLog(lines, events);

        // Step: Scan for spawns, spawn again.
        Debug.Log("Re-spawning initial objects.");
        var gameObjs = new Dictionary<int, GameObject>();
        var hasMoved = new Dictionary<int, bool>();
        foreach (var evt in events)
        {
            if(evt is SelectableSpawner.SpawnEvent)
            {
                SelectableSpawner.SpawnEvent spawn = (SelectableSpawner.SpawnEvent)evt;
                var gObj = SelectableSpawner.Spawn(spawn.id, spawn.imageName, false);
                gameObjs.Add(spawn.id, gObj);
                hasMoved.Add(spawn.id, false);
            }
        }

        // Step: Scan backwards for moves, move each
        Debug.Log("Scanning backwards for move events.");
        var elm = events.Last;
        while(elm != null)
        {
            if(elm.Value is MoveSelectable.MoveSelectableEvent)
            {
                MoveSelectable.MoveSelectableEvent move = (MoveSelectable.MoveSelectableEvent)elm.Value;
                if(hasMoved[move.Id] == false)
                {
                    var gObj = gameObjs[move.Id];
                    gObj.transform.localPosition = move.To;
                }
            }
        }
    }
}
