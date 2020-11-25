using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDate
{
    public int year;
    public int month;
    public int day;

    public CustomDate(int day, int month, int year)
    {
        this.year = year;
        this.month = month;
        this.day = day;
    }

    public string GetDate()
    {
        string month = 1 + this.month < 10 ? "0" + (1 + this.month) : (1 + this.month).ToString();
        string day = this.day < 10 ? "0" + (this.day) : (this.day).ToString();

        return day + "/" + month + "/" + this.year;
    }
}
