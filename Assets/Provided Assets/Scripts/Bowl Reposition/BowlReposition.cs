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
        foreach(GameObject BowlinArray in Bowl){
            BowlinArray.transform.GetChild(0).gameObject.SetActive(false);
            BowlinArray.transform.GetComponent<AudioSource>().Stop();
        }
    }
    public void SelectBowl(RaycastHit hit)
    {
        
        if (Input.GetMouseButtonUp(0) && hit.transform.gameObject.CompareTag("Bowl"))
        {
            SelectBowls(hit);
        }
    }
    private void SelectBowls(RaycastHit hit)
    {

        if (Selectable)
        {
            if (!SelectedBowl)
            {
                SelectedBowl = hit.transform.gameObject;
                temp = hit.transform.position;
                SelectedBowl.transform.GetChild(0).gameObject.SetActive(true);
                SelectedBowl.transform.GetChild(0).gameObject.GetComponent<Light>().intensity=100;

            }
            else if (SelectedBowl && SelectedBowl2)
            {

                SelectedBowl2 = SelectedBowl = null;
                SelectBowls(hit);
            }
            else
            {
                SelectedBowl2 = hit.transform.gameObject;
                temp2 = hit.transform.gameObject.transform.position;
                SelectedBowl2.transform.GetChild(0).gameObject.GetComponent<Light>().intensity=100;
            }

        }

        if (SelectedBowl && SelectedBowl2)
        {
            Reposition();
        }
    }
    public void Reposition()
    {
        Selectable = false;
        iTween.MoveTo(SelectedBowl, iTween.Hash("position", temp2, "time", transitionspeed));
        iTween.MoveTo(SelectedBowl2, iTween.Hash("position", temp, "time", transitionspeed));
        
            Selectable = true;
        
            SelectedBowl.transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 0;
            SelectedBowl2.transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 0;
        }
        

    private void ResetFunction()
    {
        SelectedBowl = SelectedBowl2 = null;
    }
    public void RepositionBowlInitializer()
    {
        Bowl = new GameObject[(GameManager.Instance.BowlArray.Length)];
        for (int i = 0; i < Bowl.Length; i++)
        {
            Bowl[i] = GameManager.Instance.BowlArray[i];
        }
    }
    public void Load()
    {

    }
}