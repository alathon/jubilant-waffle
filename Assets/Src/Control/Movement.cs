using UnityEngine;
using System.Collections;
using System;

public class Movement : MonoBehaviour {
    public enum MovementType {  MOUSE, RIGIDBODY };

    public MovementType movementType = MovementType.MOUSE;
    public string rigidbodyName = "Rigid Body 1";
    public int checkInterval = 10;

    GameObject ground;
    PlayerCollision playerCollision;
    private GameObject rigidBody;
    private GroundMapping groundMapper;
    private int checkCounter = 0;
    float xMin = 0f;
    float xMax = 10f;
    float zMin = 0f;
    float zMax = 10f;
    
    public class MovementEvent : LogEvent
    {
        public readonly Vector3 from;
        public readonly Vector3 to;
        public MovementType type;

        public MovementEvent(Vector3 from, Vector3 to, MovementType type) : base()
        {
            this.from = from;
            this.to = to;
            this.type = type;
        }

        public override string ToString()
        {
            return string.Format("MOVE {0} : {1} -> {2} ", this.type, from, to);
        }
    }

    void Awake()
    {
        playerCollision = this.GetComponentInChildren<PlayerCollision>();
        ground = GameObject.Find("Ground");
        groundMapper = ground.GetComponent<GroundMapping>();
    }

    private void MaybeMoveTo(Vector3 pos)
    {
        if (Vector3.Distance(this.transform.position, pos) > 0.1f)
        {
            this.transform.position = pos;
            GameObject selected = SelectionSingleton.Selected;

            if (selected != null)
            {
                MoveSelectable ms = selected.GetComponent<MoveSelectable>();

                if (Input.GetMouseButton(0))
                {
                    ms.MoveTo(new Vector3(pos.x, selected.transform.position.y, pos.z));
                }
            }
            else
            {
                Logging.Log(new MovementEvent(this.transform.position, pos, this.movementType));
            }
        }
    }

    private void MouseUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        float hitdist = 0.0f;
        if (playerPlane.Raycast(ray, out hitdist))
        {
            Vector3 i = ray.GetPoint(hitdist);
            Vector3 targetPoint = new Vector3(Mathf.Clamp(i.x, zMin, zMax), transform.position.y, Mathf.Clamp(i.z, xMin, xMax));
            //Vector3 oldPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            this.MaybeMoveTo(targetPoint);
        }
    }
    
    private void RigidbodyUpdate()
    {
        if (rigidBody == null && (checkCounter % checkInterval) == 0)
        {
            rigidBody = GameObject.Find(rigidbodyName);
        }

        if (rigidBody != null && groundMapper != null)
        {
            var newPos = groundMapper.translate(rigidBody.transform.position);
            var clampedPos = new Vector3(Mathf.Clamp(newPos.x, 0f, 10f), newPos.y, Mathf.Clamp(newPos.z, 0f, 10f));

            this.MaybeMoveTo(clampedPos);
        }
    }

	void Update () {
		switch(this.movementType)
        {
            case MovementType.MOUSE:
                this.MouseUpdate();
                break;
            case MovementType.RIGIDBODY:
                this.RigidbodyUpdate();
                break;
            default:
                this.MouseUpdate();
                break;
        }
	}
}