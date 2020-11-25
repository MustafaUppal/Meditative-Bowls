using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomTime
{
    public int hours;
    public int minutes;
    public int seconds;
    public string am_pm;

    public CustomTime(int hours, int minutes, int seconds, string am_pm = "AM")
    {
        SetTime(hours, minutes, seconds, am_pm);
    }

    public CustomTime(string timeString)
    {
        SetTime(timeString);
    }

    public void SetTime(int hours, int minutes, int seconds, string am_pm)
    {
        this.seconds = seconds;

        this.minutes = minutes + (this.seconds / 60);
        this.seconds %= 60;

        this.hours = hours + (this.minutes / 60);
        this.minutes %= 60;

        this.am_pm = am_pm;
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

    public string GetTime()
    {
        string minutes = this.minutes < 10 ? "0" + this.minutes : this.minutes.ToString();
        string hours = this.hours < 10 ? "0" + this.hours : this.hours.ToString();

        return hours + ":" + minutes + " " + am_pm;
    }

    public int GetTimeInSeconds()
    {
        return seconds + minutes * 60 + hours * 3600;
    }
    
}