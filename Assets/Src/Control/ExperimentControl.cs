using UnityEngine;
using System.Collections;

public class ExperimentControl : MonoBehaviour {
    private Vector3 lastDownPos;
    private float lastDown;

    // Interval between mouse down/up for something to be a click.
    public float clickInterval = 0.1f;

    bool wasClick()
    {
        return Time.time - lastDown <= clickInterval;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) lastDown = Time.time;

        if (Input.GetKeyDown(KeyCode.C))
        {
            ExperimentUtil.instance.LoadStateFromLog();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ExperimentUtil.instance.SaveExperiment();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            ExperimentUtil.instance.ToggleTopDownView();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ExperimentUtil.instance.ToggleMovementMethod();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            ExperimentUtil.instance.LoadExperimentFile();
        }
        if (Input.GetMouseButtonUp(0) && wasClick())
        {
            ExperimentUtil.instance.DetailView();
        }
        else if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.N))
        {
            ExperimentUtil.instance.SpawnNext();
        }
        
    }
}
