using GleyPushNotifications;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStatusChanger : MonoBehaviour
{
    public int id;
    public Button NotificationOnAndOff;
    public Text Time;
    public Button Delete;
    public GameObject Pannel;

    public TextMeshProUGUI ButtonText;
    public Image ButtonImage;

    public Color on;
    public Color off;


    private void Start()
    {

    }
    public void OnClickRemoveButton()
    {
        for (int i = 0; i < AlarmClockMenuEventListerner.instance.AlarmList.Count; i++)
        {
            if (AlarmClockMenuEventListerner.instance.AlarmList[i] == Time.text)
            {
                AlarmClockMenuEventListerner.instance.AlarmList.Remove(AlarmClockMenuEventListerner.instance.AlarmList[i]);
                AlarmClockMenuEventListerner.instance.HoursList.Remove(AlarmClockMenuEventListerner.instance.HoursList[i]);
                AlarmClockMenuEventListerner.instance.MinList.Remove(AlarmClockMenuEventListerner.instance.MinList[i]);
                AlarmClockMenuEventListerner.instance.SecList.Remove(AlarmClockMenuEventListerner.instance.SecList[i]);
                try{AlarmClockMenuEventListerner.instance.ChannelId.Remove(AlarmClockMenuEventListerner.instance.ChannelId[i]);}
                catch(Exception e){};
            }
        }
        AlarmClockMenuEventListerner.instance.SaveNotificationList();
        Destroy(this.gameObject);

    }

    public void Cancelnotification(string channel)
    {
        AlarmClockMenuEventListerner.instance.newAlarm = false;

        if (PlayerPrefs.GetInt("AlarmStatus" + id) == 0)
        {
            ButtonImage.color = on;
            ButtonText.text = "On";
            PlayerPrefs.SetInt("AlarmStatus" + id, 1);

            AlarmClockMenuEventListerner.instance.StatusOfAlarms[id] = true;
            GleyNotifications.SendNotification
            (
                "MediatativeBowl", "Time To Meditate",
                new System.TimeSpan(AlarmClockMenuEventListerner.instance.HoursList[id],
                AlarmClockMenuEventListerner.instance.MinList[id], AlarmClockMenuEventListerner.instance.SecList[id])
            );
            print("mmmm");
        }
        else
        {
            ButtonImage.color = off;
            ButtonText.text = "Off";

            PlayerPrefs.SetInt("AlarmStatus" + id, 0);
            AlarmClockMenuEventListerner.instance.StatusOfAlarms[id] = false;
            try{NotificationManager.Instance.CancelANotiFication(AlarmClockMenuEventListerner.instance.ChannelId[id]);}
            catch(Exception e){};
            AlarmClockMenuEventListerner.instance.SaveNotificationList();
            print("kashif");

        }
    }
}
