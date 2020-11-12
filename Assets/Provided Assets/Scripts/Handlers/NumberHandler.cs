using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberHandler : MonoBehaviour
{
    [Header("Values")]
    public bool isValChanged = false;
    public int number;

    [Header("Changes")]
    public int increment;
    public int decrement;
    [Tooltip("In seconds")]public float changeSensitivity = 0.25f;


    [Header("Limits")]
    public bool roundRobin;
    public bool applyLimits;
    public int maxLimit;
    public int minLimit;

    [Header("Text Field")]
    public Text numberText;

    // Private Vars
    bool isPlusPressed;
    bool isMinusPressed;
    float sensitivityVal;
    int defaultVal;

    // Properties
    public string Text => numberText.text;

    // Start is called before the first frame update
    void Start()
    {
        isValChanged = false;
        ApplyLimits();
        defaultVal = number;
        numberText.text = number.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        isValChanged = defaultVal != number;

        if(sensitivityVal <= 0 && (isPlusPressed || isMinusPressed))
        {
            sensitivityVal = changeSensitivity;
            number += isPlusPressed ? increment : -decrement;
            ApplyLimits();
            numberText.text = number.ToString();
        }

        sensitivityVal -= Time.unscaledDeltaTime;
    }

    // Buttons Listeners

    public void OnButtonDown(bool isPlus)
    {
        if (isPlus) isPlusPressed = true;
        else isMinusPressed = true;

        sensitivityVal = changeSensitivity;
    }

    public void OnButtonUp(bool isPlus)
    {
        if (isPlus) isPlusPressed = false;
        else isMinusPressed = false;
    }

    public void OnClickButton(bool isPlus)
    {
        number += isPlus ? increment : -decrement;
        ApplyLimits();
        numberText.text = number.ToString();
    }

    public void SetNumber(int number)
    {
        this.number = number;
        ApplyLimits();
        numberText.text = this.number.ToString();
    }

    void ApplyLimits()
    {
        if(applyLimits)
        {
            if(number < minLimit)
                number = roundRobin ? maxLimit - number : minLimit;
            else if(number > maxLimit)
                number = roundRobin ? minLimit + (number - maxLimit) : maxLimit;
        }
    }
}
