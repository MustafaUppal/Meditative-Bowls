using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenuEventListener : MonoBehaviour
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

    public void OnClickCarpetsButton()
    {

    }

    public void OnClickBowlsButton()
    {

    }

    public void OnClickSoundsButton()
    {

    }
}
