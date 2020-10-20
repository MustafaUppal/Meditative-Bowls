using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomTime
{
    public int hours;
    public int minutes;
    public int seconds;

    public CustomTime(int hours, int minutes, int seconds)
    {
        SetTime(hours, minutes, seconds);
    }

    public CustomTime(string timeString)
    {
        SetTime(timeString);
    }

    public void SetTime(int hours, int minutes, int seconds)
    {
        this.seconds = seconds;

        this.minutes = minutes + (this.seconds / 60);
        this.seconds %= 60;

        this.hours = hours + (this.minutes / 60);
        this.minutes %= 60;
    }

    public void SetTime(string timeString)
    {
        string[] parts = timeString.Split(':');

        int hours = int.Parse(parts[0]);
        int minutes = int.Parse(parts[1]);
        int seconds = int.Parse(parts[2]);

        this.seconds = seconds;

        this.minutes = minutes + (this.seconds / 60);
        this.seconds %= 60;

        this.hours = hours + (this.minutes / 60);
        this.minutes %= 60;
    }

    public void IncrementTime(int hours = 0, int minutes = 0, int seconds = 0)
    {
        this.seconds += seconds;

        this.minutes += minutes + (this.seconds / 60);
        this.seconds %= 60;

        this.hours += hours + (this.minutes / 60);
        this.minutes %= 60;
    }

    public void DecrementTime(int hours = 0, int minutes = 0, int seconds = 0)
    {
        int tempSeconds = this.seconds, tempMinutes = this.minutes, tempHours = this.hours;

        tempSeconds -= seconds;

        if (tempSeconds < 0)
        {
            tempSeconds += 60;
            tempMinutes -= 1;
        }

        tempMinutes -= minutes;

        if (tempMinutes < 0)
        {
            tempMinutes += 60;
            tempHours -= 1;
        }

        tempHours -= hours;

        if (tempHours < 0)
            Debug.LogError("Invalid time to decrement");
        else
        {
            this.seconds = tempSeconds;
            this.minutes = tempMinutes;
            this.hours = tempHours;
        }
    }

    public string GetTimeString()
    {
        return hours + ":" + minutes + ":" + seconds;
    }

    // public string GetFormatedTimeString()
    // {
    //     string formatedTime = "";

    //     if(hours == 1)
    //         formatedTime += hours + " " + Localizer("44") + " ";
    //     else if(hours > 1)
    //         formatedTime += hours + " " + Localizer("43") + " ";

    //     if(minutes == 1)
    //         formatedTime += minutes + " " + Localizer("45") + " ";
    //     else if(minutes > 1)
    //         formatedTime += minutes + " " + Localizer("42") + " ";

    //     if(seconds == 1)
    //         formatedTime += seconds + " " + Localizer("46");
    //     else if(seconds > 1)
    //         formatedTime += seconds + " " + Localizer("41") + " ";

    //     if(formatedTime.Length == 0)
    //         formatedTime = "0 " + Localizer("41") + " ";

    //     return formatedTime;
    // }

    public int GetTimeInSeconds()
    {
        return seconds + minutes * 60 + hours * 3600;
    }
}