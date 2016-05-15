using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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
