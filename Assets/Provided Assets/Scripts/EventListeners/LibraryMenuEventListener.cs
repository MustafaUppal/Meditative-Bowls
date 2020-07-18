using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryMenuEventListener : MonoBehaviour
{
    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData 
        { 
            replayBG = false, 
            changeCamera = false, 
            saveSession = false 
        };

        AllRefs.I.dock.ManageButtons(data);
    }

    public void OnClickBackButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
    }
}
