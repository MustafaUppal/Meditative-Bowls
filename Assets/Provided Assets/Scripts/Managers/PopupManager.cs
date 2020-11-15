using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public delegate void CancelAction();
    public CancelAction cancelAtion;

    public static PopupManager Instance;

    public Popup1 popup;
    public GeneralLoading loading;
    public MessagePopup messagePopup;
    public SpiinerLoading spinnerLoading;
    public QuestionPopup questionPopup;

    Action<string> method;

    private void Awake()
    {
        Instance = this;
    }

    public void Show(string header, Action<string> OnClickSave)
    {
        AllRefs.I.objectSelection.EnableClick(false);
        popup.Show(header);
        method = OnClickSave;
    }

    public void Hide()
    {
        AllRefs.I.objectSelection.EnableClick(true);
        popup.Hide();
    }

    public void ShowError(string error)
    {
        popup.ShowError(error);
    }

    public void OnClickPopup1Save()
    {
        method(popup.GetName());
    }

    public void OnClickPopup1Cancel()
    {
        if(cancelAtion != null)cancelAtion();
        Hide();
    }

    public void ShowMessage(string header, string body, string buttonText)
    {

    }
}
