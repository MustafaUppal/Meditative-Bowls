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

    bool allowClick;

    bool _emit;
    private void Start()
    {
        LongPressState = false;
        Pressing = false;
    }
    public void EnableClick(bool enable)
    {
        // Debug.Log("EnableClick: " + enable);
        allowClick = enable;
    }
    private void Update()
    {
        //if (GameManager.Instance.state == GameManager.State.Normal &&
        //    !(MenuManager.Instance.currentState == MenuManager.MenuStates.Alram)
        //    && !(MenuManager.Instance.currentState == MenuManager.MenuStates.Library)
        //    && !(MenuManager.Instance.currentState == MenuManager.MenuStates.BowlPlacement)
        //    || GameManager.Instance.state == GameManager.State.Sound &&
        //    (MenuManager.Instance.currentState == MenuManager.MenuStates.Main ||
        //    MenuManager.Instance.currentState == MenuManager.MenuStates.Settings ||
        //    MenuManager.Instance.currentState == MenuManager.MenuStates.Recording
        //    ))

        if (allowClick)
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
                        hit.transform.GetComponent<Bowl>().PlaySound();
                    }
                }

                if(MenuManager.Instance.currentState == MenuManager.MenuStates.Settings)
                {
                    if (GameManager.Instance.State1 == GameManager.State.Normal &&  hit.transform.CompareTag("Bowl") && Input.GetMouseButton(0))
                    {

                        Pressing = true;
                        TimeUserHold += Time.deltaTime;

                        if (TimeUserHold >= TimeUserForLongPressState)
                        {

                            // GameManager.Instance.SelectModeReposition();
                            GameManager.Instance.gameObject.GetComponent<BowlReposition>().SelectBowls(hit.transform.gameObject);
                            LongPressState = true;
                            Pressing = false;
                            TimeUserHold = 0;
                        }
                    }
                    else
                    {
                        //GameManager.Instance.gameObject.GetComponent<BowlReposition>().SelectedBowl = null;
                        LongPressState = false;
                        Pressing = false;
                        TimeUserHold = 0;
                    }
                }
                else
                {
                    // if (hit.transform.CompareTag("Bowl") && Input.GetMouseButton(0))
                    // {

                    //     Pressing = true;
                    //     TimeUserHold += Time.deltaTime;

                    //     if (TimeUserHold >= TimeUserForLongPressState)
                    //     {
                    //         // PopupManager.Instance.messagePopup.Show("Invalid Action!", "This functionality can only be used in Settings Menu.");
                    //     }
                    // }
                    // else
                    // {
                    //     //GameManager.Instance.gameObject.GetComponent<BowlReposition>().SelectedBowl = null;
                    //     LongPressState = false;
                    //     Pressing = false;
                    //     TimeUserHold = 0;

                    // }
                }
            }

        }
    }
}
