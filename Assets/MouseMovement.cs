using UnityEngine;
using System.Collections;
using System;

public class MouseMovement : MonoBehaviour {
    GameObject ground;
    float xMin = -6.23f;
    float xMax = 7.65f;
    float zMin = -3.85f;
    float zMax = 5.08f;

    void Awake()
    {
        ground = GameObject.Find("Ground");
    }

    void Start () {
	
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Plane playerPlane = new Plane (Vector3.up, transform.position);
		float hitdist = 0.0f;
		if (playerPlane.Raycast (ray, out hitdist)) {
			Vector3 i = ray.GetPoint (hitdist);
            Vector3 targetPoint = new Vector3(Mathf.Clamp(i.x, zMin, zMax), i.y, Mathf.Clamp(i.z, xMin, xMax));
            if (Vector3.Distance(transform.position, targetPoint) > 0.01f)
            {
                Logging.Log(string.Format("MouseMove {0} -> {1} \n", transform.position, targetPoint));
            }

            this.transform.position = targetPoint;
		}
	}
}