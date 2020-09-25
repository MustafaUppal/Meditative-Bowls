using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreferencesManager
{
    public static readonly string SESSION = "SESSION";
    public static readonly string ITEMS_RESTORED = "ITEMS_RESTORED";

    public static void SetItemsRestored(bool value)
    {
        PlayerPrefs.SetInt(ITEMS_RESTORED, value ? 1 : 0);
    }

    public static bool GetItemsRestored(bool defaultVal)
    {
        return PlayerPrefs.GetInt(ITEMS_RESTORED, defaultVal ? 1 : 0).Equals(1);
    }

    public static void SetPurchasedState(int type, int index, bool value)
    {
        PlayerPrefs.SetInt(GetItemID(type, index), value ? 1 : 0);
    }

    public static bool GetPurchasedState(int type, int index, bool defaultVal)
    {
        return PlayerPrefs.GetInt(GetItemID(type, index), defaultVal ? 1 : 0).Equals(1);
    }

    public static bool IsItemInitialized(int type, int index, bool defaultVal)
    {
        return PlayerPrefs.HasKey(GetItemID(type, index));
    }

    public static void ClearPreferences()
    {
        PlayerPrefs.DeleteAll();
    }

    static string GetItemID(int type, int index = -1)
    {
        string item = "";

        switch (type)
        {
            case 0:
                item = "Carpet";
                break;
            case 1:
                item = "Bowl";
                break;
            case 2:
                item = "SlideShow";
                break;
        }

        if(index != -1) item += index.ToString();

        return item;
    }
}
