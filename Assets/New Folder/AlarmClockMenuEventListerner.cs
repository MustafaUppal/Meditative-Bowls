using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Unity.Notifications.Android;
using GleyPushNotifications;
using TMPro;
using UnityEngine.Tilemaps;

public class AlarmClockMenuEventListerner : MonoBehaviour
{


    int Hou, Mi, Se;

    [Header("Time Settings")]
    public Button Okbutton;
    public CustomeDatePicker datePicker;
    public CustomeTimePicker timePicker;
    public Text timeText;
    public Text dateText;

    public CustomTime selectedTime = new CustomTime(1, 0, 0);
    public CustomDate selectedDate = new CustomDate(1, 1, 2020);

    bool isintractable;

    [Header("Others")]
    public Text Message;
    public string NotificationString;
    public string Body;
    public string Title;
    public List<int> IdofNotification;
    private bool ReminderSet;
    public GameObject Tiles;
    public GameObject AlarmSettings;
    public List<string> AlarmList;
    public List<bool> StatusOfAlarms;
    public GameObject NotificationPoint;
    public static AlarmClockMenuEventListerner instance;

    public List<int> HoursList;
    public List<int> MinList;
    public List<int> SecList;
    public List<string> ChannelId;
    public bool newAlarm;

    void Start()
    {
        instance = this;
        GleyNotifications.Initialize();
        LoadNotificationList();
    }
    void OnEnable()
    {
        // Hours.SetNumber(0);
        // Mins.SetNumber(0);
        // Second.SetNumber(0);

        AllRefs.I.objectSelection.EnableClick(false);

        CustomeTimePicker.OnClickedOk += OnTimeSet;
        CustomeDatePicker.OnClickedOk += OnDateSet;
    }

    private void OnDisable()
    {
        CustomeTimePicker.OnClickedOk -= OnTimeSet;
        CustomeDatePicker.OnClickedOk -= OnDateSet;
    }

    private void Update()
    {
        // isintractable = Hours.isValChanged || Mins.isValChanged || Second.isValChanged;
        // Okbutton.interactable = isintractable;
    }

    public void ShowingTile(string TimeofAlarm, bool State, int HourtoUpdate, int MinToUpdate, int SecondToUpdate, string dateTime)
    {
        // Set Alarm lists
        AlarmList.Add(dateTime);
        StatusOfAlarms.Add(State);
        HoursList.Add(HourtoUpdate);
        MinList.Add(MinToUpdate);
        SecList.Add(SecondToUpdate);
        GameObject game = Instantiate(Tiles, NotificationPoint.transform);
        game.GetComponent<ButtonStatusChanger>().Time.text = dateTime;
        for (int i = 0; i < AlarmList.Count; i++)
        {
            if (AlarmList[i] == dateTime)
                game.GetComponent<ButtonStatusChanger>().id = i;

        }

        SaveNotificationList();
        ButtonState(State, game);
        game = null;
    }
    public void ButtonState(bool State, GameObject tile)
    {
        //Setting Button ON and Off

        if (State)
        {
            tile.transform.GetComponent<ButtonStatusChanger>().ButtonText.text = "On";
            tile.transform.GetComponent<ButtonStatusChanger>().ButtonImage.color = Color.green;
        }
        else
        {
            tile.transform.GetComponent<ButtonStatusChanger>().ButtonText.text = "Off";
            tile.transform.GetComponent<ButtonStatusChanger>().ButtonImage.color = Color.red;
        }

    }

