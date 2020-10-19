using System.Collections;
using System.Collections.Generic;
using SerializeableClasses;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuEventListener : MonoBehaviour {
    [Header ("References")]
    public MainMenuModes modes;
    public Animator dock;
    public Button slideShowButton;
    public GameObject webContent;


    [Header ("Footer Settings")]
    public Text footertext;
    public GameObject simpleFooter;

    [Header ("Handlers")]
    public SlideShowHandler slideShow;
    public BowlsPlacementHandler bowlsPlacement;
    public RecordingFooter recordingFooter;
    public BowlRandomizationSettings randomizationSettings;

    // *******************
    // * Unity Callbacks *
    // *******************

    private void OnEnable () {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData { };

        AllRefs.I.dock.ManageButtons (data);

        ManageDock (true);

        AllRefs.I.objectSelection.EnableClick (true);
    }

    private void OnDisable () {
        GameManager.Instance.state = GameManager.State.Randomization;
    }

    private void Update () {
        if (randomizationSettings.timer > 0) {
            randomizationSettings.timer -= Time.timeScale;

            if (randomizationSettings.timer <= 0)
                SelectRandomization ();
        }
    }

    // ******************
    // * Buttons Clicks *
    // ******************

    public void SelectRandomization () {
        if (randomizationSettings.timer == 0) {
            randomizationSettings.SetIcon (true);
            AllRefs.I.objectSelection.EnableClick (false);
            randomizationSettings.root.SetActive (true);
            randomizationSettings.hours.SetNumber (0);
            randomizationSettings.mins.SetNumber (2);
            randomizationSettings.secs.SetNumber (0);

        } else {
            randomizationSettings.SetIcon (false);
            randomizationSettings.timer = 0;
            GameManager.Instance.state = GameManager.State.Normal;
        }
    }

    public void OnClickStartRandomization () {
        randomizationSettings.timer = randomizationSettings.TimeLimit;
        GameManager.Instance.state = GameManager.State.Randomization;
        AllRefs.I.objectSelection.EnableClick (true);
        randomizationSettings.root.SetActive (false);
    }

    public void OnClickBackButtonInRepositionMode () {
        GameManager.Instance.GetComponent<BowlReposition> ().ResetFuntion ();
    }

    public void OnClickLoopButton () {
        recordingFooter.SetLoop (false);
    }

    public void OnClickSlideShowButton () {
        if (!InventoryManager.Instance.IsAnySlideShowAvailible ()) {
            PopupManager.Instance.messagePopup.Show ("Not Purchased!", "Please purchase slideshow from shop to unlock this feature.");
            return;
        }

        // if in other mode don't allow this mode to work
        if (modes.placeBowls)
            return;

        modes.slideShow = !modes.slideShow;
        AllRefs.I.objectSelection.EnableClick (!modes.slideShow);
        slideShow.EnableSlideShow (modes.slideShow);
    }

    public void OnClickPlaceBowlsButton () {
        if (modes.slideShow)
            return;

        modes.placeBowls = !modes.placeBowls;
        AllRefs.I.objectSelection.EnableClick (!modes.placeBowls);
        bowlsPlacement.Enable (modes.placeBowls);
    }

    public void OnClickOpenLinkButton(string url)
    {
        Application.OpenURL(url);
    }

    public void OnClickEnableWebContent(bool enable)
    {
        webContent.SetActive(enable);
    }

    // *******************
    // * Functionalities *
    // *******************

    public void ManageFooter (bool val) {
        modes.playingRecording = val;
        simpleFooter.SetActive (!modes.playingRecording);
        recordingFooter.root.SetActive (modes.playingRecording);
    }

    public void ManageDock (bool enable) {
        dock.SetInteger ("State", enable ? 1 : 0);
    }

    void MessageSender (string Message) {
        footertext.text = Message;
    }
}