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

        SceneManager.Instance.prevState = prevState;
        SceneManager.Instance.currentState = currentState;
        
        if (!prevState.Equals(MenuStates.Shop))
            AllPanels[(int)prevState].SetActive(false);
        else if(!SceneManager.Instance.IsSceneLoaded(1))
            SceneManager.Instance.LoadScene(1);

        if (!currentState.Equals(MenuStates.Shop))
            AllPanels[(int)currentState].SetActive(true);
        else if(!SceneManager.Instance.IsSceneLoaded(2))
            SceneManager.Instance.LoadScene(2);

        // if (prevState.Equals(MenuStates.Settings)&&!AllRefs.I.settingMenu.bowlEditingSettings.root.activeInHierarchy)
        // {
        //     print("Pakistan");
        //     AllRefs.I.settingMenu.ManageFooter(false);
        //     AllRefs.I.settingMenu.OnClickBackButton();
        // }
        // if (currentState.Equals(MenuStates.Library))
        // {
        //     AllRefs.I._GameManager.state = GameManager.State.Libarary;
        // }
        // if (currentState.Equals(MenuStates.Main))
        // {
        //     try
        //     {
        //         AllRefs.I._GameManager.SelectModeNormal();
        //     }
        //     catch
        //     {

        //     }
        // }
        // if (AllRefs.I._GameManager.state.Equals(GameManager.State.Randomization))
        // {
        //     try
        //     {
        //         AllRefs.I._GameManager.SelectModeNormal();
        //     }
        //     catch
        //     {

        //     }
        // }
        ApplyChanges();
        AllRefs.I.headerHandler.SelectButton();

        // Debug.Log("MM");
    }

    IEnumerator ChangeStateE(MenuStates newState)
    {
        prevState = currentState;
        currentState = newState;
        yield return null;
    }

    private void ApplyChanges()
    {

    }

}
