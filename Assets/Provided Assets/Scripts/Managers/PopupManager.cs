using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public Popup1 popup;

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
        popup.Hide();
    }

}

[System.Serializable]
public class Popup1
{
    public GameObject root;

    public Text header;
    public Text errorText;
    public InputField inputField;

    public void Show(string header)
    {
        root.SetActive(true);
        errorText.gameObject.SetActive(false);
        this.header.text = header;
    }

    public void ShowError(string error)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = error;
    }

    public string GetName()
    {
        return inputField.text;
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}
