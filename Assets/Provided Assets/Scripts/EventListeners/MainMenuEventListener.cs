using System.Collections;
using System.Collections.Generic;
using SerializeableClasses;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuEventListener : MonoBehaviour
{
    [Header("References")]
    public MainMenuModes modes;
    public Animator dock;
    public Button slideShowButton;
    public Text Footertext;

    [Header("Handlers")]
    public SlideShowHandler slideShow;
    public BowlsPlacementHandler bowlsPlacement;
    public RecordingFooter recordingFooter;

    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData { };

        AllRefs.I.dock.ManageButtons(data);

        ManageDock(true);

        slideShowButton.interactable = PlayerPreferencesManager.GetSlideShowPurchedState(false);
    }

    public void ManageFooter(bool val)
    {
        modes.playingRecording = val;
        Footertext.gameObject.SetActive(!modes.playingRecording);
        recordingFooter.root.SetActive(modes.playingRecording);
    }

    public void ManageDock(bool enable)
    {
        dock.SetInteger("State", enable ? 1 : 0);
    }

    public void OnClickBackButtonInRepositionMode(){
         GameManager.Instance.GetComponent<BowlReposition>().ResetFuntion();
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
    //     GameManager.Instance.state = GameManager.State.Shop;
    // }

    // public void OnClickRecordingButton()
    // {
    //     MenuManager.Instance.ChangeState(MenuManager.MenuStates.Recording);
    // }

    // public void OnClickAlramButton()
    // {
    //     MenuManager.Instance.ChangeState(MenuManager.MenuStates.Alram);
    //     GameManager.Instance.state=GameManager.State.Alarm;
    // }

    public void OnClickLoopButton(bool increase)
    {
        recordingFooter.UpdateLoopCount(increase ? 1 : -1);
    }

    public void OnClickStartSlideShowButton()
    {
        if(modes.placeBowls)
            return;

        modes.slideShow = !modes.slideShow;
        slideShow.StartSlideShow(modes.slideShow);
    }

    public void OnClickPlaceBowlsButton()
    {
        if(modes.slideShow)
            return;
            
        modes.placeBowls = !modes.placeBowls;
        bowlsPlacement.Enable(modes.placeBowls);
    }
}
