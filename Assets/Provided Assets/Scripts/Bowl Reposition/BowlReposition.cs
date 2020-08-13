using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

public class BowlReposition : MonoBehaviour
{

    [SerializeField] GameObject[] Bowl;
    [SerializeField] Light SelectLight;
    public GameObject SelectedBowl, SelectedBowl2;
    [SerializeField] bool Repositions;
    [SerializeField] Vector3 temp2;
    [SerializeField] Vector3 temp;
    [SerializeField] Vector3 positiontoarrive;
    [SerializeField] Vector3 positiontoarrive2;
    [SerializeField] private bool Selectable;
    [SerializeField] private float transitionspeed;
    [SerializeField] private Material[] materialArray;

    public InventoryManager Inventory => InventoryManager.Instance;

    void Start()
    {
        Selectable = true;
        Invoke("RepositionBowlInitializer", 0.1f);
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (GameManager.Instance.state == GameManager.State.RepositionState)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                SelectBowl(hit);
            }
        }
    }
    public void StopEveryThing()
    {
        foreach (GameObject BowlinArray in Bowl)
        {
            BowlinArray.transform.GetChild(0).gameObject.SetActive(false);
            BowlinArray.GetComponent<AudioSource>().Stop();
        }

    }
    public void SelectBowl(RaycastHit hit)
    {

        if (Input.GetMouseButtonUp(0) && hit.transform.gameObject.CompareTag("Bowl"))
        {
            SelectBowls(hit);
        }
        if (Input.GetMouseButtonDown(0))
            print(hit.transform.gameObject.name);
        if (!(hit.transform.gameObject.tag == "Bowl") && Input.GetMouseButtonDown(0))
        {
            ResetFuntion();
        }

    }
    public void ResetFuntion()
    {
        for (int i = 0; i < materialArray.Length; i++)
        {
            Inventory.allBowls[Inventory.bowlsManager.activeBowlsIndexes[i]].GetComponent<Renderer>().material = materialArray[i];
        }
        GameManager.Instance.state = GameManager.State.Normal;
        GameManager.Instance.FooterText.gameObject.SetActive(true);
        GameManager.Instance.Footer.gameObject.SetActive(false);
        SelectedBowl = SelectedBowl2 = null;

    }


    public void SelectBowls(RaycastHit hit)
    {
        if (hit.transform.gameObject == SelectedBowl)
            return;


        SelectedBowl2 = hit.transform.gameObject;
        temp2 = hit.transform.gameObject.transform.position;
        SelectedBowl2.transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 50;
        SelectedBowl2.GetComponent<Renderer>().material = OriginalMaterial;


        int SelectedBowlIndex = Array.FindIndex(Inventory.allBowls.ToArray(), x => x == SelectedBowl.GetComponent<Bowl>()); ;
        int SelectedBowlIndex2 = Array.FindIndex(Inventory.allBowls.ToArray(), x => x == SelectedBowl2.GetComponent<Bowl>());

        print(SelectedBowlIndex);
        SelectedBowl2.GetComponent <AudioSource>().panStereo= BowlPanning(SelectedBowl,Inventory.bowlsManager.BowlPanningValues[SelectedBowlIndex2] );
        SelectedBowl.GetComponent<AudioSource>().panStereo=BowlPanning(SelectedBowl2,Inventory.bowlsManager.BowlPanningValues[SelectedBowlIndex]);
    
        if (SelectedBowl && SelectedBowl2)
        {
            Invoke("Reposition", 0.10f);
            SelectedBowl.GetComponent<AudioSource>().panStereo=BowlPanning(SelectedBowl2,Inventory.bowlsManager.BowlPanningValues[SelectedBowlIndex]);

        }
    }
    public float BowlPanning(GameObject BowlRequire,float value)
    {
        BowlRequire.GetComponent<AudioSource>().panStereo = value;
        return  value;
    }
    public void SelectBowls(GameObject SelectaBowl)
    {

        SelectedBowl = SelectaBowl;
        temp = SelectaBowl.transform.position;
        SelectedBowl.transform.GetChild(0).gameObject.SetActive(true);
        SelectedBowl.transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 50;
        SelectedBowl.GetComponent<Renderer>().material = OriginalMaterial;

    }

    public void Reposition()
    {
        Selectable = false;
        SelectedBowl.GetComponent<AudioSource>().spatialBlend = SelectedBowl2.GetComponent<AudioSource>().spatialBlend = 1;
        iTween.MoveTo(SelectedBowl, iTween.Hash("position", temp2, "time", transitionspeed));
        iTween.MoveTo(SelectedBowl2, iTween.Hash("position", temp, "time", transitionspeed));

        SelectedBowl.GetComponent<Renderer>().material = SubsituteMaterial;
        SelectedBowl2.GetComponent<Renderer>().material = SubsituteMaterial;

        Selectable = true;
        GameManager.Instance.SelectModeNormal();
        ResetFuntion();
    }
    [SerializeField] private Material SubsituteMaterial;
    [SerializeField] private Material OriginalMaterial;
    public void FadeEffect()
    {
        for (int i = 0; i < Inventory.bowlsManager.activeBowlsIndexes.Length; i++)
        {
            int index = Inventory.bowlsManager.activeBowlsIndexes[i];

            if (index > -1)
                Inventory.allBowls[index].GetComponent<Renderer>().material = SubsituteMaterial;
        }
    }

    private void ResetFunction()
    {
        SelectedBowl = SelectedBowl2 = null;

    }
    public void RepositionBowlInitializer()
    {
        //Bowl = new GameObject[(BowlsManager.Instance.activeBowlsIndexes.Length)];
        materialArray = new Material[Inventory.bowlsManager.activeBowlsIndexes.Length];
        
        for (int i = 0; i < materialArray.Length; i++)
        {
            // print("Masti kr rya na");
            int bowlIndex = Inventory.bowlsManager.activeBowlsIndexes[i];
            if (bowlIndex > -1)
                materialArray[i] = Inventory.allBowls[bowlIndex].GetComponent<Renderer>().material;
            // Bowl[i] = Inventory.Instance.allBowls[bowlIndex].gameObject;
        }
        if (GameManager.Instance.state == GameManager.State.RepositionState)
        {
            GameManager.Instance.FooterText.gameObject.SetActive(false);
            GameManager.Instance.Footer.gameObject.SetActive(true);
        }
    }

}