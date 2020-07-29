using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    public bool rotate;
    public float rotationSpeed;

    void Update()
    {
        if (!rotate)
            return;

        // Debug.Log("b y: " + transform.rotation.y);

        float x = transform.eulerAngles.x;
        float y = (transform.eulerAngles.y + Time.unscaledDeltaTime * rotationSpeed) % 360;
        float z = transform.eulerAngles.z;

        transform.eulerAngles = new Vector3(x, y, z);

        // Debug.Log("a y: " + transform.eulerAngles.y);
        // Debug.Log("---------------------");

    }
}
