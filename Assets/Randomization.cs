using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Randomization : MonoBehaviour
{

    public Button button;
    public InputField Timetext;

    private void OnEnable() {
        AllRefs.I.objectSelection.EnableClick(false);    
    }

    private void OnDisable() {
        AllRefs.I.objectSelection.EnableClick(true);    
    }

    public void ValueUpdate(string Stext)
    {
           button.interactable=(int.Parse(Stext)) >0 && Stext != "";
    }
}
