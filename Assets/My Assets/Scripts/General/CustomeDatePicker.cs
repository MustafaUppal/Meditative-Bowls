using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CustomeDatePicker : MonoBehaviour
{
    [Header("Texts")]
    public Text date;
    public Text text;

    [Header("Year Settings")]
    public int startingYear;
    public int numberOfYears;

    [Header("Containers")]
    public Transform yearContent;
    public Transform monthContent;
    public Transform dayContent;
    public Transform offItems;

    [Header("Snap Scrollers")]
    public VerticalScrollSnap daySroll;

    [Header("Pooled Texts")]
    public Transform pooledTextsContainer;

    [Header("Display Settings")]
    public GameObject root;
    public Animator animator;
    public float hideDelay;

    int currentIndex;

    int prevMonth;
    CustomDate customDate;
    int[] numberOfDays = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    public delegate void OnClickOkAction(CustomDate date);

    public static event OnClickOkAction OnClickedOk;

    public void OnEnable()
    {
        Init();
    }

    public void SelectedYear(Int32 year)
    {
        customDate.year = startingYear - year;
        SetDateText();
    }

    public void SelectedMonth(Int32 month)
    {
        prevMonth = customDate.month;
        customDate.month = month;
        UpdateDaysSelection();
        SetDateText();
    }

    public void SelectedDay(Int32 day)
    {
        customDate.day = day + 1;
        SetDateText();
    }

    public void OnClickOkButton()
    {
        if(OnClickedOk != null)
            OnClickedOk(customDate);

        Hide();
    }

    public void OnClickCancelButton()
    {
        Hide();
    }

    public void OnClickOutsideDatePicker()
    {
        Hide();
    }

    void SetDateText()
    {
        string month = 1 + customDate.month < 10 ? "0" + (1 + customDate.month) : (1 + customDate.month).ToString();
        string day = customDate.day < 10 ? "0" + (customDate.day) : (customDate.day).ToString();

        date.text = day + "/" + month + "/" + (customDate.year);
    }

    void Init()
    {
        if (yearContent.childCount > 0)
            return;

        Debug.Log("Init()");
        customDate = new CustomDate(1, 1, 2020);

        Text temp;

        // Init Years
        for (int i = 0; i < numberOfYears; i++)
        {
            temp = Instantiate(text, yearContent);
            temp.text = startingYear + i + "";
        }

        // Init Months
        for (int i = 0; i < 12; i++)
        {
            temp = Instantiate(text, monthContent);

            if (i + 1 < 10)
                temp.text = "0" + (1 + i);
            else
                temp.text = (1 + i) + "";
        }

        // Init Days
        for (int i = 0; i < 31; i++)
        {
            temp = Instantiate(text, dayContent);

            if (i + 1 < 10)
                temp.text = "0" + (1 + i);
            else
                temp.text = (1 + i) + "";
        }

        if (last3Days == null)
        {
            last3Days = new List<Transform>();

            for (int i = 28; i < 31; i++)
                last3Days.Add(dayContent.GetChild(i));
        }
    }

    List<Transform> last3Days;
    void UpdateDaysSelection()
    {
        if (numberOfDays[customDate.month] == 31)
        {
            if (numberOfDays[prevMonth] == 28)
            {
                for (int i = 0; i < 3; i++)
                    daySroll.AddChild(last3Days[i].gameObject);
                prevMonth = customDate.month;
            }
            else if (numberOfDays[prevMonth] == 30)
            {
                daySroll.AddChild(last3Days[2].gameObject);
                prevMonth = customDate.month;
            }
        }
        else if (numberOfDays[customDate.month] == 30)
        {
            if (numberOfDays[prevMonth] == 28)
            {
                for (int i = 0; i < 2; i++)
                    daySroll.AddChild(last3Days[i].gameObject);
                prevMonth = customDate.month;
            }
            else if (numberOfDays[prevMonth] == 31)
            {
                GameObject temp;
                daySroll.RemoveChild(30, out temp);
                prevMonth = customDate.month;
            }
        }
        else
        {
            GameObject temp;
            if (numberOfDays[prevMonth] == 30)
            {

                for (int i = 29; i > 27; i--)
                    daySroll.RemoveChild(i, out temp);
                prevMonth = customDate.month;
            }
            else if (numberOfDays[prevMonth] == 31)
            {
                for (int i = 30; i > 27; i--)
                    daySroll.RemoveChild(i, out temp);
                prevMonth = customDate.month;
            }
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
