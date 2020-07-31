using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSeection : MonoBehaviour
{
    Color finalColor;
    Color baseColor;
    float emission;
    public Material mat;
    Renderer renderer;
    bool _emit;
    private void Start()
    {

    }
    private void Update()
    {
        if (GameManager.Instance.state == GameManager.State.Normal&& !(MenuManager.Instance.currentState==MenuManager.MenuStates.Shop))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (Input.GetMouseButtonUp(0))
                {
                    if (hit.transform.GetComponent<AudioSource>() != null)
                        if (hit.transform.CompareTag("Bowl"))
                        {
                            hit = PlaySound(hit);
                        }
                }
                if (hit.transform.CompareTag("Bowl"))
                {
                    GameObject SpotLight = hit.transform.GetChild(0).gameObject;
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
