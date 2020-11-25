using System;
using System.Collections;
using System.Collections.Generic;
using MeditativeBowls;
using UnityEngine;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    public enum MenuStates
    {
        Main,
        Library,
        Shop,
        Alram,
        Recording,
        Settings,
        SavingSession,
        BowlPlacement
    }

    public static MenuManager Instance;

    public MenuStates defaultState;
    public MenuStates currentState;
    public MenuStates prevState;

    public GameObject[] AllPanels;
    public HelpMenuEventListener helpMenu;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        currentState = SceneManager.Instance.prevState;
        ChangeState(SceneManager.Instance.currentState);
    }

    public void ChangeState(MenuStates newState)
    {
        prevState = currentState;
        currentState = newState;

        // print(prevState);
        // print(currentState);

        SceneManager.Instance.prevState = prevState;
        SceneManager.Instance.currentState = currentState;

        
        if (!prevState.Equals(MenuStates.Shop))
            AllPanels[(int)prevState].SetActive(false);
        else if (!SceneManager.Instance.IsSceneLoaded(1)) // Load Scene if shop
            SceneManager.Instance.LoadScene(1);

        if (!currentState.Equals(MenuStates.Shop))
            AllPanels[(int)currentState].SetActive(true);
        else if (!SceneManager.Instance.IsSceneLoaded(2)) // Load Scene if shop
            SceneManager.Instance.LoadScene(2);

        if (prevState == MenuStates.Settings && !(GameManager.Instance.State1 == GameManager.State.RepositionState))
        {
            if(GameManager.Instance.gameObject)
            GameManager.Instance.gameObject.GetComponent<BowlReposition>().ResetFuntion();
        }
        AllRefs.I.headerHandler.SelectButton();

        // Debug.Log("MM");
    }

    public void EnableHelp(bool enable)
    {
        helpMenu.gameObject.SetActive(enable);
        helpMenu.Enable(enable);
    }
}
