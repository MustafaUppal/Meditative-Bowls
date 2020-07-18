using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEventListener : MonoBehaviour
{
    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData{};

        AllRefs.I.dock.ManageButtons(data);
    }

    public void OnClickBackButton()
    {
        Application.Quit();
    }

    public void OnClickLibraryButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Library);
    }

    public void OnClickShopButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Shop);
    }

    public void OnClickRecordingButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Recording);
    }

    public void OnClickAlramButton()
    {

    }

    public void OnClickSettingsButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Settings);
    }
}
