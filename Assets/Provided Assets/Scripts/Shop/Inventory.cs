using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[System.Serializable]
public class ItemData
{
    public Sprite ItemImage;
    public string ItemName;
    [TextArea(2, 4)]
    public string itemDescription;
}
                                                   
public class Inventory : MonoBehaviour
{
    public GameObject[] AllPanels;
    public Image ItemImage;
    public Text ItemName;
    public Text ItemDescription;

    [Header("All_Data")]
    public Bowl[] BowlInventoryItems;


    [Header("Carpet Data")]

    public Bowl[] Carpet;
    
    [Header("BackGround Sound")]
    public Bowl[] BackGroundSound;


    [Header("Bowl")]
    public GameObject[] BowlDataButtons;
    [Header("Carpet")]
   
    public GameObject[] CarpetsDataButtons;
    [Header("BackGround Sound")]
    
    public GameObject[] BackGroundSoundDataButtons;
    
    
    public static Inventory Instance;

    [Header("All_Data Show")]
    public GameObject CarpetScrollView;
    public GameObject BackGroundMusicScrollView;
    public GameObject BowlView;

 
    
    [Header("List")]
    public GameObject ParentOfList;
    public Button Button;

    private void Start()
    { 
        Instance = this;
    }
    public void ShowBowlInAList()
    {

    }
    public void ShowBowlInAList(ItemData Data)
    {
        
    }
    
}