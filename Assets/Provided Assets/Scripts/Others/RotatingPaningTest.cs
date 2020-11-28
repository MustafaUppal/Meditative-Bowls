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
    public bool isPressed;
    public RotateObj rotationScript;


    [Header("Defualt Positions")]
    public Transform idleTransform;
    public float idleFieldOfView;

    private float initialFingersDistance;

    public bool isChanged = false;

    void Update()
    {
        if (!AllRefs.I.shopMenu.currentState.Equals(ShopMenuEventListener.ShopStates.Bowls))
            return;

        if (isOnPanel && Input.touches.Length != 2 && isPressed)
        {
            // rotationScript.rotate = false;
            if (!isChanged)
            {
                AllRefs.I.shopMenu.selectedItem.resetButton.SetActive(true);
                isChanged = true;
            }
            // camera.transform.RotateAround(transform.position, new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0), Time.deltaTime * rotationSpeed);
            transform.Rotate(Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad, -Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad, 0, Space.World);
        }
        else
        {
            // rotationScript.rotate = true;
        }

        if (Input.touches.Length == 2)
        {
            _Scaling();
            return;
        }
    }

    public void Reset()
    {
        isChanged = false;
        AllRefs.I.shopMenu.selectedItem.resetButton.SetActive(false);
        camera.fieldOfView = idleFieldOfView;
        transform.position = idleTransform.position;
        transform.rotation = idleTransform.rotation;
    }

    public void OnPanel(bool isOnPanel)
    {
        this.isOnPanel = isOnPanel;
    }

    public void OnPress(bool isPressed)
    {
        this.isPressed = isPressed;
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

                if (!isChanged)
                {
                    AllRefs.I.shopMenu.selectedItem.resetButton.SetActive(true);
                    isChanged = true;
                }
            }
        }

    }
}
