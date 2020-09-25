using System.Collections;
using System.Collections.Generic;
using MeditativeBowls;
using UnityEngine;

public class HeaderHandler : MonoBehaviour
{

    public enum MenuButtonState
    {
        Open,
        Close,
        Back
    }

    [Header("Animators")]
    Animator animator;
    public Animator a_MenuButton;
    public Animator a_dock;
    public Animator[] buttonSelectors;

    public MenuButtonState menuButtonState;

    // *******************
    // * Unity Callbacks *
    // *******************

    private void Start()
    {
        animator = GetComponent<Animator>();

        if(SceneManager.Instance.currentState.Equals(MenuManager.MenuStates.Main))
            menuButtonState = MenuButtonState.Open;
        else
            menuButtonState = MenuButtonState.Back;

        ChangeState(menuButtonState);
        // Debug.Log("HH");
    }

    // ****************
    // * Button Cicks *
    // ****************
    public void OnClickMenuButton()
    {
        if(AllRefs.I.mainMenu != null)
            AllRefs.I.mainMenu.ManageFooter(false);
        ManageState(true);
    }

    public void OnClickStateButton(int newState)
    {
        if (newState != (int)MenuManager.Instance.currentState)
        {
            MenuManager.Instance.ChangeState((MenuManager.MenuStates)newState);
            // SelectButton();
        }
    }

    // ********************
    //  * Functionalities *
    // ********************

    public void ChangeState(MenuButtonState newState)
    {
        menuButtonState = newState;
        ManageState();

    }

    public void ManageState(bool reverse = false)
    {
        switch (menuButtonState)
        {
            case MenuButtonState.Open: // Means ham burger sign
                if (reverse)
                {
                    menuButtonState = MenuButtonState.Close;
                    ManageState();
                    return;
                }
                EnableHeader(false);
                break;
            case MenuButtonState.Close: // Means cross sign
                if (reverse)
                {
                    menuButtonState = MenuButtonState.Open;
                    ManageState();
                    return;
                }
                EnableHeader(true);
                break;
            case MenuButtonState.Back:
                if (reverse)
                {
                    menuButtonState = MenuButtonState.Close;
                    MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
                    ManageState();
                 
                    // SelectButton();
                    return;
                }
                EnableHeader(true);
                break;
        }

        a_MenuButton.SetInteger("State", (int)menuButtonState);
    }

    public void EnableHeader(bool enable)
    {
        // Debug.Log("Enable: " + enable);
        // string headerAnim = enable ? "Header In" : "Header out";

        // if (!animator.GetCurrentAnimatorStateInfo(0).IsName(headerAnim))
        animator.SetInteger("State", enable ? 1 : 0);


        // if (!a_dock.GetCurrentAnimatorStateInfo(0).IsName(dockAnim))
        a_dock.SetInteger("State", enable ? 1 : 0);

    }

    public void SelectButton()
    {
        int prev = (int)MenuManager.Instance.prevState;
        int current = (int)MenuManager.Instance.currentState;

        if (prev < 6 && prev > 0)
            buttonSelectors[prev - 1].Play("Deselect");
        if (current < 6 && current > 0)
            buttonSelectors[current - 1].Play("Select");

        if (current != 0) // If we are not n main Menu
            ChangeState(MenuButtonState.Back);
    }
}
