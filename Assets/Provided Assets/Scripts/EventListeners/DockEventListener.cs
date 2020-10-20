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

    public void ManageButtons(ButtonsData data)
    {
        if(data == null)
            return;

        allButtons[0].interactable = data.replayBG;
        allButtons[1].interactable = data.changeCamera;
        allButtons[2].interactable = data.saveSession;
    }

    public void OnClickReplayBGButton()
    {
        AllRefs.I._GameManager.SoundRestart();
    }

    public void OnClickChangeCameraAngleButton()
    {
        cameraPosition++;
        if(cameraPosition > 3) cameraPosition = 0;
        // AllRefs.I._GameManager.OnclickBgMusicButton();
        camera.SetInteger("State", cameraPosition);
    }
    

    public void OnClickSaveSessionButton()
    {
        PopupManager.Instance.Show("Save Bowl Arrangement", SaveBowlPosition);

        AllRefs.I._GameManager.State1 = GameManager.State.SavingSession;

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
