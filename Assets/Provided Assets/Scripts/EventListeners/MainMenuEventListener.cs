using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuEventListener : MonoBehaviour
{
    public bool playingMode;
    public bool slideShowMode;
    public Animator dock;

    public SlideShowHandler slideShow;

    [System.Serializable]
    public class RecordingFooter
    {
        public GameObject root;
        public int loopCount = 0;
        public int currentLoop = 1;
        public Text loopCountText;
        public Button minusButton;
        public Image timerFill;
        public Text timer;

        public void Enable(bool enable)
        {
            root.gameObject.SetActive(true);
            minusButton.interactable = false;
        }

        public void UpdateLoopCount(int value, bool useExact = false)
        {
            loopCount = useExact ? value : loopCount + value;

            minusButton.interactable = loopCount > currentLoop;
            loopCountText.text = loopCount.ToString();
        }

        public void UpdateTimer(float percentage, int timer)
        {
            timerFill.fillAmount = percentage;

            int mins = timer/60;
            int secs = timer - mins*60;

            this.timer.text = mins + ":" + (secs < 10 ? "0" + secs : secs.ToString());
        }

        public void InitLoopCount(int val)
        {
            loopCount = currentLoop = val;
        }
    }

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
        slideShowMode = !slideShowMode;
        slideShow.StartSlideShow(slideShowMode);
    }
}
