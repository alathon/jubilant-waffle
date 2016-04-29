using UnityEngine;
using System.Collections;

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