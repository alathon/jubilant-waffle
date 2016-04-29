﻿using UnityEngine;
using System.Collections;

public class MoveSelectable : MonoBehaviour {
    public class MoveSelectableEvent
    {
        public readonly Vector3 From;
        public readonly Vector3 To;
        public readonly string Name;
        public MoveSelectableEvent(string name, Vector3 from, Vector3 to)
        {
            this.Name = name;
            this.From = from;
            this.To = to;
        }

        public override string ToString()
        {
            return string.Format("MoveSelectable <{0}> from {1} -> {2}", Name, From, To);
        }
    }

	public void MoveBy(Vector3 diff)
    {
        Vector3 oldPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        this.transform.Translate(diff);
        Logging.Log(new MoveSelectableEvent(this.gameObject.name, oldPos, this.transform.position));
    }
}