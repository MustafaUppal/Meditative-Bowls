using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DockEventListener : MonoBehaviour
{
    public new Animator camera;
    public int cameraPosition = 2;
    public class ButtonsData
    {
        public bool replayBG = true;
        public bool changeCamera = true;
        public bool saveSession = true;
    }
    public Button[] allButtons;
    public GameObject[] offPanels;

    public void ManageButtons(ButtonsData data)
    {
        if(data == null)
            return;

        allButtons[0].interactable = data.replayBG;
        offPanels[0].SetActive(!data.replayBG);

        allButtons[1].interactable = data.changeCamera;
        offPanels[1].SetActive(!data.changeCamera);

        allButtons[2].interactable = data.saveSession;
        offPanels[2].SetActive(!data.saveSession);
    }

    public void OnClickReplayBGButton()
    {
        if(AllRefs.I.mainMenu.modes.playingRecording)
            AllRefs.I.mainMenu.ManageFooter(false);

        GameManager.Instance.SoundRestart();
    }

    public void OnClickChangeCameraAngleButton()
    {
        cameraPosition++;
        if(cameraPosition > 3) cameraPosition = 0;
        // GameManager.Instance.OnclickBgMusicButton();
        camera.SetInteger("State", cameraPosition);
    }
    

    public void OnClickSaveSessionButton()
    {
        PopupManager.Instance.Show("Save Bowl Arrangement", SaveBowlPosition);

        GameManager.Instance.State1 = GameManager.State.SavingSession;

    }

    void SaveBowlPosition(string name)
    {
        string status = SessionManager.Instance.ValidateSessionName(name);

        if (status.Equals("Pass"))
        {
            SessionManager.Instance.SaveSession(name);
            PopupManager.Instance.Hide();
        }
        else
            PopupManager.Instance.ShowError(status);
    }
}
