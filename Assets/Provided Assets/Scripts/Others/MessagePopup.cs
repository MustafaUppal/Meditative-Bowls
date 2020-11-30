using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopup : MonoBehaviour
{
    public GameObject root;
    public Text header;
    public Text body;

    public Text button;
    public Animator animator;

    public Action buttonCallBack;

    public void OnClickButton()
    {
        if (buttonCallBack != null) buttonCallBack();
    }

    public void Show(string header, string body, string buttonText = "OK", Action buttonCallBack = null)
    {
        if (AllRefs.I.objectSelection != null)
            AllRefs.I.objectSelection.EnableClick(false);
        root.SetActive(true);
        animator.Play("Popup In");
        this.header.text = header;
        this.body.text = body;
        this.button.text = buttonText;

        this.buttonCallBack = buttonCallBack;
    }

    Coroutine hide = null;

    public void Hide()
    {
        if (AllRefs.I.objectSelection != null && MenuManager.Instance.currentState == MenuManager.MenuStates.Main)
            AllRefs.I.objectSelection.EnableClick(true);
        if (hide != null)
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
