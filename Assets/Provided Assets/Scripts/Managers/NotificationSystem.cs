// //using iRely.Common.PushNotification;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Playables;
// // using Unity.Notifications.iOS;


// public class NotificationSystem : MonoBehaviour
// {

//     public static NotificationSystem instance;


//     void Start()
//     {
//         instance=this;
//     }
//     iOSNotificationTimeIntervalTrigger IniTialize(int hour, int min, int Sec)
//     {
//         iOSNotificationTimeIntervalTrigger Trigger = new iOSNotificationTimeIntervalTrigger()
//         {
//             TimeInterval = new System.TimeSpan(hour, min, Sec),
//             Repeats = false,
//         };
//         return Trigger;
//     }
//     public void InitializeWithChalender(int year, int Month, int Day, int date, int hour, int Min, int Sec, bool Repeat)
//     {
//         iOSNotificationCalendarTrigger CalendarTrigger = new iOSNotificationCalendarTrigger
//         {
//             Year = year,
//             Month = Month,
//             Day = Day,
//             Minute = Min,
//             Hour = hour,
//             Second = Sec,
//             Repeats = Repeat,
//         };
//     }
//     public void InitializeWithlocation(float lONGitude, float lattitude, float Radi, bool NotifyEnty, bool NotifyExits)
//     {
//         iOSNotificationLocationTrigger locationTrigger = new iOSNotificationLocationTrigger()
//         {
//             Center = new Vector2(lONGitude, lattitude),
//             Radius = Radi,
//             NotifyOnEntry = NotifyEnty,
//             NotifyOnExit = NotifyExits,
//         };
//     }
//     public void RemoveNotification(string NotificationId)
//     {
//         iOSNotificationCenter.RemoveScheduledNotification(NotificationId);
//     }
//     public void RemoveAllScheduledNotification()
//     {
//         iOSNotificationCenter.RemoveAllScheduledNotifications();
//     }
//     public void RemoveDeliverNotification(string NotificationId)
//     {
//         iOSNotificationCenter.RemoveDeliveredNotification(NotificationId);
//     }
//     public void RemoveAllDeliveredNotification()
//     {
//         iOSNotificationCenter.RemoveAllDeliveredNotifications();
//     }
//     public void RemoveADeliveredNotification(string NotificationId)
//     {
//         iOSNotificationCenter.RemoveDeliveredNotification(NotificationId);
//     }
//     public void OnRemoteCallBacks(iOSNotification notification)
//     {
//         iOSNotificationCenter.OnRemoteNotificationReceived += receivedNotification =>
//          {
//              print("receivedNotification" + notification.Identifier + "!");
//          };
//     }
//     public void NotificationScheduleer(string Identifiers, string Titles, string Subtitiles, string Bodys, bool ShowInForeGrounds, string CategoryIdentifiers, string ThreadIdentifiers, int Hour, int Min, int Sec)
//     {
//         iOSNotificationTimeIntervalTrigger time = IniTialize(Hour, Min, Sec);
//         iOSNotification notification = new iOSNotification()
//         {
//             Identifier = Identifiers,
//             Title = Titles,
//             Subtitle = Subtitiles,
//             Body = Bodys,
//             ShowInForeground = ShowInForeGrounds,
//             ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
//             CategoryIdentifier = CategoryIdentifiers,
//             ThreadIdentifier = ThreadIdentifiers,
//             Trigger = time,
//         };
//         iOSNotificationCenter.ScheduleNotification(notification);
//         print(notification.Identifier);
//         notificationSettings=  iOSNotificationCenter.GetScheduledNotifications();
    
//     }
//     public iOSNotification[] notificationSettings;
//     public void  NotificationGet(){
//         print(notificationSettings.Length);
        
           
        
//     }
// }
