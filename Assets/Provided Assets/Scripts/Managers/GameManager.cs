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
    public GameObject carpetPlane;
    public GameObject[] BowlArray;
    public GameObject Bowl;
    public GameObject BowlToLoad;
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
        switch (state)
        {
            case State.Normal:
                break;
            case State.RepositionState:
                break;
            case State.RecordingMode:
                break;
            case State.Load:
                if (Input.GetMouseButtonUp(0))
                {
                   StartCoroutine(LoadABowl());
                }
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
            if (hit.transform.CompareTag("Bowl")&& Input.GetMouseButton(0))
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
    private IEnumerator LoadABowl()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
        yield return null;
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
