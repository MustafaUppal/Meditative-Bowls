using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestionPopup : MonoBehaviour
{
    public GameObject root;

    public Text header;
    public Text questiontext;
    public Animator animator;

    public Text[] buttonsText;

    Action<bool> callback;

    public void Show(string header, string question, Action<bool> callback)
    {
        root.SetActive(true);
        animator.Play("Popup In");
        
        this.header.text = header;
        questiontext.text = question;
        this.callback = callback;
    }

    public void SetButton(string positive = "OK", string negative = "Cancel")
    {
        buttonsText[0].text = positive;
        buttonsText[1].text = negative;
    }

    Coroutine hide = null;

    public void Hide()
    {
        if(hide != null)
            StopCoroutine(hide);
        
        hide = StartCoroutine(HideE());
        animator.Play("Popup Out");

        if(GameManager.Instance != null)
            GameManager.Instance.State1 = GameManager.State.Normal;
    }

    IEnumerator HideE()
    {
        yield return new WaitForSecondsRealtime(1);
        root.SetActive(false);

        hide = null;
    }

    public void OnClickButton(bool response)
    {
        if(callback != null) callback(response);

        Hide();
    }
}
