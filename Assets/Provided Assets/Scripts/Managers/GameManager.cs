using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEditorInternal;

public class GameManager : MonoBehaviour
{
    [Header("GameManager Variable")]
    public static GameManager Instance;
    public GameObject postPocessing;
    public bool Reposition;
    public GameObject[] BowlArray;
    public GameObject Bowl;
    public GameObject BowlToLoad;
    public State state;




    public enum State
    {
        Normal,
        RepositionState,
        Shop,
        RecordingMode,
        Load
    }

    void Start()
    {
        Instance = this;
    }
    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                break;
            case State.RepositionState:
                break;
            case State.RecordingMode:
                break;
            case State.Load:
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Input.GetMouseButtonUp(0))
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        Transform objectHit = hit.transform;

                        if (hit.transform.GetComponent<AudioSource>() != null)
                            if (hit.transform.CompareTag("Bowl"))
                            {
                                hit.transform.gameObject.SetActive(false);
                                BowlToLoad.transform.position = hit.transform.position;
                                state = GameManager.State.Normal;
                            }
                    }
                }
                break;
        }
    }
    public void SelectModeReposition()
    {

        state = State.RepositionState;
        this.gameObject.GetComponent<BowlReposition>().StopEveryThing();
        this.gameObject.GetComponent<BowlReposition>().FadeEffect();
    }

    public void SelectModeNormal()
    {

        state = State.Normal;
    }
    public void SelectModeRecording()
    {
        state = State.RecordingMode;
    }
    public void SelectShopModeState()
    {
        state = State.Shop;
    }



}
