using UnityEngine;
using System.Collections;

public class HideOnCollision : MonoBehaviour {
	private int collisionCount = 0;

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<SelectionIndicator>() != null) {
			collisionCount += 1;
			if(collisionCount > 0) this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			SelectionSingleton.Select (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.GetComponent<SelectionIndicator>() != null) {
			SelectionSingleton.Deselect (other.gameObject);
			collisionCount -= 1;
		}
	}
		
	void Update() {
		if (collisionCount <= 0) {
			this.gameObject.GetComponent<MeshRenderer> ().enabled = true;
		}
	}
}
