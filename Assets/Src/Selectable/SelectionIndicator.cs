using UnityEngine;
using System.Collections;

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
