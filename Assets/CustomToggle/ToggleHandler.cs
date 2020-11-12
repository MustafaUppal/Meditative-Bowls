using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    public bool isOn;
    public bool isInteractable = true;

    [Header("Tween Settings")]
    public float time;
    public iTween.EaseType easeType;

    [Header("Text Settings")]
    public string onText;
    public string offText;

    [Header("Color Settings")]
    public Color onColor;
    public Color offColor;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool>
    {
    }

    [Space]
    public BoolEvent onClickSwitch;

    float width = 60;
    Text text;
    Button tSwitch;
    Image bg;
    // Start is called before the first frame update
    void Start()
    {
        bg = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        text = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        tSwitch = transform.GetChild(0).GetComponent<Button>();

        width = GetComponent<RectTransform>().sizeDelta.x;
        // Debug.Log("Width: " + width);

        SetValue(isOn);
    }

    private void Update()
    {
        tSwitch.interactable = isInteractable;
    }

    public void OnClickSwitchButton()
    {
        isOn = !isOn;
        onClickSwitch.Invoke(isOn);

        SetValue(isOn);
    }

    public void SetValue(bool isOn)
    {
        this.isOn = isOn;

        if (gameObject.activeInHierarchy)
        {
            iTween.MoveTo(tSwitch.gameObject, iTween.Hash("x", isOn ? 0 : -width / 2, "easeType", easeType, "time", time, "islocal", true));
            bg.color = isOn ? onColor : offColor;
            text.text = isOn ? onText : offText;
        }
    }
}
