﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Popup1: MonoBehaviour
{
    public GameObject root;

    public Text header;
    public Text errorText;
    public InputField inputField;
    public Animator animator;

    public void Show(string header)
    {
        root.SetActive(true);
        animator.Play("Popup In");
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

    Coroutine hide = null;

    public void Hide()
    {
        if(hide != null)
            StopCoroutine(hide);
        
        hide = StartCoroutine(HideE());
        animator.Play("Popup Out");
    }

    IEnumerator HideE()
    {
        yield return new WaitForSecondsRealtime(1);
        root.SetActive(false);

        hide = null;
    }
}