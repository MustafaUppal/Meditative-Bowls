using System;
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
    public GameObject panResetBtn;
    int selectedBowl = -1;

    public InventoryManager Inventory => InventoryManager.Instance;

    public void SelectRandomization()
    {
        if(GivenTimeInputField.text!="" && GivenTimeInputField.text != null)
        GameManager.Instance.SelectRandomiszation(float.Parse(GivenTimeInputField.text));
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
        //GameManager.Instance.VolumeSlider.gameObject.SetActive(false);
        AllRefs.I.dock.ManageButtons(data);
        AllRefs.I.objectSelection.EnableClick(true);
    }

    private void OnDisable() 
    {
        OnClickBackButton();
    }

    private void Start()
    {
        //Instance = this;
    }
    public void ManageFooter(bool isEditingBowl)
    {
        // print(!isEditingBowl);
        // print(isEditingBowl);
        bowlEditingSettings.root.SetActive((isEditingBowl));
        FooterPanel.SetActive(!isEditingBowl);
    }
    public void OnClickStopRandomizationButton()
    {
        GameManager.Instance.SoundRestart();
        GameManager.Instance.State1=GameManager.State.Normal;
    }

    public void OnClickStopRepositioningButton()
    {
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().ResetFuntion();
    }
    public void OnClickBackButton()
    {
        //SettingsMenuEventListener.Instance.ManageFooter(false);
        GameManager.Instance.SelectModeNormal();
        if(GameManager.Instance)
            GameManager.Instance.gameObject.GetComponent<BowlReposition>().ResetFuntion();
    }
    public void OnClickRemoveButton()
    {
        // GameManager.Instance.Remove();
        Debug.Log("normal");
        GameManager.Instance.State1 = GameManager.State.Normal;
        GameManager.Instance.GetComponent<BowlReposition>().ResetFuntion();
    }
    public void OnClickStopMusicButton()
    {
        GameManager.Instance.SoundStop();
    }
    public void OnClickBackGroundMusic()
    {
        GameManager.Instance.SelectModeNormal();

    }

    public void OnVolumeSliderChange(Single val)
    {
        GameManager.Instance.VolumeChange((float)val);
    }

    public void OnPaningSliderChange(Single val)
    {
        panResetBtn.SetActive(true);
        GameManager.Instance.PanningSliderChange((float)val);
    }

    public void OnClickResetPanValueButton()
    {
        GameManager.Instance.PanningSliderChange(Inventory.bowlsManager.BowlPanningValues[selectedBowl]);
        GameManager.Instance.PanningSlider.value = Inventory.bowlsManager.BowlPanningValues[selectedBowl];
        panResetBtn.SetActive(false);
    }

    public void SetResetBtn(int index, float sterioPan)
    {
        panResetBtn.SetActive(false);

        for (int i = 0; i < Inventory.bowlsManager.activeBowlsIndexes.Length; i++) 
        {
            if(Inventory.bowlsManager.activeBowlsIndexes[i] == index)
            {
                selectedBowl = i;
                panResetBtn.SetActive(sterioPan != Inventory.bowlsManager.BowlPanningValues[i]);
                break;
            }
        }
    }
}
