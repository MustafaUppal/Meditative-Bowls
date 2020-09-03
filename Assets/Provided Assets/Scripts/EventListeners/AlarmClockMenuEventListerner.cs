using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using Unity.Notifications.Android;
using GleyPushNotifications;
using UnityEngine.Tilemaps;

public class AlarmClockMenuEventListerner : MonoBehaviour
{
    int Hour, Min, Sec;
    public InputField Hours, Mins, Second;
    public Button Okbutton;
    bool isintractable;
    public Text Message;
    public string NotificationString;
    public string Body;
    public string Title;
    public List<int> IdofNotification;
    private bool ReminderSet;
    public GameObject Tiles;
    public List<string> AlarmList;
    public List<bool> StatusOfAlarms;
    public GameObject NotificationPoint;
    public static AlarmClockMenuEventListerner instance;
    public List<AndroidNotificationChannel> androidNotification;
    public List<int> HoursList;
    public List<int> MinList;
    public List<int> SecList;
    public List<string> ChannelId;
    public bool newAlarm;
    void Start()
    {
        instance = this;
        LoadNotificationList();
    }
    public void ShowingTile(string TimeofAlarm, bool State, int HourtoUpdate, int MinToUpdate, int SecondToUpdate)
    {
        GameObject game = Instantiate(Tiles, NotificationPoint.transform);
        Tiles.transform.GetChild(0).gameObject.GetComponent<Text>().text = TimeofAlarm;
        // Set Alarm lists
        AlarmList.Add(TimeofAlarm);
        StatusOfAlarms.Add(State);
        HoursList.Add(HourtoUpdate);
        MinList.Add(HourtoUpdate);
        SecList.Add(HourtoUpdate);
        ButtonState(State, game);
        SaveNotificationList();
    }
    public void ButtonState(bool State, GameObject tile)
    {
        //Setting Button ON and Off

        if (State)
        {
            tile.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "On";
            
        }
        else
        {
            tile.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>().text = "Off";
        }

    }

    private void Update()
    {
        isintractable = (Hours.text != "") && (Second.text != "") && (Mins.text != "");
        Okbutton.interactable = isintractable;
    }

    public void OnClickSetAlarmButton()
    {

        GleyNotifications.Initialize(false);

        Hour = int.Parse(Hours.text);
        Min = int.Parse(Mins.text);
        Sec = int.Parse(Second.text);

        var ms = new System.TimeSpan(Hour, Min, Sec);
        // NotificationManager.Instance.GetAllNotifications();

        GleyNotifications.SendNotification("MeditativeBowls", "Time To Meditate", new System.TimeSpan(Hour, Min, Sec));

        ShowingTile(Hour + ":" + Min + ":" + Min, true, Hour, Min, Sec);


    }
    public void SaveNotificationList()
    {
        int capacity = AlarmList.Count;
        print(capacity);

        for (int i = 0; i < capacity; i++)
        {
            //Saving Time tO SHOW
            PlayerPrefs.SetString("AlarmList" + i, AlarmList[i]);

            //SavingTime
            PlayerPrefs.SetInt("HourList" + i, HoursList[i]);
            PlayerPrefs.SetInt("MinList" + i, MinList[i]);
            PlayerPrefs.SetInt("SecondList" + i, SecList[i]);

            //Saving Channelids
            PlayerPrefs.SetString("ChaneelIDs" + i, ChannelId[i]);
            //Saving Bool State
            if (StatusOfAlarms[i])
                PlayerPrefs.SetInt("AlarmStatus" + i, 0);
            else
                PlayerPrefs.SetInt("AlarmStatus" + i, 1);
        }
        //Saving Al list Capacity
        PlayerPrefs.SetInt("Capacity", capacity);
    }

    public void LoadNotificationList()
    {

        int capacity = PlayerPrefs.GetInt("Capacity");

        for (int i = 0; i < capacity; i++)
        {
            AlarmList.Add(PlayerPrefs.GetString("AlarmList" + i));
            //iNSTENTIATE Tiles
            GameObject NEWtILE = Instantiate(Tiles, NotificationPoint.transform);
            NEWtILE.transform.GetChild(0).GetComponent<Text>().text = AlarmList[i];
            NEWtILE.GetComponent<ButtonStatusChanger>().id = i;
            //Getting Channel id 
            ChannelId.Add(PlayerPrefs.GetString("ChaneelIDs" + i));
            HoursList.Add(PlayerPrefs.GetInt("HourList" + i));
            MinList.Add(PlayerPrefs.GetInt("MinList" + i));
            SecList.Add(PlayerPrefs.GetInt("SecondList" + i));

            NEWtILE.GetComponent<ButtonStatusChanger>().id = i;
            if (PlayerPrefs.GetInt("AlarmStatus" + i) == 0)
            {
                StatusOfAlarms.Add(true);
                NEWtILE.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>().text = "ON";

            }
            else
            {
                StatusOfAlarms.Add(false);
                NEWtILE.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>().text = "OFF";
            }
        }

    }

    private void MessageSender(string v)
    {
        Message.text = v;
    }
    public void OnClickBackButton()
    {
        // MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
        GameManager.Instance.state = GameManager.State.Normal;
    }

}
