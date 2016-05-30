using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    private GameObject player;       
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    private Vector3 origEulerAngles;
    private Vector3 origOffset;

    public float yOffset = 5f;

    void OnEnable()
    {
        player = GameObject.Find("Player");
        origEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        origOffset = transform.position - player.transform.position;

        // Pan camera down to 90 degrees and move directly above player.
        transform.localEulerAngles = new Vector3(90f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        offset = new Vector3(0f, yOffset, 0f);
    }

    void OnDisable()
    {
        transform.localEulerAngles = origEulerAngles;
        transform.position = player.transform.position + origOffset;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;
    }
}
