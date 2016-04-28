using UnityEngine;
using System.Collections;

public class FollowRigidbody : MonoBehaviour {
    public string rigidbodyName = "Rigid Body XXX";
    private GameObject rigidBody;
    private GroundMapping groundMapper;
    private int checkCounter = 0;
    public int checkInterval = 10;

    void Awake()
    {
        GameObject ground = GameObject.Find("Ground");
        if (ground == null)
        {
            Debug.LogError("There is no GameObject called Ground! Cannot map coordinates.");
        }
        else
        {
            groundMapper = GameObject.Find("Ground").GetComponent<GroundMapping>();
        }
    }

	void Update () {
        if (rigidBody == null && (checkCounter % checkInterval) == 0)
        {
            rigidBody = GameObject.Find(rigidbodyName);
        }

        if (rigidBody != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("X:" + rigidBody.transform.position.x + " Z:" + rigidBody.transform.position.z);
            }

            if(groundMapper != null) this.transform.position = groundMapper.translate(rigidBody.transform.position);
        }
	}
}
