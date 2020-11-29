using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuEventListener : MonoBehaviour
{
    public GameObject[] allPages;

    public Animator animator;

    public void Enable(bool enable)
    {
        // animator.SetInteger("State", enable ? 1 : 0);

        if(enable)
        {
            print((int)MenuManager.Instance.prevState);
            print((int)MenuManager.Instance.currentState);

            allPages[(int)MenuManager.Instance.prevState].SetActive(false);
            allPages[(int)MenuManager.Instance.currentState].SetActive(true);
        }

        if(AllRefs.I.objectSelection)
        AllRefs.I.objectSelection.EnableClick(!enable);
    }

    // void OnDisable()
    // {
    //     AllRefs.I.objectSelection.EnableClick(true);
    // }
}
