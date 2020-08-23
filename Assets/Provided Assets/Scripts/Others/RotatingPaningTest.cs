using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotatingPaningTest : MonoBehaviour
{
    public new Camera camera;

    public float zoomSpeed;
    public float rotationSpeed = 20f;
    public bool isOnPanel;

    [Header("Defualt Positions")]
    public Transform idleTransform;
    public float idleFieldOfView;

    private float initialFingersDistance;

    void Update()
    {
        if(!AllRefs.I.shopMenu.currentState.Equals(ShopMenuEventListener.ShopStates.Bowls))
            return;

        if (Input.GetMouseButton(0) && Input.touches.Length != 2 && isOnPanel)
        {
            camera.transform.RotateAround(transform.position, new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0), Time.deltaTime * rotationSpeed);
        }

        if (Input.touches.Length == 2)
        {
            _Scaling();
            return;
        }
    }

    public void Reset()
    {
        camera.fieldOfView = idleFieldOfView;
        camera.transform.position = idleTransform.position;
        camera.transform.rotation = idleTransform.rotation;
    }

    public void OnPanel(bool isOnPanel)
    {
        this.isOnPanel = isOnPanel;
    }

    void _Scaling()
    {
        if (Input.touches.Length == 2)
        {
            Touch t1 = Input.touches[0];
            Touch t2 = Input.touches[1];

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                initialFingersDistance = Vector2.Distance(t1.position, t2.position);
            }

            else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {

                if (!isOnPanel)
                    return;

                float currentFingersDistance = Vector2.Distance(t1.position, t2.position);
                var scaleFactor = currentFingersDistance / initialFingersDistance;

                float zoomSpeedTemp = zoomSpeed * Time.deltaTime;
                if (scaleFactor < 1)
                    zoomSpeedTemp = -zoomSpeedTemp;

                camera.fieldOfView += zoomSpeedTemp;
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 40, 70);
            }
        }

    }

}
