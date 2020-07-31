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
    public void OnClickRepositionButton()
    {
        MessageSender("Reposition");
        GameManager.Instance.SelectModeReposition();
        GameManager.Instance.VolumeSlider.gameObject.SetActive(false);
        StatusText.SetActive(true);


    }

    public void OnClickRemoveButton()
    {

        MessageSender("Remove");
        StatusText.SetActive(true);

        GameManager.Instance.state = GameManager.State.Remove;
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().ResetFuntion();
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().StopEveryThing();
        GameManager.Instance.VolumeSlider.gameObject.SetActive(false);
    }

    public void OnClickVolumeButton()
    {
        MessageSender("Volume");
        GameManager.Instance.state = GameManager.State.Sound;
        StatusText.SetActive(false);
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().StopEveryThing();
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().ResetFuntion();
        GameManager.Instance.VolumeSlider.gameObject.SetActive(true);

    }
}
