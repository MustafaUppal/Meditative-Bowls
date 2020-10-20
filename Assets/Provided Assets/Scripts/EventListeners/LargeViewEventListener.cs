using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SerializeableClasses;

public class LargeViewEventListener : MonoBehaviour
{
    public ButtonOnOffSettings rotateButton;
    public RotateObj bowl;

    public bool startRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickRotateButton()
    {
        startRotation = !startRotation;
        rotateButton.SetIcon(startRotation);
        bowl.rotate = startRotation;
    }
}
