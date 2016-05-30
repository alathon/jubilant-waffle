using UnityEngine;

/**
 * Forces the GameObject to always look at the camera. Used for items
 * so that they are rotated so as to face the screen, whatever perspective 
 * we happen to be in.
 */
public class CameraFacingBillboard : MonoBehaviour
{
    private Camera m_Camera;

    void Awake()
    {
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
    }
}