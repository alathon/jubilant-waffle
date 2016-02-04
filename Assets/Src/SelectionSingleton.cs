using UnityEngine;
using System.Collections;

public class SelectionSingleton : MonoBehaviour {
	public static SelectionSingleton instance = null;
	private GameObject selected = null;
    public static GameObject Selected { get { return SelectionSingleton.instance.selected; } }

    public static void Select(GameObject o) {
		if (SelectionSingleton.instance.selected != null) {
			SelectionSingleton.Deselect (SelectionSingleton.instance.selected);
		}

        Logging.Log(string.Format("Selected {0}\n", o.name));
		SelectionSingleton.instance.selected = o;
		o.GetComponent<SelectionIndicator> ().Activate ();
	}

	public static void Deselect(GameObject o) {
		o.GetComponent<SelectionIndicator> ().Deactivate ();
        SelectionSingleton.instance.selected = null;
	}

    

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}
}