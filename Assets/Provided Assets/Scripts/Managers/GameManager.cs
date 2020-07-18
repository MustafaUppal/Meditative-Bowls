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
    public State state;

    [Header("SubInventory")]
    #region Subinformationinventory
    public GameObject[] carpetData;
    public GameObject[] BowlData;
    public GameObject[] BackGroundMusicData;
    public string SelectedItem;


    public GameObject SubInformationInventoryPanel;
    public TextMeshProUGUI ItemnameSlot;
    public TextMeshProUGUI priceSlot;
    public TextMeshProUGUI CategorySlot;
    public Image ShowItemImageSlot;

    public GameObject GridParent;
    #endregion

    public void LoadItemInSubInventory()
    {
        switch (SelectedItem)
        {

            case "Carpet":
                CarpetDataShow();
                break;

            case "Bowl":
                BowlDataShow();
                break;

            case "BackGroundMusicvData":
                BackGroundDataShow();
                break;

        }
    }
    public void CarpetDataShow()
    {
        for (int i = 0; i < BowlData.Length; i++)
        {
            Instantiate(carpetData[i], GridParent.transform);
        }
    }
    public void BowlDataShow()
    {
        for (int i = 0; i < BowlData.Length; i++)
        {
            Instantiate(BowlData[i], GridParent.transform);
        }
    }
    public void BackGroundDataShow()
    {
        for (int i = 0; i < BowlData.Length; i++)
        {
            Instantiate(BackGroundMusicData[i], GridParent.transform);
        }
    }
    public void BackFromSubInventory()
    {
        for (int i = 0; i < GridParent.transform.childCount; i++)
        {
            Destroy(GridParent.transform.GetChild(i));
        }
    }
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
                break;
        }
    }
    public void SelectModeReposition()
    {

        state = State.RepositionState;
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
