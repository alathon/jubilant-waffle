using UnityEngine;
using System.Collections;

public class SelectionIndicator : MonoBehaviour {
	private MeshRenderer indicatorMeshRenderer = null;

	void Awake() {
		indicatorMeshRenderer = this.gameObject.transform.FindChild ("Indicator").GetComponent<MeshRenderer> ();
	}

	public void Activate() {
		indicatorMeshRenderer.enabled = true;
	}

	public void Deactivate() {
		indicatorMeshRenderer.enabled = false;
	}
}
