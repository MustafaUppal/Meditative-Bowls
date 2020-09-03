using System.Collections;
using System.Collections.Generic;
using SerializeableClasses;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuEventListener : MonoBehaviour
{
    public Text Status;
    public GameObject FooterPanel;
    //public static SettingsMenuEventListener Instance;
    public BowlEditingSettings bowlEditingSettings;

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
        //GameManager.Instance.VolumeSlider.gameObject.SetActive(false);
        AllRefs.I.dock.ManageButtons(data);
    }
    private void Start()
    {
        //Instance = this;
    }
    public void ManageFooter(bool isEditingBowl)
    {
        print(!isEditingBowl);
        print(isEditingBowl);
        bowlEditingSettings.root.SetActive((isEditingBowl));
        FooterPanel.SetActive(!isEditingBowl);
    }

    public void OnClickBackButton()
    {
        //SettingsMenuEventListener.Instance.ManageFooter(false);
        AllRefs.I.settingMenu.ManageFooter(false);

        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
        GameManager.Instance.SelectModeNormal();
        GameManager.Instance.VolumeSlider.gameObject.SetActive(false);
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().ResetFuntion();
        
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
