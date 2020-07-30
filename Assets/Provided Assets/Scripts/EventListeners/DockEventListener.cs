using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockEventListener : MonoBehaviour
{
    public class ButtonsData
    {
        public bool replayBG = true;
        public bool changeCamera = true;
        public bool saveSession = true;
    }
    public GameObject[] allButton;

    public void ManageButtons(ButtonsData data)
    {
        if(data == null)
            return;

        allButton[0].SetActive(data.replayBG);
        allButton[1].SetActive(data.changeCamera);
        allButton[2].SetActive(data.saveSession);
    }

    public void OnClickReplayBGButton()
    {

    }

    public void OnClickChangeCameraAngleButton()
    {

    }

    public void OnClickSaveSessionButton()
    {
        PopupManager.Instance.Show("Name Session", SaveBowlPosition);
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
