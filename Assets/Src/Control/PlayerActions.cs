using UnityEngine;

/**
 * This script houses a collection of mouse-related click
 * things, like activating the DetailView on left click, and
 * spawning a new item on right click.
 */
public class PlayerActions : MonoBehaviour {
    private Vector3 lastDownPos;
    private float lastDown;
    private GameObject detailViewSpawned;

    // Interval between mouse down/up for something to be a click.
    public float clickInterval = 0.1f;

    bool wasClick()
    {
        return Time.time - lastDown <= clickInterval;
    }

    void Update () {
        if (Input.GetMouseButtonDown(0)) lastDown = Time.time;

        if (Input.GetMouseButtonUp(0) && wasClick())
        {
            ExperimentUtil.instance.DetailView(this.detailViewSpawned);
            this.detailViewSpawned = null;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            this.detailViewSpawned = ExperimentUtil.instance.SpawnNext();
        }
    }
}
