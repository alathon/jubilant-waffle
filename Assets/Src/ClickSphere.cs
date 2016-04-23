using UnityEngine;
using System.Collections;

public class ClickSphere : MonoBehaviour {

	void OnMouseDown()
    {
        GameObject.Destroy(gameObject);
    }
}
