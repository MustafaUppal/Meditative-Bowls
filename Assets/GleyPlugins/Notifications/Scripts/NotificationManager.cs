﻿namespace GleyPushNotifications
{
    using UnityEngine;
    using System;
#if EnableNotificationsIos
    using Unity.Notifications.iOS;
#endif
#if EnableNotificationsAndroid
    using Unity.Notifications.Android;
#endif


    public class NotificationManager : MonoBehaviour
    {
        int count = 0;
        const string channelID = "channel_id:";
        private static NotificationManager instance;
#if EnableNotificationsAndroid
        private bool initialized;
#endif
        public void GetAllNotifications()
        {
#if EnableNotificationsAndroid
            AndroidNotificationChannel[] channel = AndroidNotificationCenter.GetNotificationChannels();
            print(channel.Length);
            foreach (var channels in channel)
            {

                print(channels.Id);
            }

#endif

        }
        /// <summary>
        /// Static instance to access this class
        /// </summary>
        /// 
        public static NotificationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "NotificationManager";
                    DontDestroyOnLoad(go);
                    instance = go.AddComponent<NotificationManager>();
                }
                return instance;
            }
        }
        private void Start()
        {
            //count = PlayerPrefs.SetInt("Count", 0);
        }
        /// <summary>
        /// Initializes notification channel and removes already scheduled notifications
        /// </summary>
        /// 
        public void CancelANotiFication(string channelId)
        {
#if EnableNotificationsIos
            iOSNotificationCenter.RemoveDeliveredNotification(channelId);
#elif EnableNotificationsAndroid

#endif
        }
        public void Initialize(bool cancelPendingNotifications)
        {
#if EnableNotificationsAndroid

            count = PlayerPrefs.GetInt("ChannelCount");
            if (initialized == false)
            {
                //initialized = true;

                var c = new AndroidNotificationChannel()
                {
                    Id = channelID ,
                    Name = "Default Channel",
                    Importance = Importance.High,
                    Description = "Generic notifications",
                };
                //count = count + 1;
                PlayerPrefs.SetInt("ChannelCount", count);
                AlarmClockMenuEventListerner.instance.ChannelId.Add(c.Id);
                AndroidNotificationCenter.RegisterNotificationChannel(c);
            }

            if (cancelPendingNotifications == true)
            {
                AndroidNotificationCenter.CancelAllNotifications();
            }

#endif
#if EnableNotificationsIos
            if (cancelPendingNotifications == true)
            {
                iOSNotificationCenter.RemoveAllScheduledNotifications();
                iOSNotificationCenter.RemoveAllDeliveredNotifications();
            }
#endif
        }

        /// <summary>
        /// Schedules a notification
        /// </summary>
        /// <param name="title">title of the notification</param>
        /// <param name="text">body of the notification</param>
        /// <param name="timeDelayFromNow">time to appear, calculated from now</param>
        /// <param name="smallIcon">small icon name for android only - from Mobile Notification Settings </param>
        /// <param name="largeIcon">large icon name for android only - from Mobile Notification Settings </param>
        /// <param name="customData">custom data that can be retrieved when user opens the app from notification </param>
        internal void SendNotification(string title, string text, TimeSpan timeDelayFromNow, string smallIcon, string largeIcon, string customData)
        {
#if EnableNotificationsAndroid
            var notification = new AndroidNotification();
            notification.Title = title;
            notification.Text = text;
            if (smallIcon != null)
            {
                notification.SmallIcon = smallIcon;
            }
            if (smallIcon != null)
            {
                notification.LargeIcon = largeIcon;
            }
            if (customData != null)
            {
                notification.IntentData = customData;
            }
            notification.FireTime = DateTime.Now.Add(timeDelayFromNow);

            AndroidNotificationCenter.SendNotification(notification, channelID + count);
#endif

#if EnableNotificationsIos
            iOSNotificationTimeIntervalTrigger timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = timeDelayFromNow,
                Repeats = false,
            };

            iOSNotification notification = new iOSNotification()
            {
                Identifier = channelID + count,
                Title = title,
                Subtitle = "Time To Meditate",
                Body = text,
                Data = customData,
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };
            if (AlarmClockMenuEventListerner.instance.newAlarm)
            {
                AlarmClockMenuEventListerner.instance.ChannelId.Add(notification.Identifier);
                count++;
            }
            iOSNotificationCenter.ScheduleNotification(notification);
#endif
        }

        /// <summary>
        /// Check if app was opened from notification
        /// </summary>
        /// <returns>the custom data from notification schedule or null if the app was not opened from notification</returns>
        public string AppWasOpenFromNotification()
        {
#if EnableNotificationsAndroid
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

            if (notificationIntentData != null)
            {
                return notificationIntentData.Notification.IntentData;
            }
            else
            {
                return null;
            }
#elif EnableNotificationsIos
            iOSNotification notificationIntentData = iOSNotificationCenter.GetLastRespondedNotification();

            if (notificationIntentData != null)
            {
                return notificationIntentData.Data;
            }
            else
            {
                return null;
            }
#else
            return null;
        }
#endif
        }
    }
}
