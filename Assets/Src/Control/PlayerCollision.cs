using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * This script maintains a list of objects the player has
 * collided with. The logic is as follows:
 * 
 * - If we have nothing selected, pick the closest object we are
 * currently colliding with and select it.
 * - If we have something selected, and are no longer holding down the
 * mouse button (i.e. dragging something), then select the closest object.
 * - If we have something selected, and are holding down the mouse button,
 * we will continue to be selecting that no matter what happens to be
 * closest.
 * 
 * To avoid excessively checking for updates, we only check for a new closest
 * selection if the list of collided objects changes. This happens whenever a collider
 * enters or exits the collider attached to the GameObject of this script, and is
 * controlled by the 'dirty' boolean.
 * 
 * The mesh renderer of the GameObject this script is attached to is turned off when
 * something is selected, i.e., to make the mouse cursor disappear when an item is
 * selected.
 */
public class PlayerCollision : MonoBehaviour
{
    public LinkedList<GameObject> CollidedWith { get; private set; }
    private bool dirty = false;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        this.meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        this.CollidedWith = new LinkedList<GameObject>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SelectionIndicator>() != null)
        {
            CollidedWith.AddLast(other.gameObject);
            dirty = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SelectionIndicator>() != null)
        {
            if (CollidedWith.Contains(other.gameObject))
            {
                CollidedWith.Remove(other.gameObject);
                dirty = true;
            }
        }
    }

    
    GameObject GetClosest()
    {
        double dist = Double.MaxValue;
        GameObject closest = null;
        foreach (GameObject gObj in CollidedWith)
        {
            double d = Vector3.Distance(gObj.transform.position, this.transform.position);
            if (d <= dist) closest = gObj;
        }
        return closest;
    }

    void Update()
    {
        if (dirty)
        {
            GameObject closest = GetClosest();
            GameObject selected = SelectionSingleton.Selected;

            if (closest != null)
            {
                if (selected == null)
                {
                    SelectionSingleton.Select(closest);
                    //closest.transform.position = new Vector3(this.transform.position.x, closest.transform.position.y, this.transform.position.z);
                    //transform.parent.gameObject.transform.position = closest.gameObject.transform.position;
                    this.meshRenderer.enabled = false;
                }
                else if (selected != closest && Input.GetMouseButton(0) == false)
                {
                    SelectionSingleton.Select(closest);
                    this.meshRenderer.enabled = false;
                }
            }
            else
            {
                if(Input.GetMouseButton(0) == false && selected != null) {
                    SelectionSingleton.Deselect(SelectionSingleton.Selected);
                    this.meshRenderer.enabled = true;
                }
            }

            dirty = false;
        }
    }
}
