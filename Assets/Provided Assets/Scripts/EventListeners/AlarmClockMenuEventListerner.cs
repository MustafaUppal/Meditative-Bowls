using System;
using System.Collections;
using System.Collections.Generic;
// using Unity.Notifications.Android;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AlarmClockMenuEventListerner : MonoBehaviour
{
    int Hour, Min, Sec;
    public InputField Hours, Mins, Second;
    public Button Okbutton;
    bool isintractable;
    public Text Message;
    private bool ReminderSet;
    private GameObject Tile;
    void Start()
    {
        // GleyNotifications.Initialize();
        ShowNotification();
    }
    
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        isintractable = (Hours.text != "") || (Second.text != "") || (Mins.text != "");
        
        Okbutton.interactable= isintractable ;

    }
    public void OnClickSetAlarmButton()
    {

        Hour = int.Parse( Hours.text);
        Min = int.Parse(Mins.text);
        Sec = int.Parse(Second.text);
        // GleyNotifications.SendNotification("Meditative Bowl","Reminder to Meditate",new System.TimeSpan(Hour,Min,Sec));
        OnClickBackButton();
        ShowNotification();


    }
    public void ShowNotification()
    {
        // Debug.Log(AndroidNotificationCenter.GetNotificationChannels());
    }
    //private void OnApplicationFocus(bool focus)
    //{
    //    if (ReminderSet)
    //        GleyNotifications.SendNotification("Meditative Bowl", "Reminder to Meditate", new System.TimeSpan(Hour, Min, Sec));
    //}
    private void MessageSender(string v)
    {
        Message.text = v;
    }
    public void OnClickBackButton()
    {
        // MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);

    }

}
