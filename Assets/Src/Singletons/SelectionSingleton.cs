using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionSingleton : MonoBehaviour {
	public static SelectionSingleton instance = null;
	private GameObject selected = null;
    public static GameObject Selected { get { return SelectionSingleton.instance.selected; } }

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

    public static void Select(GameObject o) {
		if (SelectionSingleton.instance.selected != null) {
			SelectionSingleton.Deselect (SelectionSingleton.instance.selected);
		}

        Logging.LogText(string.Format("Selected {0}", o.name));
		SelectionSingleton.instance.selected = o;
		o.GetComponent<SelectionIndicator> ().Activate ();
	}

	public static void Deselect(GameObject o) {
		o.GetComponent<SelectionIndicator> ().Deactivate ();
        SelectionSingleton.instance.selected = null;
	}

    

	
}