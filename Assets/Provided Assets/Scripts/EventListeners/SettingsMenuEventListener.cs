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
    public InputField GivenTimeInputField;

    public void SelectRandomization()
    {
        if(GivenTimeInputField.text!=""&& GivenTimeInputField.text != null)
        AllRefs.I._GameManager.SelectRandomiszation(float.Parse(GivenTimeInputField.text));
    }
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
        //AllRefs.I._GameManager.VolumeSlider.gameObject.SetActive(false);
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
        AllRefs.I._GameManager.SelectModeNormal();
        AllRefs.I._GameManager.VolumeSlider.gameObject.SetActive(false);
        AllRefs.I._GameManager.gameObject.GetComponent<BowlReposition>().ResetFuntion();
        
        MessageSender("Select an option to for setting up environment");
    }
    public void OnClickRemoveButton()
    {
        AllRefs.I._GameManager.Remove();
        Debug.Log("normal");
        AllRefs.I._GameManager.state = GameManager.State.Normal;
        AllRefs.I._GameManager.GetComponent<BowlReposition>().ResetFuntion();
    }
    public void OnClickStopMusicButton()
    {
        AllRefs.I._GameManager.SoundStop();
    }
    public void OnClickBackGroundMusic()
    {

        AllRefs.I._GameManager.OnclickBgMusicButton(); ;

    }
}
