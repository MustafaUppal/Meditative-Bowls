using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RecordingMenuEventListener : MonoBehaviour
{
    public Text Footertext;
    void MessageSender(string Message)
    {
        Footertext.text = Message;
    }
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
