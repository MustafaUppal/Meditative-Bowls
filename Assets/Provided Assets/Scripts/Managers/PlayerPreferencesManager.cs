using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreferencesManager
{
    public static readonly string SESSION = "SESSION";
    public static readonly string SLIDE_SHOW = "SLIDE_SHOW";

    public static void SaveSession(string sessionName, string value)
    {
        PlayerPrefs.SetString(sessionName, value);
    }
    public static void GetBowlPurchaseState()
    {

    }
    public static void SetBowlPurchaseState()
    {

    }
    public static string GetSession(string sessionName, string defaultSession)
    {
        return PlayerPrefs.GetString(sessionName, defaultSession);
    }

    public static void SetSlideShowPurchedState(bool value)
    {
        PlayerPrefs.SetInt(SLIDE_SHOW, value ? 1 : 0);
    }

    public static bool GetSlideShowPurchedState(bool defaultVal)
    {
        return PlayerPrefs.GetInt(SLIDE_SHOW, defaultVal ? 1 : 0).Equals(1);
    }

    public static void ClearPreferences()
    {
        PlayerPrefs.DeleteAll();
    }
}
