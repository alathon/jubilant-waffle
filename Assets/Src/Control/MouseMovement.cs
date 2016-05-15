using UnityEngine;
using System.Collections;
using System;

public class MouseMovement : MonoBehaviour {
    GameObject ground;
    PlayerCollision playerCollision;
    float xMin = 0f;
    float xMax = 10f;
    float zMin = 0f;
    float zMax = 10f;
    
    public class MouseMovementEvent
    {
        public readonly Vector3 from;
        public readonly Vector3 to;

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
        playerCollision = this.GetComponentInChildren<PlayerCollision>();
        ground = GameObject.Find("Ground");
    }

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Plane playerPlane = new Plane (Vector3.up, transform.position);
		float hitdist = 0.0f;
		if (playerPlane.Raycast (ray, out hitdist)) {
			Vector3 i = ray.GetPoint (hitdist);
            Vector3 targetPoint = new Vector3(Mathf.Clamp(i.x, zMin, zMax), transform.position.y, Mathf.Clamp(i.z, xMin, xMax));
            Vector3 oldPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            if (Vector3.Distance(oldPosition, targetPoint) > 0.1f)
            {
                this.transform.position = targetPoint;
                GameObject selected = SelectionSingleton.Selected;

                if(selected != null)
                {
                    if(Input.GetMouseButton(0))
                    {
                        //Vector3 diff = targetPoint - oldPosition;
                        //selected.GetComponent<MoveSelectable>().MoveBy(diff);
                        selected.GetComponent<MoveSelectable>().MoveTo(new Vector3(targetPoint.x, selected.transform.position.y, targetPoint.z));
                    }
                } else
                {
                    Logging.Log(new MouseMovementEvent(oldPosition, targetPoint));
                }
            }
        }
	}
}