    public void OnClickSetAlarmButton()
    {
        GleyNotifications.Initialize(false);

        int tempHours = selectedTime.hours + (selectedTime.am_pm.Equals("AM") ? 0 : 12);

        DateTime current = DateTime.Now;
        DateTime selected = new DateTime(selectedDate.year, selectedDate.month + 1, selectedDate.day, tempHours, selectedTime.minutes, 0);

        TimeSpan reqTime = selected.Subtract(current);

        Debug.Log("Difference: " + (reqTime.Hours + reqTime.Days * 24) + ":" + reqTime.Minutes + ":" + reqTime.Seconds);

        Hou = reqTime.Hours + reqTime.Days * 24;
        Mi = reqTime.Minutes;
        Se = reqTime.Seconds;

        if (Hou < 0 || Mi < 0 || Se < 0)
        {
            PopupManager.Instance.messagePopup.Show("Invalid Date/Time!", "Please set reminder for future Date/Time only.");
            return;
        }

        AlarmSettings.SetActive(false);
        for (int i = 0; i < AlarmList.Count; i++)
        {
            if (AlarmList[i] == (Hou.ToString() + Mi.ToString() + Se.ToString()))
            {
                PopupManager.Instance.messagePopup.Show("Duplicate Notification", "Notification Already Exits");
                return;
            }
        }
        // var ms = new System.TimeSpan(Hou, Mi, Se);
        var ms = new System.TimeSpan(reqTime.Hours);

        newAlarm = true;
        NotificationManager.Instance.GetAllNotifications();

        GleyNotifications.SendNotification("MeditativeBowls", "Time To Meditate", new System.TimeSpan(Hou, Mi, Se));
        string TotalTimeString = (Hou.ToString() + ":" + Mi.ToString() + ":" + Se.ToString());
        print(TotalTimeString);
        ShowingTile(TotalTimeString, true, Hou, Mi, Se, selected.ToString("dd/MM/yyyy hh:mm tt"));
    }

    public void SaveNotificationList()
    {
        int capacity = AlarmList.Count;
        PlayerPrefs.SetInt("Capacity", capacity);
        //print(capacity);

        for (int i = 0; i < capacity; i++)
        {
            //Saving Time tO SHOW
            PlayerPrefs.SetString("AlarmList" + i, AlarmList[i]);

            //SavingTime:
            PlayerPrefs.SetInt("HourList" + i, HoursList[i]);
            PlayerPrefs.SetInt("MinList" + i, MinList[i]);
            PlayerPrefs.SetInt("SecondList" + i, SecList[i]);

            //Saving Channelids
            //Saving Bool State
            if (StatusOfAlarms[i])
                PlayerPrefs.SetInt("AlarmStatus" + i, 1);
            else
                PlayerPrefs.SetInt("AlarmStatus" + i, 0);

            try { PlayerPrefs.SetString("ChaneelIDs" + i, ChannelId[i]); }
            catch (Exception e) { };
        }

        //Saving Al list Capacity
    }

    public void LoadNotificationList()
    {
        int capacity = PlayerPrefs.GetInt("Capacity");
        print(capacity);
        for (int i = 0; i < capacity; i++)
        {

            //Getting Channel id 
            ChannelId.Add(PlayerPrefs.GetString("ChaneelIDs" + i));
            HoursList.Add(PlayerPrefs.GetInt("HourList" + i));
            MinList.Add(PlayerPrefs.GetInt("MinList" + i));
            SecList.Add(PlayerPrefs.GetInt("SecondList" + i));

            AlarmList.Add(PlayerPrefs.GetString("AlarmList" + i));
            //iNSTENTIATE Tiles
            GameObject NEWtILE = Instantiate(Tiles, NotificationPoint.transform);
            NEWtILE.GetComponent<ButtonStatusChanger>().id = AlarmList.IndexOf(PlayerPrefs.GetString("AlarmList" + i));
            NEWtILE.GetComponent<ButtonStatusChanger>().Time.text = AlarmList[i];

            if (PlayerPrefs.GetInt("AlarmStatus" + i) == 0)
            {
                StatusOfAlarms.Add(true);
                NEWtILE.transform.GetComponent<ButtonStatusChanger>().ButtonText.text = "On";
                NEWtILE.transform.GetComponent<ButtonStatusChanger>().ButtonImage.color = Color.green;
            }
            else
            {
                StatusOfAlarms.Add(false);
                NEWtILE.transform.GetComponent<ButtonStatusChanger>().ButtonText.text = "Off";
                NEWtILE.transform.GetComponent<ButtonStatusChanger>().ButtonImage.color = Color.red;
            }

        }
    }

    private void MessageSender(string v)
    {
        Message.text = v;
    }
    public void OnClickBackButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
        GameManager.Instance.State1 = GameManager.State.Normal;
    }

    public void OnClickShowPickerButton(int index)
    {
        switch (index)
        {
            case 0:
                timePicker.Show();
                break;
            case 1:
                datePicker.Show();
                break;
        }
    }

    public void OnTimeSet(CustomTime time)
    {
        selectedTime = time;
        timeText.text = time.GetTime();
    }

    public void OnDateSet(CustomDate date)
    {
        selectedDate = date;
        dateText.text = date.GetDate();
    }
}