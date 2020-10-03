﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Unity.Notifications.Android;
using GleyPushNotifications;
using TMPro;
using UnityEngine.Tilemaps;

public class AlarmClockMenuEventListerner : MonoBehaviour {
    int Hou, Mi, Se;
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

    void Start () {
        instance = this;
        GleyNotifications.Initialize ();
        LoadNotificationList ();
    }
    void OnEnable () {
        AllRefs.I.objectSelection.EnableClick (false);
    }

    public void ShowingTile (string TimeofAlarm, bool State, int HourtoUpdate, int MinToUpdate, int SecondToUpdate) {
        // Set Alarm lists
        AlarmList.Add (TimeofAlarm);
        StatusOfAlarms.Add (State);
        HoursList.Add (HourtoUpdate);
        MinList.Add (MinToUpdate);
        SecList.Add (SecondToUpdate);
        GameObject game = Instantiate (Tiles, NotificationPoint.transform);
        game.GetComponent<ButtonStatusChanger> ().Time.text = TimeofAlarm;
        for (int i = 0; i < AlarmList.Count; i++) {
            if (AlarmList[i] == TimeofAlarm)
                game.GetComponent<ButtonStatusChanger> ().id = i;

        }

        SaveNotificationList ();
        ButtonState (State, game);
        game = null;
    }
    public void ButtonState (bool State, GameObject tile) {
        //Setting Button ON and Off

        if (State) {
            tile.transform.GetComponent<ButtonStatusChanger> ().ButtonText.text = "On";
            tile.transform.GetComponent<ButtonStatusChanger> ().ButtonImage.color = Color.green;
        } else {
            tile.transform.GetComponent<ButtonStatusChanger> ().ButtonText.text = "Off";
            tile.transform.GetComponent<ButtonStatusChanger> ().ButtonImage.color = Color.red;
        }

    }

    private void Update () {
        isintractable = (Hours.text != "") && (Second.text != "") &&
            (Mins.text != "") && (int.Parse (Hours.text) >= 0) &&
            (int.Parse (Second.text) >= 0) && (int.Parse (Mins.text) >= 0);
        Okbutton.interactable = isintractable;
    }

    public void OnClickSetAlarmButton () {

        GleyNotifications.Initialize (false);

        Hou = int.Parse (Hours.text);
        Mi = int.Parse (Mins.text);
        Se = int.Parse (Second.text);
        AlarmSettings.SetActive (false);
        for (int i = 0; i < AlarmList.Count; i++) {
            if (AlarmList[i] == (Hours.text + Mins.text + Second.text)) {
                PopupManager.Instance.messagePopup.Show ("Duplicate Notification", "Notification Already Exits");
                return;
            }
        }
        var ms = new System.TimeSpan (Hou, Mi, Se);
        newAlarm = true;
        NotificationManager.Instance.GetAllNotifications ();

        GleyNotifications.SendNotification ("MeditativeBowls", "Time To Meditate", new System.TimeSpan (Hou, Mi, Se));
        string TotalTimeString = (Hou.ToString () + ":" + Mi.ToString () + ":" + Se.ToString ());
        print (TotalTimeString);
        ShowingTile (TotalTimeString, true, Hou, Mi, Se);
    }

    public void SaveNotificationList () {
        int capacity = AlarmList.Count;
        PlayerPrefs.SetInt ("Capacity", capacity);
        //print(capacity);
        if (capacity != -1) {
            for (int i = 0; i < capacity; i++) {
                //Saving Time tO SHOW
                PlayerPrefs.SetString ("AlarmList" + i, AlarmList[i]);

                //SavingTime:
                PlayerPrefs.SetInt ("HourList" + i, HoursList[i]);
                PlayerPrefs.SetInt ("MinList" + i, MinList[i]);
                PlayerPrefs.SetInt ("SecondList" + i, SecList[i]);

                //Saving Channelids
                //Saving Bool State
                if (StatusOfAlarms[i])
                    PlayerPrefs.SetInt ("AlarmStatus" + i, 1);

                else
                    PlayerPrefs.SetInt ("AlarmStatus" + i, 0);

                PlayerPrefs.SetString ("ChaneelIDs" + i, ChannelId[i]);
            }
        }
        //Saving Al list Capacity
    }

    public void LoadNotificationList () {
        int capacity = PlayerPrefs.GetInt ("Capacity");
        print (capacity);
        for (int i = 0; i < capacity; i++) {

            //Getting Channel id 
            ChannelId.Add (PlayerPrefs.GetString ("ChaneelIDs" + i));
            HoursList.Add (PlayerPrefs.GetInt ("HourList" + i));
            MinList.Add (PlayerPrefs.GetInt ("MinList" + i));
            SecList.Add (PlayerPrefs.GetInt ("SecondList" + i));

            AlarmList.Add (PlayerPrefs.GetString ("AlarmList" + i));
            //iNSTENTIATE Tiles
            GameObject NEWtILE = Instantiate (Tiles, NotificationPoint.transform);
            NEWtILE.GetComponent<ButtonStatusChanger> ().id = AlarmList.IndexOf (PlayerPrefs.GetString ("AlarmList" + i));
            NEWtILE.GetComponent<ButtonStatusChanger> ().Time.text = AlarmList[i];

            if (PlayerPrefs.GetInt ("AlarmStatus" + i) == 0) {
                StatusOfAlarms.Add (true);
                NEWtILE.transform.GetComponent<ButtonStatusChanger> ().ButtonText.text = "On";
                NEWtILE.transform.GetComponent<ButtonStatusChanger> ().ButtonImage.color = Color.green;
            } else {
                StatusOfAlarms.Add (false);
                NEWtILE.transform.GetComponent<ButtonStatusChanger> ().ButtonText.text = "Off";
                NEWtILE.transform.GetComponent<ButtonStatusChanger> ().ButtonImage.color = Color.red;
            }

        }
    }

    private void MessageSender (string v) {
        Message.text = v;
    }
    public void OnClickBackButton () {
        MenuManager.Instance.ChangeState (MenuManager.MenuStates.Main);
        AllRefs.I._GameManager.state = GameManager.State.Normal;
    }

}