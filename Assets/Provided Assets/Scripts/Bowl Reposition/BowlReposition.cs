using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BowlReposition : MonoBehaviour
{
    public GameObject[] Bowl;
    public GameObject[] BowlPosition;
    public Material originalMat;
    public GameObject SelectedBowl, SelectedBowl2;
    public LayerMask clickableLine;
    public bool Repositions;
    [SerializeField] private Material SubsituteMaterial;
    [SerializeField] Vector3 temp2;
    [SerializeField] Vector3 temp;
    [SerializeField] Vector3 positiontoarrive;
    [SerializeField] Vector3 positiontoarrive2;
    [SerializeField] private bool Selectable;
    [SerializeField] private float transitionspeed;
    void Start()
    {
        Selectable = true;
        Invoke("RepositionBowlInitializer", 0.5f);

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

    public void SelectBowl(RaycastHit hit)
    {
        //print(hit.transform.gameObject.name);
        if (Input.GetMouseButtonUp(0) && hit.transform.gameObject.CompareTag("Bowl"))
        {
            SelectBowls(hit);

        }
    }

    private void SelectBowls(RaycastHit hit)
    {
        Material tempmat = SubsituteMaterial;
        Material tempmat2 = SubsituteMaterial;
        if (Selectable)
        {
            if (!SelectedBowl)
            {
                SelectedBowl = hit.transform.gameObject;
                temp = hit.transform.position;

                positiontoarrive = SelectedBowl.transform.position;
                tempmat = hit.transform.GetComponent<Material>();

                hit.transform.GetComponent<Renderer>().material = SubsituteMaterial;
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


                positiontoarrive2 = SelectedBowl2.transform.position;

                tempmat2 = hit.transform.GetComponent<Material>();

                hit.transform.GetComponent<Renderer>().material = SubsituteMaterial;
            }

        }

        if (SelectedBowl && SelectedBowl2)
        {
            Reposition(tempmat, tempmat2);
        }
    }
    public void Reposition(Material mat, Material mat2)
    {

        iTween.MoveTo(SelectedBowl, iTween.Hash("position", temp2, "time", transitionspeed));
        iTween.MoveTo(SelectedBowl2, iTween.Hash("position", temp, "time", transitionspeed));
        Selectable = false;

        print(SelectedBowl.transform.GetComponent<Renderer>().material.name);


        SelectedBowl.transform.GetComponent<Renderer>().material = originalMat;
        SelectedBowl2.transform.GetComponent<Renderer>().material = originalMat;
        Selectable = true;
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
}