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
    [Header("Game Manager Variable")]
    public static GameManager Instance;
    public Slider PanningSlider;

    public GameObject postPocessing;
    public bool Reposition;
    public GameObject carpetPlane;
    public GameObject Bowl;
    public GameObject Footer;
    public GameObject FooterText1;

    public int BowlToLoad;
    public Slider VolumeSlider;
    public Text FooterText;
    public Button[] allButtons;
    public GameObject BackgroundMusic;
    [SerializeField] private GameObject SelectedSoundBowl;

    [SerializeField] private Text SoundChangerIndicatorText;
    public string DefaultFooterText;
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
        DefaultFooterText = FooterText.text;

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
                VolumeChanger();
                if (SelectedSoundBowl)
                {
                    VolumeSlider.onValueChanged.AddListener(delegate { VolumeChange(VolumeSlider.value); });
                    PanningSlider.onValueChanged.AddListener(delegate { PanningSliderChange(PanningSlider.value); });
                }

                break;
            case State.RecordingMode:
                break;
            case State.Load:
                LoadABowl();

                break;
            case State.Sound:

                break;
            case State.Remove:
                Remove();
                break;
        }
    }
    public void PanningSliderChange(float SliderValue)
    {
        SelectedSoundBowl.GetComponent<AudioSource>().spatialBlend = 0;
        SelectedSoundBowl.GetComponent<AudioSource>().panStereo = SliderValue;
    }
    void VolumeChange(float Value)
    {
        SelectedSoundBowl.GetComponent<AudioSource>().volume = Value;
    }

    private void Remove()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Bowl") && Input.GetMouseButton(0))
            {
                hit.transform.gameObject.GetComponent<Bowl>().currentState = Item.State.Purchased;
                GameObject SubsituteGameObject = new GameObject();
                SubsituteGameObject.transform.position = hit.transform.gameObject.transform.position;
                SubsituteGameObject.AddComponent<BoxCollider>();
                SubsituteGameObject.AddComponent<Rigidbody>();
                SubsituteGameObject.AddComponent<indexHolder>();
                SubsituteGameObject.GetComponent<indexHolder>().index =
                Array.FindIndex(Inventory.Instance.allBowls, x => x == hit.transform.GetComponent<Bowl>());
                SubsituteGameObject.GetComponent<Rigidbody>().isKinematic = true;
                hit.transform.gameObject.SetActive(false);
                SubsituteGameObject.tag = "Bowl2";
                PanningSlider.value = SelectedSoundBowl.GetComponent<AudioSource>().panStereo;
            }
        }
    }
    private void VolumeChanger()
    {

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Bowl"))
                {
                    SelectedSoundBowl = hit.transform.gameObject;
                    SoundChangerIndicatorText.text = SelectedSoundBowl.name + " (" + SelectedSoundBowl.GetComponent<Bowl>().set + ") ";
                }
                else
                {

                    SoundChangerIndicatorText.text = "BackGound";
                    SelectedSoundBowl = BackgroundMusic;
                }
            }
        }
    }
    private void LoadABowl()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Bowl"))
                {
                    // Disable bowl which is clicked
                    hit.transform.gameObject.SetActive(false);
                    // Place bowl to load on clicked bowl position and enable it
                    Inventory.Instance.allBowls[BowlToLoad].transform.position = hit.transform.position;
                    Inventory.Instance.allBowls[BowlToLoad].transform.gameObject.SetActive(true);

                    // Replace new bowl's index with clicked bowl index in active bowls array
                    int hitItemIndex = Array.FindIndex(Inventory.Instance.allBowls, x => x == hit.transform.GetComponent<Bowl>());
                    BowlsManager.Instance.activeBowlsIndexes[hitItemIndex] = BowlToLoad;

                    //Changing State
                    hit.transform.gameObject.GetComponent<Bowl>().currentState = Item.State.Purchased;
                    Inventory.Instance.allBowls[BowlToLoad].transform.gameObject.GetComponent<Bowl>().currentState = Item.State.Loaded;
                    state = State.Normal;
                    GameManager.Instance.FooterText.text = GameManager.Instance.DefaultFooterText;
                    MenuManager.Instance.currentState = MenuManager.MenuStates.Main;
                }
                else if (hit.transform.gameObject.CompareTag("Bowl2"))
                {
                    Inventory.Instance.allBowls[BowlToLoad].transform.position = hit.transform.position;
                    Inventory.Instance.allBowls[BowlToLoad].transform.gameObject.SetActive(true);

                    int hitItemIndex = hit.transform.GetComponent<indexHolder>().index;
                    BowlsManager.Instance.activeBowlsIndexes[hitItemIndex] = BowlToLoad;
                    Destroy(hit.transform.gameObject);
                    state = State.Normal;
                    Inventory.Instance.allBowls[BowlToLoad].transform.gameObject.GetComponent<Bowl>().currentState = Item.State.Loaded;
                    GameManager.Instance.FooterText.text = GameManager.Instance.DefaultFooterText;
                    MenuManager.Instance.currentState = MenuManager.MenuStates.Main;

                }
                else
                {
                    GameManager.Instance.FooterText.text = "You are Placing the bowl in wrong Place";
                }
            }
        }
    }

    public void SelectModeReposition()
    {
        state = State.RepositionState;
        this.gameObject.GetComponent<BowlReposition>().RepositionBowlInitializer();
        this.gameObject.GetComponent<BowlReposition>().FadeEffect();
        this.gameObject.GetComponent<BowlReposition>().StopEveryThing();
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
