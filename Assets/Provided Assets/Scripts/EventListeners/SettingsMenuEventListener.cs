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
        GameManager.Instance.SelectModeNormal();
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().ResetFuntion();
    }

    public void OnClickRepositionButton()
    {
     GameManager.Instance.SelectModeReposition();
    }

    public void OnClickRemoveButton()
    {

    }

    public void OnClickVolumeButton()
    {

    }
}
