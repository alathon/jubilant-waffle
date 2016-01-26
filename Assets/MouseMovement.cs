using UnityEngine;
using System.Collections;

public class MouseMovement : MonoBehaviour {
	void Start () {
	
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Plane playerPlane = new Plane (Vector3.up, transform.position);
		float hitdist = 0.0f;
		if (playerPlane.Raycast (ray, out hitdist)) {
			Vector3 targetPoint = ray.GetPoint (hitdist);
			this.
			transform.position = targetPoint;
		}
	}
}