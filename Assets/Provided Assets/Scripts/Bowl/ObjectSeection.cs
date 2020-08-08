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
        if (EventSystem.current.IsPointerOverGameObject())
            return;



        if (GameManager.Instance.state == GameManager.State.Normal || GameManager.Instance.state == GameManager.State.RepositionState || GameManager.Instance.state == GameManager.State.Sound && !(MenuManager.Instance.currentState == MenuManager.MenuStates.Shop))
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
                        hit = PlaySound(hit);
                    }
                }
                if (hit.transform.CompareTag("Bowl") && Input.GetMouseButton(0) && GameManager.Instance.state == GameManager.State.Normal)
                {

                    Pressing = true;
                    TimeUserHold += Time.deltaTime;

                    if (TimeUserHold >= TimeUserForLongPressState)
                    {

                        GameManager.Instance.SelectModeReposition();
                        //hit.transform.GetComponent<AudioSource>().spatialBlend = 1;
                        GameManager.Instance.gameObject.GetComponent<BowlReposition>().SelectBowls(hit.transform.gameObject);

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




    private static RaycastHit PlaySound(RaycastHit hit)
    {
        hit.transform.GetChild(0).gameObject.SetActive(true);
        hit.transform.transform.GetChild(0).GetComponent<AudioLightSync>().emit = true;

        if (hit.transform.GetComponent<AudioSource>().isPlaying)
            hit.transform.GetComponent<AudioSource>().Stop();

        hit.transform.GetComponent<AudioSource>().Play();
        //hit.transform.GetChild(0).gameObject.SetActive(true);
        return hit;
    }
}
