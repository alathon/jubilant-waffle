using UnityEngine;
using System.Collections;

public class SphereMovement : MonoBehaviour {
    public class SphereMovementEvent
    {
        public readonly Vector3 From;
        public readonly Vector3 To;
        public SphereMovementEvent(Vector3 from, Vector3 to)
        {
            this.From = from;
            this.To = to;
        }
    }

	public void MoveBy(Vector3 diff)
    {
        Vector3 oldPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        this.transform.Translate(diff);
        Logging.Log(new SphereMovementEvent(oldPos, this.transform.position));
    }
}
