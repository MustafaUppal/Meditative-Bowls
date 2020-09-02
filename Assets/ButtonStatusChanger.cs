using GleyPushNotifications;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStatusChanger : MonoBehaviour
{
    public int id;
    
    private void Start()
    {

    }
    public void Cancelnotification(string channel)
    {
       AlarmClockMenuEventListerner.instance.newAlarm = false;
        if (PlayerPrefs.GetInt("AlarmStatus"+id)==0)
        {
            this.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "On";
            GleyNotifications.SendNotification("MediatativeBowl", "Time To Meditate",             
                new System.TimeSpan(AlarmClockMenuEventListerner.instance.HoursList[id], 
                AlarmClockMenuEventListerner.instance.MinList[id], AlarmClockMenuEventListerner.instance.SecList[id]));
            AlarmClockMenuEventListerner.instance.StatusOfAlarms[id] = true;
            PlayerPrefs.SetInt("AlarmStatus" + id, 1);

            
        }
        else
        {
            this.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "Off";
            PlayerPrefs.SetInt("AlarmStatus" + id, 0);
            AlarmClockMenuEventListerner.instance.StatusOfAlarms[id] = false;
            // NotificationManager.Instance.CancelANotiFication(AlarmClockMenuEventListerner.instance.ChannelId[id]);
            //AlarmClockMenuEventListerner.instance.SaveNotificationList();
        }
    }
}
