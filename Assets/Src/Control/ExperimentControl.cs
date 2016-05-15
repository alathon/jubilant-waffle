using UnityEngine;
using System.Collections;

public class ExperimentControl : MonoBehaviour {
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ExperimentUtil.instance.LoadCatalog();
        }
        else if(Input.GetKeyDown(KeyCode.N))
        {
            ExperimentUtil.instance.SpawnNext();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ExperimentUtil.instance.SaveExperiment();
        }
    }
}
