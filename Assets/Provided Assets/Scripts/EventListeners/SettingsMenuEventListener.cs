using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuEventListener : MonoBehaviour
{
    public Text Status;
    public GameObject FooterPanel;
    public GameObject StatusText;

    void MessageSender(string Message)
    {

        Status.text = Message;
    }
    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData
        {
            saveSession = false
        };
        GameManager.Instance.VolumeSlider.gameObject.SetActive(false);
        AllRefs.I.dock.ManageButtons(data);
    }

    public void OnClickBackButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
        GameManager.Instance.SelectModeNormal();
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().ResetFuntion();
        GameManager.Instance.VolumeSlider.gameObject.SetActive(false);
        MessageSender("Select an option to for setting up environment");
    }
    public void OnClickRemoveButton()
    {
        GameManager.Instance.Remove();
        Debug.Log("normal");
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
