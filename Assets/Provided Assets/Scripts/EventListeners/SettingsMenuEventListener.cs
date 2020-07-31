using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuEventListener : MonoBehaviour
{
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

    }
    public void OnClickRepositionButton()
    {
        GameManager.Instance.SelectModeReposition();
        GameManager.Instance.VolumeSlider.gameObject.SetActive(false);
    }

    public void OnClickRemoveButton()
    {

        GameManager.Instance.state = GameManager.State.Remove;
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().ResetFuntion();
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().StopEveryThing();
        GameManager.Instance.VolumeSlider.gameObject.SetActive(false);

    }

    public void OnClickVolumeButton()
    {
        GameManager.Instance.state = GameManager.State.Sound;
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().StopEveryThing();
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().ResetFuntion();
        GameManager.Instance.VolumeSlider.gameObject.SetActive(true);


    }
}
