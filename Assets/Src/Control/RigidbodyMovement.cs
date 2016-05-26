using UnityEngine;
using System.Collections;

public class RigidbodyMovementEvent : LogEvent
{
    public readonly Vector3 from;
    public readonly Vector3 to;

    public RigidbodyMovementEvent(Vector3 from, Vector3 to) : base()
    {
        this.from = from;
        this.to = to;
    }

    public override string ToString()
    {
        return string.Format("RigidBodyMove {0} -> {1}", from, to);
    }
}

public class RigidbodyMovement : MonoBehaviour {
    public string rigidbodyName = "Rigid Body XXX";
    private GameObject rigidBody;
    private GroundMapping groundMapper;
    private int checkCounter = 0;
    public int checkInterval = 10;

    void Awake()
    {
        GameObject ground = GameObject.Find("Ground");
        if (ground == null)
        {
            Debug.LogError("There is no GameObject called Ground! Cannot map coordinates.");
        }
        else
        {
            groundMapper = GameObject.Find("Ground").GetComponent<GroundMapping>();
        }
    }

	void Update () {
        if (rigidBody == null && (checkCounter % checkInterval) == 0)
        {
            rigidBody = GameObject.Find(rigidbodyName);
        }

        if (rigidBody != null && groundMapper != null)
        {
            var newPos = groundMapper.translate(rigidBody.transform.position);
            var clampedPos = new Vector3(Mathf.Clamp(newPos.x, 0f, 10f), newPos.y, Mathf.Clamp(newPos.z, 0f, 10f));

            if (Vector3.Distance(this.transform.position, clampedPos) > 0.1f)
            {
                this.transform.position = clampedPos;
                GameObject selected = SelectionSingleton.Selected;

                if (selected != null)
                {
                    if (Input.GetMouseButton(0))
                    {
                        selected.GetComponent<MoveSelectable>().MoveTo(new Vector3(clampedPos.x, selected.transform.position.y, clampedPos.z));
                    }
                }
                else
                {
                    Logging.Log(new RigidbodyMovementEvent(this.transform.position, clampedPos));
                }
            }
        }
	}
}
