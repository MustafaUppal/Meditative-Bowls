using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PersistantData
{
    public static void Save(SessionData sessionData)
    {
        // Debug.Log(sessionData.Length);
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        FileStream file = null;

        if (!File.Exists(Application.persistentDataPath + "/appData.dat"))
            file = File.Create(Application.persistentDataPath + "/appData.dat");
        else
            file = File.OpenWrite(Application.persistentDataPath + "/appData.dat");

        binaryFormatter.Serialize(file, sessionData);
        file.Close();
    }

    public static SessionData Load()
    {
        if (!File.Exists(Application.persistentDataPath + "/appData.dat"))
            Save(new SessionData());
            
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/appData.dat", FileMode.Open);
        SessionData sessionData = binaryFormatter.Deserialize(file) as SessionData;

        // Debug.Log("L: " + sessionData.Length);
        file.Close();

        return sessionData;
    }
}