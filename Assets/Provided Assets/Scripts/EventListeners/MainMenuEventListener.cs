using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuEventListener : MonoBehaviour
{
    public Text Footertext;
    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData { };

        AllRefs.I.dock.ManageButtons(data);
    }
    public void OnClickBackButtonInRepositionMode(){
         GameManager.Instance.GetComponent<BowlReposition>().ResetFuntion();
    }
    void MessageSender(string Message)
    {
        Footertext.text = Message;
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
        GameManager.Instance.state = GameManager.State.Shop;
    }

    public void OnClickRecordingButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Recording);
    }

    public void OnClickAlramButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Alram);
        GameManager.Instance.state=GameManager.State.Alarm;
    }

    public void OnClickSettingsButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Settings);
    }
    public void OnClickRemoveButton()
    {
        GameManager.Instance.Remove();
        GameManager.Instance.state = GameManager.State.Normal;
        GameManager.Instance.GetComponent<BowlReposition>().ResetFuntion();
    }
    public void OnClickStopMusicButton()
    {
        GameManager.Instance.SoundStop();
    }
    public void OnClickBackGroundMusic()
    {

        GameManager.Instance.OnclickBgMusicButton(); ;
    
    }

}
