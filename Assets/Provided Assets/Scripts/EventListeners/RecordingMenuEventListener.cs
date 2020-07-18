using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingMenuEventListener : MonoBehaviour
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

    public void OnClickRecordButton()
    {

    }

    public void OnClickSaveButton()
    {

    }
}
