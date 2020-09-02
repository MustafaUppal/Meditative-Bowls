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
    Action<string> method;

    private void Awake()
    {
        Instance = this;
    }

    public void Show(string header, Action<string> OnClickSave)
    {
        
        popup.Show(header);
        method = OnClickSave;
    }

    public void Hide()
    {
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
        cancelAtion();
        Hide();
    }

}
