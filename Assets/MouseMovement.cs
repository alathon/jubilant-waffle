using UnityEngine;
using System.Collections;
using System;

public class MouseMovement : MonoBehaviour {
	void Start () {
	
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Plane playerPlane = new Plane (Vector3.up, transform.position);
		float hitdist = 0.0f;
		if (playerPlane.Raycast (ray, out hitdist)) {
			Vector3 targetPoint = ray.GetPoint (hitdist);
            if (Vector3.Distance(transform.position, targetPoint) > 0.01f)
            {
                Logging.Log(string.Format("MouseMove {0} -> {1} \n",transform.position, targetPoint));
            }

			this.transform.position = targetPoint;
		}
	}
}