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

        AllRefs.I.objectSelection.EnableClick(true);
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
    public void SelectRandomization()
    {
        if (!(GameManager.Instance.state == GameManager.State.Randomization))
            GameManager.Instance.state = GameManager.State.Randomization;
        else
            GameManager.Instance.state = GameManager.State.Normal;


    }
    public void OnClickBackButtonInRepositionMode(){
         GameManager.Instance.GetComponent<BowlReposition>().ResetFuntion();
    }
    void MessageSender(string Message)
    {
        Footertext.text = Message;
    }

   

    public void OnClickLoopButton(bool increase)
    {
        recordingFooter.UpdateLoopCount(increase ? 1 : -1);
    }

    public void OnClickStartSlideShowButton()
    {
        // if slide show is not purcased
        if(!PlayerPreferencesManager.GetPurchasedState(2, 0, false))
        {
            PopupManager.Instance.messagePopup.Show("Not Purchased!", "Please purchase slideshow from shop to unlock this feature.");
            return;
        }

        if(modes.placeBowls)
            return;

        AllRefs.I.objectSelection.EnableClick(modes.slideShow);    
        modes.slideShow = !modes.slideShow;
        AllRefs.I.objectSelection.EnableClick(!modes.slideShow);    
        slideShow.StartSlideShow(modes.slideShow);
    }

    public void OnClickPlaceBowlsButton()
    {
        if(modes.slideShow)
            return;
            
        modes.placeBowls = !modes.placeBowls;
        AllRefs.I.objectSelection.EnableClick(!modes.placeBowls);    
        bowlsPlacement.Enable(modes.placeBowls);
    }
}
