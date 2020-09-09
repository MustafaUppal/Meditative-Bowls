using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSeection : MonoBehaviour
{
    Color finalColor;
    Color baseColor;
    float emission;
    public Material mat;
    public GameObject SelectedBowl;
    public bool LongPressState, Pressing;
    public float TimeUserHold, TimeUserForLongPressState;
    Renderer renderer;
    bool _emit;
    private void Start()
    {
        LongPressState = false;
        Pressing = false;
    }

    private void Update()
    {
        if (AllRefs.I._GameManager.state == GameManager.State.Normal &&
            !(MenuManager.Instance.currentState == MenuManager.MenuStates.Alram)
            || AllRefs.I._GameManager.state == GameManager.State.Sound &&
            (MenuManager.Instance.currentState == MenuManager.MenuStates.Main ||
            MenuManager.Instance.currentState == MenuManager.MenuStates.Settings ||
            MenuManager.Instance.currentState == MenuManager.MenuStates.Recording
            && !(MenuManager.Instance.currentState == MenuManager.MenuStates.Library)) )
        {   
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                // Debug.Log(hit.transform);
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.GetComponent<AudioSource>() != null && hit.transform.CompareTag("Bowl"))
                    {
                        // Debug.Log("Click");
                        InventoryManager.Instance.bowlsManager.PlaySound(hit.transform);
                    }
                }
                if (hit.transform.CompareTag("Bowl") && Input.GetMouseButton(0) && AllRefs.I._GameManager.state == GameManager.State.Normal&&MenuManager.Instance.currentState==MenuManager.MenuStates.Settings)
                {

                    Pressing = true;
                    TimeUserHold += Time.deltaTime;

                    if (TimeUserHold >= TimeUserForLongPressState)
                    {

                        AllRefs.I._GameManager.SelectModeReposition();
                        AllRefs.I._GameManager.gameObject.GetComponent<BowlReposition>().SelectBowls(hit.transform.gameObject);
                        AllRefs.I._GameManager.PanningSlider.value = hit.transform.GetComponent<AudioSource>().panStereo;
                        AllRefs.I._GameManager.VolumeSlider.value = hit.transform.GetComponent<AudioSource>().volume;
                        LongPressState = true;
                        Pressing = false;
                        TimeUserHold = 0;
                        AllRefs.I.settingMenu.ManageFooter(true);
                    }
                }
                else
                {
                    //AllRefs.I._GameManager.gameObject.GetComponent<BowlReposition>().SelectedBowl = null;
                    LongPressState = false;
                    Pressing = false;
                    TimeUserHold = 0;

                }
            }

        }
    }
}
