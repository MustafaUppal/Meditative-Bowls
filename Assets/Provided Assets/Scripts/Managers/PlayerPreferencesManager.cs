using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreferencesManager
{
    public static readonly string SESSION = "SESSION";

    public static void SaveSession(string sessionName, string value)
    {
        PlayerPrefs.SetString(sessionName, value);
    }

    public static string GetSession(string sessionName, string defaultSession)
    {
        return PlayerPrefs.GetString(sessionName, defaultSession);
    }

    public static void ClearPreferences()
    {
        PlayerPrefs.DeleteAll();
    }
}
