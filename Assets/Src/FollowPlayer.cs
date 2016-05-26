using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    public GameObject player;       //Public variable to store a reference to the player game object
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    private Vector3 origEulerAngles;
    public float yOffset = 5f;

    void OnEnable()
    {
        origEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        // Pan camera down to 90 degrees.
        transform.localEulerAngles = new Vector3(90f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        //offset = transform.position - player.transform.position;
        offset = new Vector3(0f, yOffset, 0f);
    }

    void OnDisable()
    {
        transform.localEulerAngles = origEulerAngles;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;
    }
}
