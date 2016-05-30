using UnityEngine;

/**
 * Provides methods to move the selectable object this script is
 * attached to. Note that movement is denied if the selectable happens
 * to be colliding with another selectable, within a certain range.
 * 
 * This can be controlled by the Physics.OverlapBox call, which is passed
 * a Vector3 size to determine how far away something can be and still be
 * considered overlapping with this GameObject.
 */
public class MoveSelectable : MonoBehaviour {
    public class MoveSelectableEvent : LogEvent
    {
        public readonly Vector3 From;
        public readonly Vector3 To;
        public readonly int Id;
        public MoveSelectableEvent(int id, Vector3 from, Vector3 to) : base()
        {
            this.Id = id;
            this.From = from;
            this.To = to;
        }

        public override string ToString()
        {
            return string.Format("MoveSelectable {0} from {1} -> {2}", Id, From, To);
        }

        // MoveSelectable 1 from (5.0, 0.5, 5.0) -> (5.1, 0.5, 5.2)
        public static LogEvent FromParts(string[] parts)
        {
            var id = int.Parse(parts[2]);

            float[] from = {    float.Parse(parts[4].Substring(1,parts[4].Length-2).Trim()),
                                float.Parse(parts[5].Remove(parts[5].IndexOf(',')).Trim()),
                                float.Parse(parts[6].Remove(parts[6].IndexOf(')')).Trim())
            };

            float[] to = {  float.Parse(parts[8].Substring(1, parts[8].Length-2).Trim()),
                            float.Parse(parts[9].Remove(parts[5].IndexOf(',')).Trim()),
                            float.Parse(parts[10].Remove(parts[6].IndexOf(')')).Trim())
            };

            var fromVect = new Vector3(from[0], from[1], from[2]);
            var toVect = new Vector3(to[0], to[1], to[2]);
            return new MoveSelectableEvent(id, fromVect, toVect);
        }
    }

    public void MoveTo(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapBox(pos, new Vector3(0.8f, 0.8f, 0.4f));
        foreach(Collider col in colliders)
        {
            if (col.gameObject == this.gameObject) continue;

            if(col is BoxCollider)
            {
                return;
            }
        }

        Vector3 oldPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        this.transform.position = pos;
        Logging.Log(new MoveSelectableEvent(int.Parse(this.gameObject.name), oldPos, this.transform.position));
    }

	public void MoveBy(Vector3 diff)
    {
        Vector3 oldPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        this.transform.Translate(diff);
        Logging.Log(new MoveSelectableEvent(int.Parse(this.gameObject.name), oldPos, this.transform.position));
    }
}
