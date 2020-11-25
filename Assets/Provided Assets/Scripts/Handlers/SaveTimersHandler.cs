using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveTimersHandler : MonoBehaviour
{
    public Transform content;


    void Start()
    {

    }

    void Update()
    {

    }

    private void OnEnable()
    {
        RefreshList();
    }

    public void OnClickSaveTimer()
    {
        if (MenuManager.Instance.currentState == MenuManager.MenuStates.Main)
        {
            SaveItem(new CustomTime(
                AllRefs.I.mainMenu.randomizationSettings.hours.number,
                AllRefs.I.mainMenu.randomizationSettings.mins.number,
                AllRefs.I.mainMenu.randomizationSettings.secs.number
            ));
        }
    }

    public void OnClickLoadTimer(Text timeText)
    {
        CustomTime time = new CustomTime(timeText.text);

        if(MenuManager.Instance.currentState == MenuManager.MenuStates.Main)
        {
            AllRefs.I.mainMenu.randomizationSettings.hours.SetNumber(time.hours);
            AllRefs.I.mainMenu.randomizationSettings.mins.SetNumber(time.minutes);
            AllRefs.I.mainMenu.randomizationSettings.secs.SetNumber(time.seconds);
        }
    }

    public void SaveItem(CustomTime time)
    {
        int index = PlayerPreferencesManager.GetTimerIndex(0);

        PlayerPreferencesManager.SaveTimer(index, time);
        RefreshList();
        PlayerPreferencesManager.SetTimerIndex((index + 1) % 10);
    }

    void RefreshList()
    {
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPreferencesManager.HasTimer(i))
            {
                content.GetChild(i).GetChild(0).GetComponent<Text>().text = PlayerPreferencesManager.GetTimer(i).GetTimeString();
                content.GetChild(i).gameObject.SetActive(true);
            }
            else break;
        }
    }
}
