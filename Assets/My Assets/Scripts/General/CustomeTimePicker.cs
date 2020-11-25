using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CustomeTimePicker : MonoBehaviour
{
    [Header("Texts")]
    public Text time;
    public Text text;

    [Header("Containers")]
    public Transform hourContent;
    public Transform minuteContent;
    public Transform am_pmContent;

    [Header("Pooled Texts")]
    public Transform pooledTextsContainer;

    [Header("Display Settings")]
    public GameObject root;
    public Animator animator;
    public float hideDelay;

    int currentIndex;

    // int hour, minute;
    // string am_pm = "AM";
    public CustomTime customTime;

    public delegate void OnClickOkAction(CustomTime time);

    public static event OnClickOkAction OnClickedOk;

    public void OnEnable()
    {
        Init();
    }

    public void SelectedHour(Int32 hour)
    {
        this.customTime.hours = hour + 1;
        SetTimeText();
    }

    public void SelectedMinute(Int32 minute)
    {
        this.customTime.minutes = minute;
        SetTimeText();
    }

    public void SelectedAM_PM(Int32 am_pm)
    {
        this.customTime.am_pm = am_pm == 0 ? "AM" : "PM";
        SetTimeText();
    }

    public void OnClickOkButton()
    {
        if(OnClickedOk != null)
            OnClickedOk(customTime);

        Hide();
    }

    public void OnClickCancelButton()
    {
        Hide();
    }

    public void OnClickOutsideTimePicker()
    {
        Hide();
    }

    void SetTimeText()
    {
        string hour = this.customTime.hours < 10 ? "0" + this.customTime.hours : this.customTime.hours.ToString();
        string min = this.customTime.minutes < 10 ? "0" + (this.customTime.minutes) : (this.customTime.minutes).ToString();

        time.text = hour + ":" + min + " " + customTime.am_pm;
    }

    void Init()
    {
        if (hourContent.childCount > 0)
            return;

        Debug.Log("Init()");

        Text temp;
        customTime = new CustomTime(1, 0, 0);

        // Init hours
        for (int i = 1; i <= 12; i++)
        {
            temp = Instantiate(text, hourContent);
            temp.text = i + "";
        }

        // Init minutes
        for (int i = 0; i < 60; i++)
        {
            temp = Instantiate(text, minuteContent);
            temp.text = i + "";
        }
    }

    #region Display Settings
    public enum TransitionTypes
    {
        vanish,
        smooth
    }

    Coroutine hide;

    public void Show(TransitionTypes transitionType = TransitionTypes.smooth)
    {
        if (transitionType == TransitionTypes.smooth)
        {
            animator.enabled = true;

            root.SetActive(true);
            animator.SetInteger("State", 1);
        }
        else
        {
            animator.enabled = false;
            root.SetActive(true);
        }
    }

    public void Hide(TransitionTypes transitionType = TransitionTypes.smooth)
    {
        if (transitionType == TransitionTypes.smooth)
        {
            animator.enabled = true;

            animator.SetInteger("State", 0);

            if (hide != null)
                StopCoroutine(hide);

            hide = StartCoroutine(HideE());
        }
    }

    IEnumerator HideE()
    {
        yield return new WaitForSeconds(hideDelay);
        root.SetActive(false);
        hide = null;
    }
    #endregion
}
