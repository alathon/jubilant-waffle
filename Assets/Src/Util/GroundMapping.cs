using UnityEngine;
using System.Collections;

// Linear interpolation between coordinates supplied by OptiTrack and a Unity local quad.
public class GroundMapping : MonoBehaviour {
    // X scale of quad.
    public int quadXSize = 10;
    // Y scale of quad.
    public int quadYSize = 10;

    // X Value at leftmost point in room.
    public float xMin = -1.4f;
    // X Value at rightmost point in room.
    public float xMax = 1.5f;

    // Z Value furthest back in room.
    public float zMin = 2.045f;
    // Z Value furthest forward in room.
    public float zMax = 4.4f;

    // These values automatically calculated.
    private float xMult;
    private float zMult;

    public GroundMapping()
    {
        xMult = quadXSize / (xMax - xMin);
        zMult = quadYSize / (zMax - zMin);
    }

    // Translate from OptiTrack-provided coordinates
    // to the local proportions of this ground quad.
    public Vector3 translate(Vector3 input)
    {
        var x = (input.x - xMin) * xMult;
        var z = (input.z - zMin) * zMult;
        return new Vector3(x, 0f, z);
    }
}
