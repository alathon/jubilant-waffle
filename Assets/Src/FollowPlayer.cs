using UnityEngine;

/**
 * Forces the GameObject this script is attached to
 * to follow the GameObject with the name 'Player',
 * and to do so looking straight downward at the player.
 * 
 * This is meant to be attached to a camera, to make it follow
 * a player from above.
 * 
 * Set the yOffset to control how far above the player the camera
 * should be.
 * 
 * When enabled, moves the camera. When disabled, resets the camera
 * to its original offset and orientation.
 **/
public class FollowPlayer : MonoBehaviour {
    private GameObject player;       
    private Vector3 offset; //Private variable to store the offset distance between the player and camera

    private Vector3 origEulerAngles;
    private Vector3 origOffset;
    public float yOffset = 5f;
    private Vector2 screenMiddle;

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
