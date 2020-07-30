using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
//using UnityEditorInternal;

public class GameManager : MonoBehaviour
{
    [Header("GameManager Variable")]
    public static GameManager Instance;
    public GameObject postPocessing;
    public bool Reposition;
    public GameObject carpetPlane;
    public GameObject Bowl;
    public int BowlToLoad;
    [SerializeField] private GameObject SelectedSoundBowl;
    [SerializeField] private Text SoundChangerIndicatorText;
    public GameObject BackgroundMusic;
    [SerializeField] private Slider VolumeSlider;


    [Header("State")]
    public State state;
    public enum State
    {
        Normal,
        RepositionState,
        Shop,
        RecordingMode,
        Load,
        Remove,
        Sound
    }

    void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        switch (state)
        {
            case State.Normal:
                break;
            case State.RepositionState:
                break;
            case State.RecordingMode:
                break;
            case State.Load:

                LoadABowl();

                break;
            case State.Sound:
                if (Input.GetMouseButtonUp(0))
                {
                    StartCoroutine(VolumeChanger());
                }
                break;
            case State.Remove:
                StartCoroutine(Remove());
                break;
        }
    }

    private IEnumerator Remove()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Bowl") && Input.GetMouseButton(0))
            {
                hit.transform.gameObject.SetActive(false);
                hit.transform.GetComponent<Bowl>().currentState = Item.State.Purchased;
            }
        }
        yield return null;
    }
    private IEnumerator VolumeChanger()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Bowl"))
            {
                SelectedSoundBowl = hit.transform.gameObject;
                SoundChangerIndicatorText.text = "Bowl";
            }
            else
            {
                SoundChangerIndicatorText.text = "BackGound Sound";
                SelectedSoundBowl = BackgroundMusic;
            }
        }
        if (SelectedSoundBowl)
            SelectedSoundBowl.GetComponent<AudioSource>().volume = VolumeSlider.value;
        yield return null;
    }
    private void LoadABowl()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Why checking audio source when you are already checking tag?
                // Is there any possiblilty that a gameobject with bol take will not have audiosource? 
                if (hit.transform.GetComponent<AudioSource>() != null && hit.transform.CompareTag("Bowl"))
                {
                    // Disable bowl which is clicked
                    hit.transform.gameObject.SetActive(false);

                    // Place bowl to load on clicked bowl position and enable it
                    Inventory.Instance.allBowls[BowlToLoad].transform.position = hit.transform.position;
                    Inventory.Instance.allBowls[BowlToLoad].transform.gameObject.SetActive(true);

                    // Replace new bowl's index with clicked bowl index in active bowls array
                    int hitItemIndex = Array.FindIndex(Inventory.Instance.allBowls, x => x == hit.transform.GetComponent<Bowl>());
                    BowlsManager.Instance.activeBowlsIndexes[hitItemIndex] = BowlToLoad;

                    state = State.Normal;
                }
            }
        }
    }

    public void SelectModeReposition()
    {
        state = State.RepositionState;
        this.gameObject.GetComponent<BowlReposition>().RepositionBowlInitializer();
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
