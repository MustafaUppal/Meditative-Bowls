﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    public enum MenuStates
    {
        Main,
        Library,
        Shop,
        Recording,
        Alram,
        Settings
    }

    public static MenuManager Instance;

    public MenuStates defaultState;
    public MenuStates currentState;
    public MenuStates prevState;

    public GameObject[] AllPanels;
 
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        ChangeState(defaultState);
    }

    public void ChangeState(MenuStates newState)
    {
        prevState = currentState;
        currentState = newState;

        AllPanels[(int)prevState].SetActive(false);
        AllPanels[(int)currentState].SetActive(true);
    }
}
