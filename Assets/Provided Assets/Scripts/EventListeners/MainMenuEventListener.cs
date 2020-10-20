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
    public GameObject webContent;


    [Header("Footer Settings")]
    public Text footertext;
    public GameObject simpleFooter;
    public Animator footerAnim;

    [Header("Handlers")]
    public SlideShowHandler slideShow;
    public BowlsPlacementHandler bowlsPlacement;
    public RecordingFooter recordingFooter;
    public BowlRandomizationSettings randomizationSettings;

    // *******************
    // * Unity Callbacks *
    // *******************

    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData { };

        AllRefs.I.dock.ManageButtons(data);

        AllRefs.I.objectSelection.EnableClick(true);
    }

    private void OnDisable()
    {
        OnClickStartRandomization(false);
    }

    private void Start() {
        ManageDock(false);
        EnableFooter(false);
    }

    private void Update()
    {
        if (randomizationSettings.isStarted && randomizationSettings.TimeLimit < randomizationSettings.stopwatch.Elapsed.TotalSeconds)
        {
            OnClickStartRandomization(false);
        }
    }

    // ******************
    // * Buttons Clicks *
    // ******************

    public void OnClickOpenRandomizationPanelButton()
    {
        if (!randomizationSettings.isStarted)
        {
            randomizationSettings.root.SetActive(true);
            AllRefs.I.objectSelection.EnableClick(false);

            randomizationSettings.hours.SetNumber(0);
            randomizationSettings.mins.SetNumber(2);
            randomizationSettings.secs.SetNumber(0);
        }
        else
            OnClickStartRandomization(false);
    }

    public void OnClickStartRandomization(bool start)
    {
        randomizationSettings.isStarted = start;

        if (start)
        {
            randomizationSettings.SetIcon(true);
            randomizationSettings.stopwatch.Start();

            GameManager.Instance.State1 = GameManager.State.Randomization;
            AllRefs.I.objectSelection.EnableClick(true);
            randomizationSettings.root.SetActive(false);
        }
        else
        {
            randomizationSettings.stopwatch.Stop();
            randomizationSettings.stopwatch.Reset();

            randomizationSettings.SetIcon(false);
            randomizationSettings.timer = 0;
            GameManager.Instance.State1 = GameManager.State.Normal;
        }
    }

    public void OnClickBackButtonInRepositionMode()
    {
        GameManager.Instance.GetComponent<BowlReposition>().ResetFuntion();
    }

    public void OnClickLoopButton()
    {
        recordingFooter.SetLoop(false);
    }

    public void OnClickSlideShowButton()
    {
        if (!InventoryManager.Instance.IsAnySlideShowAvailible())
        {
            PopupManager.Instance.messagePopup.Show("Not Purchased!", "Please purchase slideshow from shop to unlock this feature.");
            return;
        }

        // if in other mode don't allow this mode to work
        if (modes.placeBowls)
            return;

        modes.slideShow = !modes.slideShow;
        AllRefs.I.objectSelection.EnableClick(!modes.slideShow);
        slideShow.EnableSlideShow(modes.slideShow);
    }

    public void OnClickPlaceBowlsButton()
    {
        if (modes.slideShow)
            return;

        modes.placeBowls = !modes.placeBowls;
        AllRefs.I.objectSelection.EnableClick(!modes.placeBowls);
        bowlsPlacement.Enable(modes.placeBowls);
    }

    public void OnClickOpenLinkButton(string url)
    {
        if (url.Length != 0)
            Application.OpenURL(url);
    }

    public void OnClickEnableWebContent(bool enable)
    {
        AllRefs.I.objectSelection.EnableClick(!enable);
        webContent.SetActive(enable);
    }

    // *******************
    // * Functionalities *
    // *******************

    public void ManageFooter(bool val)
    {
        modes.playingRecording = val;
        simpleFooter.SetActive(!modes.playingRecording);
        recordingFooter.root.SetActive(modes.playingRecording);
    }

    public void EnableFooter(bool enable)
    {
        footerAnim.SetInteger("State", enable ? 1 : 0);
    }

    public void ManageDock(bool enable)
    {
        dock.SetInteger("State", enable ? 1 : 0);
    }

    void MessageSender(string Message)
    {
        footertext.text = Message;
    }
}