using System;
using System.Collections;
using System.Collections.Generic;

//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.iOS;
using UnityEditor;
using Unity.Notifications.Android;

public class AlarmClockMenuEventListerner : MonoBehaviour
{
    int Hour, Min, Sec;
    public InputField Hours, Mins, Second;
    public Button Okbutton;
    bool isintractable;
    public Text Message;
    private bool ReminderSet;
    private GameObject Tile;
    int count=0;
    public List <string> id=new List<string>();
  
  
    void Start()
    {
        
    }
    
    private void Update()
    {
        isintractable = (Hours.text != "") || (Second.text != "") || (Mins.text != "");    
        Okbutton.interactable= isintractable;
    }
    public void OnClickSetAlarmButton()
    {
        PlayerPrefs.GetInt("Count");
        Hour = int.Parse( Hours.text);
        Min = int.Parse(Mins.text);
        Sec = int.Parse(Second.text);
        id.Add("Time TO Meditate"+ Hours+Mins+Second);
        NotificationSystem.instance.NotificationScheduleer(id[count],"Meditative Bowl","Time To Meditate","GetUp To Meditate",true,"","",Hour,Min,Sec);
        count++;
        PlayerPrefs.SetInt("Count",count);
        OnClickBackButton();
        ShowNotification();
    }
    public void ShowNotification()
    {
        NotificationSystem.instance.NotificationGet();
    }

    private void MessageSender(string v)
    {
        Message.text = v;
    }
    public void OnClickBackButton()
    {
         MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
         GameManager.Instance.state =GameManager.State.Normal;

    }

}
