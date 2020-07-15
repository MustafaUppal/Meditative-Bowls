using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItems
{

    public List<Sprite> Item;
    public List<int> price;
    public List<string> Set_Name;
    public List<string> ItemName;
}
                                                   
public class Inventory : MonoBehaviour
{

    #region Data

    [Header("Data Region")]
    [SerializeField]
    private InventoryItems SelectedList;

    [SerializeField]

    private int CurrentIndex = -1;
    [SerializeField]
    private InventoryItems BowlData;

    [SerializeField]
    private InventoryItems BackGroundMusicData;
    #endregion
    [SerializeField]
    private InventoryItems CarpetData;

    #region VariableToShowData 

    [Header("Data Show Variabe")]
    [SerializeField]
    private Image ItemImage;
    [SerializeField]
    private TextMeshProUGUI ItemName;
    [SerializeField]
    private TextMeshProUGUI SetName;
    [SerializeField]
    private TextMeshProUGUI Price;
    #endregion

    #region Swipe Detection
    public float swipeThreshold = 50f;
    public float timeThreshold = 0.3f;

    private Vector2 fingerDown;
    private DateTime fingerDownTime;
    private Vector2 fingerUp;
    private DateTime fingerUpTime;
  
    void Start()
    {
        
    } 
    private void Update()
    {
        if (GameManager.Instance.state == GameManager.State.Shop)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.fingerDown = Input.mousePosition;
                this.fingerUp = Input.mousePosition;
                this.fingerDownTime = DateTime.Now;
            }
            if (Input.GetMouseButtonUp(0))
            {
                this.fingerDown = Input.mousePosition;
                this.fingerUpTime = DateTime.Now;
                this.CheckSwipe();
            }
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    this.fingerDown = touch.position;
                    this.fingerUp = touch.position;
                    this.fingerDownTime = DateTime.Now;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    this.fingerDown = touch.position;
                    this.fingerUpTime = DateTime.Now;
                    this.CheckSwipe();
                }
            }

        }
    }
    private void CheckSwipe()
    {
        float duration = (float)this.fingerUpTime.Subtract(this.fingerDownTime).TotalSeconds;
        if (duration > this.timeThreshold) return;

        float deltaX = this.fingerDown.x - this.fingerUp.x;

        if (Mathf.Abs(deltaX) > this.swipeThreshold)
        {
            if (deltaX > 0)
            {
                Debug.Log("right");
                Next();
            }
            else if (deltaX < 0)
            {
                Debug.Log("left");
                Back();
            }
        }

    }
    #endregion 
    void Next()
    {
        if (CurrentIndex != -1 && CurrentIndex != SelectedList.Item.Capacity - 1)
            CurrentIndex++;
        else
            CurrentIndex = 0;
        ShowItems(CurrentIndex);
    }
    void Back()
    {
        if (CurrentIndex != 0)
            CurrentIndex--;
        else
            CurrentIndex = SelectedList.Item.Capacity - 1;
        ShowItems(CurrentIndex);
    }

    public void ShowItems(int INDEX)
    {
        ItemImage.sprite = SelectedList.Item[INDEX];
        Price.text = SelectedList.price[INDEX].ToString();
        SetName.text = SelectedList.Set_Name[INDEX];
        ItemName.text = SelectedList.ItemName[INDEX];
    }
    public void SelectCarpet()
    {
        GameManager.Instance.SelectedItem = "Carpet";
        int Capacity = CarpetData.Item.Capacity;
        print(Capacity);
        CarpetData.Item.Capacity = Capacity;
        CarpetData.ItemName.Capacity = Capacity;
        CarpetData.price.Capacity = Capacity;
        CarpetData.Set_Name.Capacity = Capacity;

        SelectedList = CarpetData;
        ItemImage.sprite = CarpetData.Item[0];
        Price.text = CarpetData.price[0].ToString();
        SetName.text = CarpetData.Set_Name[0];
        ItemName.text = CarpetData.ItemName[0];
    }
    public void SelectBackgound()
    {
        GameManager.Instance.SelectedItem = "Bowl";
        int Capacity = BackGroundMusicData.Item.Capacity;
        BackGroundMusicData.Item.Capacity = Capacity;
        BackGroundMusicData.ItemName.Capacity = Capacity;
        BackGroundMusicData.price.Capacity = Capacity;
        BackGroundMusicData.Set_Name.Capacity = Capacity;
        SelectedList = BackGroundMusicData;
        ItemImage.sprite = BackGroundMusicData.Item[0];
        Price.text = BackGroundMusicData.price[0].ToString();
        SetName.text = BackGroundMusicData.Set_Name[0];
        ItemName.text = BackGroundMusicData.ItemName[0];
    }
    public void SelectBowl()
    {
        GameManager.Instance.SelectedItem = "Bowl";

        int Capacity = BowlData.Item.Capacity;
        BowlData.Item.Capacity = Capacity;
        BowlData.ItemName.Capacity = Capacity;
        BowlData.price.Capacity = Capacity;
        BowlData.Set_Name.Capacity = Capacity;
        SelectedList = BowlData;
        ItemImage.sprite = BowlData.Item[0];
        Price.text = BowlData.price[0].ToString();
        SetName.text = BowlData.Set_Name[0];
        ItemName.text = BowlData.ItemName[0];

    }
    void Load(Material mat)
    {
        foreach (GameObject Obj in GameManager.Instance.BowlArray)
        {

        }
    }
}