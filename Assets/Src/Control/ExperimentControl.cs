using UnityEngine;

/**
 * Starting, saving and loading experiments.
 * Also has keybindings to toggle top-down view and movement mode
 **/
public class ExperimentControl : MonoBehaviour {
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ExperimentUtil.instance.LoadExperimentFromLog();
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
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ExperimentUtil.instance.StartNewExperiment();
        }
    }
}
