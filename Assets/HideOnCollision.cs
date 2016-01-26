using UnityEngine;
using System.Collections;

public class HideOnCollision : MonoBehaviour {
	private static string TARGET = "Sphere";
	private int collisionCount = 0;

	void OnTriggerEnter(Collider other) {
		print ("OnCollisionEnter!");
		if (other.gameObject.name.Equals (TARGET)) {
			collisionCount += 1;
			if(collisionCount > 0) this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			SelectionSingleton.Select (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.name.Equals (TARGET)) {
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
