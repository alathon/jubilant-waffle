using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class HideOnCollision : MonoBehaviour {
	private LinkedList<GameObject> collidedWith = new LinkedList<GameObject>();
    private bool dirty = false;

    void OnTriggerEnter(Collider other) {
        if (other.GetComponent<SelectionIndicator>() != null)
        {
            collidedWith.AddLast(other.gameObject);
            dirty = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.GetComponent<SelectionIndicator>() != null)
        {
            if(collidedWith.Contains(other.gameObject))
            {
                //print(string.Format("Found {0} in collidedWith. Removing.", other.gameObject));
                collidedWith.Remove(other.gameObject);
                dirty = true;
            }
        }
    }

    GameObject GetClosest()
    {
        double dist = Double.MaxValue;
        GameObject closest = null;
        foreach(GameObject gObj in collidedWith)
        {
            double d = Vector3.Distance(gObj.transform.position, this.transform.position);
            if (d <= dist) closest = gObj;
        }
        return closest;
    }

    void Update()
    {
        if(dirty)
        {
            GameObject closest = GetClosest();
            GameObject selected = SelectionSingleton.Selected;
            //print(string.Format("Closest: {0}, Selected: {1}", closest, selected));

            if(closest != null)
            {
                if (selected == null)
                {
                    SelectionSingleton.Select(closest);
                    this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                } else if(selected != closest && Input.GetMouseButton(0) == false)
                {
                    SelectionSingleton.Select(closest);
                    this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            } else
            {
                SelectionSingleton.Deselect(SelectionSingleton.Selected);
                this.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }

            dirty = false;
        }
    }
}
