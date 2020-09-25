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

     public void Show(string header, string body, string buttonText = "OK")
    {
        root.SetActive(true);
        animator.Play("Popup In");
        this.header.text = header;
        this.body.text = body;
        this.button.text = buttonText;
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
