﻿using System;
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
        if (GameManager.Instance.state == GameManager.State.Normal  || GameManager.Instance.state == GameManager.State.Sound  && !(MenuManager.Instance.currentState == MenuManager.MenuStates.Shop))
        {   
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.GetComponent<AudioSource>() != null && hit.transform.CompareTag("Bowl"))
                    {
                        InventoryManager.Instance.bowlsManager.PlaySound(hit.transform);
                    }
                }
                if (hit.transform.CompareTag("Bowl") && Input.GetMouseButton(0) && GameManager.Instance.state == GameManager.State.Normal)
                {

                    Pressing = true;
                    TimeUserHold += Time.deltaTime;

                    if (TimeUserHold >= TimeUserForLongPressState)
                    {

                        GameManager.Instance.SelectModeReposition();
                        GameManager.Instance.gameObject.GetComponent<BowlReposition>().SelectBowls(hit.transform.gameObject);
                        GameManager.Instance.PanningSlider.value = hit.transform.GetComponent<AudioSource>().panStereo;
                        GameManager.Instance.VolumeSlider.value = hit.transform.GetComponent<AudioSource>().volume;
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

        }
    }
}
