using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralLoading : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public Text text;

    Coroutine animationC;

    public void Show(bool enable, string text = "")
    {
        AllRefs.I.objectSelection.EnableClick(enable);
        this.text.text = text;
        if(enable) gameObject.SetActive(enable);

        if(animationC != null)
            StopCoroutine(animationC);

        animationC = StartCoroutine(WaitForAnimation(enable));
    }

    IEnumerator WaitForAnimation(bool enable)
    {
        animator.SetInteger("State", enable ? 1 : 0);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(enable);
    }
}
