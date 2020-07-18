using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuEventListener : MonoBehaviour
{
    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData
        {
            saveSession = false
        };

        AllRefs.I.dock.ManageButtons(data);
    }

    public void OnClickBackButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
    }

    public void OnClickRepositionButton()
    {

    }

    public void OnClickRemoveButton()
    {

    }

    public void OnClickVolumeButton()
    {

    }
}
