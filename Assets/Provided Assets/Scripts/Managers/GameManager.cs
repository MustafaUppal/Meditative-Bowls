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
    [SerializeField] public GameObject SelectedSoundBowl;

    [SerializeField] public Text SoundChangerIndicatorText;
    public string DefaultFooterText;
    [Header("State")]
    private State state1;
    public float time;
    public float interpolationPeriod = 1;
    public float givenTime;
    public float givenTime1;

    public enum State
    {
        Normal,
        RepositionState,
        Shop,
        RecordingMode,
        Load,
        Remove,
        Sound,
        Alarm,
        Randomization,
        SavingSession,
        Libarary
    }
    public InventoryManager Inventory => InventoryManager.Instance;

    public State State1 
    { 
        get => state1; 
        set 
        { 
            // Debug.Log("GM State: " + state1 + " -> " + value);
            state1 = value;
        } 
    }

    void Start()
    {
        Instance = this;
        DefaultFooterText = FooterText.text;
        // if(Inventory.bowlsManager.activeBowlsIndexes[0] != -1)
        for(int i = 0; i < Inventory.bowlsManager.activeBowlsIndexes.Length; i++)
        {
            if(Inventory.bowlsManager.activeBowlsIndexes[i] != -1)
            {
                SelectedSoundBowl = Inventory.allBowls[Inventory.bowlsManager.activeBowlsIndexes[i]].gameObject;
                break;
            }
        }
    }

    private void Update()
    {
        switch (State1)
        {
            case State.Normal:
                break;
            case State.RepositionState:
                // VolumeChanger();
                // if (SelectedSoundBowl)
                // {
                //     VolumeSlider.onValueChanged.AddListener(delegate { VolumeChange(VolumeSlider.value); });
                //     PanningSlider.onValueChanged.AddListener(delegate { PanningSliderChange(PanningSlider.value); });
                // }

                break;
            case State.RecordingMode:
                break;
            case State.Load:
                // LoadABowl();
                break;
            case State.Sound:

                break;
            case State.Remove:
                // Remove();
                break;
            case State.Alarm:
                break;
            case State.Randomization:

                int RandomBowlIndex = UnityEngine.Random.Range(0, Inventory.bowlsManager.activeBowlsIndexes.Length - 1);
                // Debug.Log("Bwol"+RandomBowlIndex);
                time += Time.deltaTime;
                givenTime1 += Time.deltaTime;

                if (Inventory.bowlsManager.activeBowlsIndexes[RandomBowlIndex] != -1)
                    //if (givenTime1 < givenTime)
                    //{
                    if (time >= interpolationPeriod)
                    {
                        time = 0;
                        interpolationPeriod = UnityEngine.Random.Range(8, 36);
                        Inventory.allBowls[Inventory.bowlsManager.activeBowlsIndexes[RandomBowlIndex]].GetComponent<Bowl>().PlaySound();
                    }
                //}
                break;
        }
    }
    
    [Obsolete]
    public void OnclickBgMusicButton()
    {
        SoundChangerIndicatorText.text = "BackGound";
        SelectedSoundBowl = BackgroundMusic;
    }
    
    public void SoundStop()
    {
        print("SSSSS");
        SelectedSoundBowl.GetComponent<AudioSource>().Stop();
    }

    public void SoundRestart()
    {
        for (int i = 0; i < Inventory.bowlsManager.activeBowlsIndexes.Length; i++)
        {
            if (Inventory.bowlsManager.activeBowlsIndexes[i] != -1)
            {
                Inventory.allBowls[Inventory.bowlsManager.activeBowlsIndexes[i]].AudioSource.Stop();
            }
        }
    }
    
    public void SelectRandomiszation(float Time)
    {
        givenTime = (Time*60);
        print(givenTime);
        State1 = State.Randomization;
    }
    
    public void PanningSliderChange(float SliderValue)
    {
        // Debug.Log("Panning: " + SelectedSoundBowl.name);
        SelectedSoundBowl.GetComponent<AudioSource>().spatialBlend = 0;
        SelectedSoundBowl.GetComponent<AudioSource>().panStereo = SliderValue;
    }

    public void VolumeChange(float Value)
    {
        // Debug.Log("Volume: " + SelectedSoundBowl.name);
        SelectedSoundBowl.GetComponent<AudioSource>().volume = Value;
    }

    [Obsolete]
    public void Remove()
    {
        SelectedSoundBowl.gameObject.GetComponent<Bowl>().CurrentState = Item.State.Purchased;
        GameObject SubsituteGameObject = new GameObject();
        SubsituteGameObject.transform.position = SelectedSoundBowl.transform.gameObject.transform.position;
        SubsituteGameObject.AddComponent<BoxCollider>();
        SubsituteGameObject.AddComponent<Rigidbody>();
        SubsituteGameObject.AddComponent<indexHolder>();
        int index = Array.FindIndex(Inventory.allBowls.ToArray(), x => x == SelectedSoundBowl.transform.GetComponent<Bowl>());
        SubsituteGameObject.GetComponent<indexHolder>().index = index;
        SubsituteGameObject.GetComponent<Rigidbody>().isKinematic = true;
        SelectedSoundBowl.transform.gameObject.SetActive(false);
        SubsituteGameObject.tag = "Bowl2";

        // Removing from active indeces in bowl Manager
        var thisBowlIndex = Array.FindIndex
        (
            InventoryManager.Instance.bowlsManager.activeBowlsIndexes,
            x => x == index
        );
        InventoryManager.Instance.bowlsManager.activeBowlsIndexes[thisBowlIndex] = -1;
    }
    
    public void StopMusic()
    {
        SelectedSoundBowl.GetComponent<AudioSource>().Stop();
    }

    [Obsolete]
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
                    Inventory.allBowls[BowlToLoad].transform.position = hit.transform.position;
                    Inventory.allBowls[BowlToLoad].transform.gameObject.SetActive(true);

                    // Replace new bowl's index with clicked bowl index in active bowls array
                    int hitItemIndex = Array.FindIndex(Inventory.allBowls.ToArray(), x => x == hit.transform.GetComponent<Bowl>());
                    Inventory.bowlsManager.activeBowlsIndexes[hitItemIndex] = BowlToLoad;

                    //Changing State
                    hit.transform.gameObject.GetComponent<Bowl>().CurrentState = Item.State.Purchased;
                    Inventory.allBowls[BowlToLoad].transform.gameObject.GetComponent<Bowl>().CurrentState = Item.State.Loaded;
                    State1 = State.Normal;
                    GameManager.Instance.FooterText.text = GameManager.Instance.DefaultFooterText;
                    MenuManager.Instance.currentState = MenuManager.MenuStates.Main;

                }
                else if (hit.transform.gameObject.CompareTag("Bowl2"))
                {
                    Inventory.allBowls[BowlToLoad].transform.position = hit.transform.position;
                    Inventory.allBowls[BowlToLoad].transform.gameObject.SetActive(true);

                    int hitItemIndex = hit.transform.GetComponent<indexHolder>().index;
                    Inventory.bowlsManager.activeBowlsIndexes[hitItemIndex] = BowlToLoad;
                    Destroy(hit.transform.gameObject);
                    State1 = State.Normal;
                    Inventory.allBowls[BowlToLoad].transform.gameObject.GetComponent<Bowl>().CurrentState = Item.State.Loaded;
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

    [Obsolete]
    public void SelectModeReposition()
    {
        State1 = State.RepositionState;

        this.gameObject.GetComponent<BowlReposition>().RepositionBowlInitializer();
        this.gameObject.GetComponent<BowlReposition>().FadeEffect();
        this.gameObject.GetComponent<BowlReposition>().StopEveryThing();
    }

    public void SelectModeNormal()
    {
        State1 = State.Normal;
    }

    public void SelectModeRecording()
    {
        State1 = State.RecordingMode;
    }
    public void SelectShopModeState()
    {

        State1 = State.Shop;
    }

}
