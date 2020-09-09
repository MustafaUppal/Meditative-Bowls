using System.Collections;
using System.Collections.Generic;
using SerializeableClasses;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuEventListener : MonoBehaviour
{
    public bool playingMode;
    public bool slideShowMode;
    public Animator dock;

    public SlideShowHandler slideShow;
    public RecordingFooter recordingFooter;
    
    public Text Footertext;
    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData { };

        AllRefs.I.dock.ManageButtons(data);

        ManageDock(true);
        
    }

    public void ManageFooter(bool val)
    {
        playingMode = val;
        Footertext.gameObject.SetActive(!playingMode);
        recordingFooter.root.SetActive(playingMode);
    }

    public void ManageDock(bool enable)
    {
        dock.SetInteger("State", enable ? 1 : 0);
    }

    public void OnClickBackButtonInRepositionMode(){
         AllRefs.I._GameManager.GetComponent<BowlReposition>().ResetFuntion();
    }
    void MessageSender(string Message)
    {
        Footertext.text = Message;
    }

    // public void OnClickBackButton()
    // {
    //     Application.Quit();
    // }

    // public void OnClickLibraryButton()
    // {
    //     MenuManager.Instance.ChangeState(MenuManager.MenuStates.Library);
    // }

    // public void OnClickShopButton()
    // {
    //     MenuManager.Instance.ChangeState(MenuManager.MenuStates.Shop);
    //     AllRefs.I._GameManager.State = AllRefs.I._GameManager.State.Shop;
    // }

    // public void OnClickRecordingButton()
    // {
    //     MenuManager.Instance.ChangeState(MenuManager.MenuStates.Recording);
    // }

    // public void OnClickAlramButton()
    // {
    //     MenuManager.Instance.ChangeState(MenuManager.MenuStates.Alram);
    //     AllRefs.I._GameManager.State=AllRefs.I._GameManager.State.Alarm;
    // }

    public void OnClickLoopButton(bool increase)
    {
        recordingFooter.UpdateLoopCount(increase ? 1 : -1);
    }

    public void OnClickStartSlideShowButton()
    {
        slideShowMode = !slideShowMode;
        slideShow.StartSlideShow(slideShowMode);
    }
}
