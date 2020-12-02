using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiinerLoading : MonoBehaviour
{
    public GameObject root;
    public Animator animator;
    public Text message;
    // Start is called before the first frame update
    public void Show(string message)
    {
        root.SetActive(true);
        animator.Play("Popup In");
        this.message.text = message;
    }

    Coroutine hide = null;

    public void Hide()
    {
        if(!gameObject.activeInHierarchy) return;
        
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
