using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        if (!prevState.Equals(MenuStates.Shop))
            AllPanels[(int)prevState].SetActive(false);
        else if(!SceneManager.GetActiveScene().buildIndex.Equals(0))
            SceneManager.LoadScene(0);

        if (!currentState.Equals(MenuStates.Shop))
            AllPanels[(int)currentState].SetActive(true);
        else if(!SceneManager.GetActiveScene().buildIndex.Equals(1))
            SceneManager.LoadScene(1);

        ApplyChanges();
    }

    private void ApplyChanges()
    {

    }
}
