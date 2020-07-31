using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BowlReposition : MonoBehaviour
{
    [SerializeField] GameObject[] BowlPosition;
    [SerializeField] GameObject[] Bowl;
    [SerializeField] Light SelectLight;
    [SerializeField] GameObject SelectedBowl, SelectedBowl2;
    [SerializeField] bool Repositions;
    [SerializeField] Vector3 temp2;
    [SerializeField] Vector3 temp;
    [SerializeField] Vector3 positiontoarrive;
    [SerializeField] Vector3 positiontoarrive2;
    [SerializeField] private bool Selectable;
    [SerializeField] private float transitionspeed;
    [SerializeField] private Material[] materialArray;

    void Start()
    {
        Selectable = true;
        Invoke("RepositionBowlInitializer", 0.1f);
    }

    private void Update()
    {
        
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
    }
    public void ResetFuntion()
    {
        for (int i = 0; i < Bowl.Length; i++)
        {
            int bowlIndex = BowlsManager.Instance.activeBowlsIndexes[i];

            Bowl[i].GetComponent<Renderer>().material = materialArray[bowlIndex] ;
        }
    }
    
    
    private void SelectBowls(RaycastHit hit)
    {
        
        if (Selectable)
        {
            if (!SelectedBowl)
            {
                Material tempMaterial;
                SelectedBowl = hit.transform.gameObject;
                temp = hit.transform.position;
                SelectedBowl.transform.GetChild(0).gameObject.SetActive(true);
                SelectedBowl.transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 50;
                tempMaterial = SelectedBowl.transform.GetComponent<Material>();
                SelectedBowl.GetComponent<Renderer>().material = OriginalMaterial;

            }
            else if (SelectedBowl && SelectedBowl2)
            {
                SelectedBowl2 = SelectedBowl = null;
                SelectBowls(hit);
            }
            else
            {
                Material tempMaterial;
                SelectedBowl2 = hit.transform.gameObject;
                temp2 = hit.transform.gameObject.transform.position;
                SelectedBowl2.transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 50;
                SelectedBowl2.GetComponent<Renderer>().material = OriginalMaterial;

            }
            //FadeEffect();

        }

        if (SelectedBowl && SelectedBowl2)
        {
            Invoke("Reposition",0.10f);
        }
    }

    public void Reposition()
    {
        Selectable = false;
        iTween.MoveTo(SelectedBowl, iTween.Hash("position", temp2, "time", transitionspeed));
        iTween.MoveTo(SelectedBowl2, iTween.Hash("position", temp, "time", transitionspeed));


        SelectedBowl.GetComponent<Renderer>().material = SubsituteMaterial;
        SelectedBowl2.GetComponent<Renderer>().material = SubsituteMaterial;
        SelectedBowl.transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 0;
        SelectedBowl2.transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 0;
        Selectable = true;
    }
    [SerializeField] private Material SubsituteMaterial;
    [SerializeField] private Material OriginalMaterial;
    public void FadeEffect()
    {
        foreach (GameObject bowl in Bowl)
        {
            bowl.GetComponent<Renderer>().material = SubsituteMaterial;
        }
    }

    private void ResetFunction()
    {
        SelectedBowl = SelectedBowl2 = null;
    }
    public void RepositionBowlInitializer()
    {
        Bowl = new GameObject[(BowlsManager.Instance.activeBowlsIndexes.Length)];
        materialArray= new Material[BowlsManager.Instance.activeBowlsIndexes.Length];

        for (int i = 0; i < Bowl.Length; i++)
        {
            int bowlIndex = BowlsManager.Instance.activeBowlsIndexes[i];
            
            Bowl[i] = Inventory.Instance.allBowls[bowlIndex].gameObject;
            materialArray[i] = Inventory.Instance.allBowls[bowlIndex].GetComponent<Renderer>().material;
        }
    }
   
}