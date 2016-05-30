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
    

    void Awake()
    {
        GameObject ground = GameObject.Find("Ground");
        if (ground == null)
        {
            Debug.LogError("There is no GameObject called Ground! Cannot map coordinates.");
        }
        else
        {
            //groundMapper = GameObject.Find("Ground").GetComponent<GroundMapping>();
        }
    }

	void Update () {
        
	}
}
