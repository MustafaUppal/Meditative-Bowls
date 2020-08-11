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

    // public Carpet[] AllCarpets { get => allCarpets; set { Debug.Log("allCarpets"); allCarpets = value; } }

    private void Awake()
    {
        Instance = this;
    }
}