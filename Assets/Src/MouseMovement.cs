using UnityEngine;
using System.Collections;
using System;

public class MouseMovement : MonoBehaviour {
    GameObject ground;
    float xMin = -6.23f;
    float xMax = 7.65f;
    float zMin = -3.85f;
    float zMax = 5.08f;
    
    public class MouseMovementEvent
    {
        public readonly Vector3 from;
        public readonly Vector3 to;
        public readonly double time;

        public MouseMovementEvent(Vector3 from, Vector3 to)
        {
            this.from = from;
            this.to = to;
        }

        public override string ToString()
        {
            return string.Format("MouseMove {0} -> {1} \n", from, to);
        }
    }

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
            Vector3 oldPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            if (Vector3.Distance(oldPosition, targetPoint) > 0.001f)
            {
                Logging.Log(new MouseMovementEvent(oldPosition, targetPoint));
                
                this.transform.position = targetPoint;
                if (Input.GetMouseButton(0))
                {
                    GameObject selected = SelectionSingleton.Selected;
                    if (selected != null)
                    {
                        Vector3 diff = targetPoint - oldPosition;
                        selected.GetComponent<DataItemMovement>().MoveBy(diff);
                    }
                }
            }
        }
	}
}