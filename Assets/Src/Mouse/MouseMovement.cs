using UnityEngine;
using System.Collections;
using System;

public class MouseMovement : MonoBehaviour {
    GameObject ground;
    float xMin = 0f;
    float xMax = 10f;
    float zMin = 0f;
    float zMax = 10f;
    
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
            return string.Format("MouseMove {0} -> {1}", from, to);
        }
    }

    void Awake()
    {
        ground = GameObject.Find("Ground");
    }

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Plane playerPlane = new Plane (Vector3.up, transform.position);
		float hitdist = 0.0f;
		if (playerPlane.Raycast (ray, out hitdist)) {
			Vector3 i = ray.GetPoint (hitdist);
            Vector3 targetPoint = new Vector3(Mathf.Clamp(i.x, zMin, zMax), i.y, Mathf.Clamp(i.z, xMin, xMax));
            Vector3 oldPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            if (Vector3.Distance(oldPosition, targetPoint) > 0.01f)
            {
                GameObject selected = SelectionSingleton.Selected;

                // Only log mouse movements when we don't have something selected,
                // since moving a selected object generates its own event.
                if(selected == null) Logging.Log(new MouseMovementEvent(oldPosition, targetPoint));
                
                this.transform.position = targetPoint;
                if (Input.GetMouseButton(0))
                {
                    
                    if (selected != null)
                    {
                        Vector3 diff = targetPoint - oldPosition;
                        selected.GetComponent<MoveSelectable>().MoveBy(diff);
                    }
                }
            }
        }
	}
}