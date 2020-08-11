using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("Items")]
    public Bowl[] allBowls;
    public Carpet[] allCarpets;
    public BG_Music[] allMusics;

    [Header("3D Imagess")]
    public GameObject bowl;
    public GameObject carpet;

    // public Carpet[] AllCarpets { get => allCarpets; set { Debug.Log("allCarpets"); allCarpets = value; } }

    private void Awake()
    {
        Instance = this;
    }

    public void Manage3DItems(int currentState, int materialIndex)
    {
        carpet.SetActive(currentState.Equals(0));
        bowl.SetActive(currentState.Equals(1));

        switch (currentState)
        {
            case 0:
                carpet.GetComponent<Renderer>().material = allCarpets[materialIndex].material;
                break;
            case 1:
                bowl.GetComponent<Renderer>().material = allBowls[materialIndex].material;
                break;
        }
    }

    public Item GetItem(int itemType, int index)
    {
        switch (itemType)
        {
            case 0:
                return allCarpets[index].GetComponent<Item>();
            case 1:
                return allBowls[index].GetComponent<Item>();
            case 2:
                return allMusics[index].GetComponent<Item>();
            default:
                return null;
        }
    }

    public int GetItemCount(int itemType)
    {
        switch (itemType)
        {
            case 0:
                return allCarpets.Length;
            case 1:
                return allBowls.Length;
            case 2:
                return allMusics.Length;
            default:
                return 0;
        }
    }
}