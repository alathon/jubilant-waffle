using UnityEngine;

/**
 * A script to control the selection indicator of a selectable item,
 * i.e. the red border.
 */
public class SelectionIndicator : MonoBehaviour {
	private MeshRenderer indicatorMeshRenderer = null;
    public GameObject indicatorChild;

	void Awake() {
		indicatorMeshRenderer = indicatorChild.GetComponent<MeshRenderer> ();
	}

	public void Activate() {
		indicatorMeshRenderer.enabled = true;
	}

	public void Deactivate() {
		indicatorMeshRenderer.enabled = false;
	}
}
