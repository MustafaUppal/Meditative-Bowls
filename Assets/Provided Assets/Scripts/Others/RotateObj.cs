using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    public bool rotate;
    public float rotationSpeed;

    public bool xAxis;
    public bool yAxis = true;
    public bool zAxis;

    float x, y, z;

    void Start()
    {
        
    }

    void Update()
    {
        if (!rotate)
            return;

        // Debug.Log("b y: " + transform.rotation.y)

        x = xAxis ? (transform.eulerAngles.x + Time.unscaledDeltaTime * rotationSpeed) % 360 : transform.eulerAngles.x;
        y = yAxis ? (transform.eulerAngles.y + Time.unscaledDeltaTime * rotationSpeed) % 360 : transform.eulerAngles.y;
        z = zAxis ? (transform.eulerAngles.z + Time.unscaledDeltaTime * rotationSpeed) % 360 : transform.eulerAngles.z;

        transform.eulerAngles = new Vector3(x, y, z);

        // Debug.Log("a y: " + transform.eulerAngles.y);
        // Debug.Log("---------------------");

    }

    public void StartRotation(bool start)
    {
        rotate = start;
    }
}
