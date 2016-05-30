using UnityEngine;

/**
 * Controls the movement of the player.
 * 
 * There are three movement types: MOUSE_ABS, MOUSE_REL and RIGIDBODY.
 * 
 * MOUSE_ABS works by projecting a ray from the camera centre and onto a
 * plane based on the players current position.
 * 
 * MOUSE_REL works by calculating a by-frame offset from where the mouse
 * was last, and moving the player by that offset. The reason this does
 * not use Input.GetAxis to calculate the offset is that this does not
 * work on Virtual Machines or over remote connections.
 * Please see http://forum.unity3d.com/threads/input-getaxis-mouse-x-and-y-axis-equivalent-dont-work-in-remote-desktop.115526/
 * for more information. You *can* switch over to GetAxis if you are okay
 * with not being able to remotely connect to the machine running the experiment
 * for testing. Ideally there should be a debug switch for this in the code,
 * where you can switch between using GetAxis() and the current method.
 * 
 * RIGIDBODY works by mapping the rigidbody movements (which come from the
 * MotiveDirect script) to the ground objects coordinates, which by default
 * are between 0 and 10 on the X and Y axis.
 * 
 * Movement is only considered movement if we have
 * moved more than a certain threshold. You can set this threshold in
 * the MaybeMoveTo method (the Vector3.Distance check), or remove it
 * entirely there. The reason for it is to not generate an excessive
 * amount of movement events, when the player is actually standing
 * still. Especially RIGIDBODY movements occur all the time, every frame.
 */
public class Movement : MonoBehaviour {
    public enum MovementType {  MOUSE_ABS, MOUSE_REL, RIGIDBODY };

    public MovementType movementType = MovementType.MOUSE_ABS;
    public string rigidbodyName = "Rigid Body 1"; // name of rigidbody in Motive.
    public int checkInterval = 10; // How many ms between checking for the rigidbody presence.
    public float relativeSpeed = 0.05f; // Dampening factor on relative movements.

    GameObject ground;
    PlayerCollision playerCollision;
    private GameObject rigidBody;
    private GroundMapping groundMapper;
    private int checkCounter = 0;

    
    private float xMin;
    private float xMax;
    private float zMin;
    private float zMax;

    // Works around remote desktop limitations
    private Vector2 lastMouseAxis;

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
        this.xMin = 0;
        this.xMax = groundMapper.quadXSize;
        this.zMin = 0;
        this.zMax = groundMapper.quadYSize;
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
        if(this.movementType == MovementType.MOUSE_REL)
        {
            if (lastMouseAxis != null)
            {
                Vector3 newAxis = new Vector3(Input.mousePosition.x - lastMouseAxis.x,
                0f, Input.mousePosition.y - lastMouseAxis.y);
                this.lastMouseAxis = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector3 dest = this.transform.localPosition + newAxis * this.relativeSpeed;
                dest = new Vector3(Mathf.Clamp(dest.x, zMin, zMax), dest.y, Mathf.Clamp(dest.z, zMin, zMax));
                this.MaybeMoveTo(dest);
            }
            else
            {
                lastMouseAxis = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        } else if(this.movementType == MovementType.MOUSE_ABS)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            float hitdist = 0.0f;
            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 i = ray.GetPoint(hitdist);
                Vector3 targetPoint = new Vector3(Mathf.Clamp(i.x, zMin, zMax), transform.position.y, Mathf.Clamp(i.z, xMin, xMax));
                this.MaybeMoveTo(targetPoint);
            }
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
            case MovementType.MOUSE_ABS:
            case MovementType.MOUSE_REL:
                this.MouseUpdate();
                break;
            case MovementType.RIGIDBODY:
                this.RigidbodyUpdate();
                break;
        }
	}
}