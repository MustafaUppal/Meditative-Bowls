using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;

    public enum States
    {
        BowlPosition,
        Recording,
        Mp3
    }

    public int[] defaultSession;

    private SessionData sessionData;

    public SessionData SessionData
    {
        get
        {
            if (sessionData == null)
                sessionData = PersistantData.Load();

            return sessionData;
        }
        set => sessionData = value;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void SaveSession(string name)
    {
        int[] tempsession = BowlsManager.Instance.activeBowlsIndexes;
        int[] session = new int[tempsession.Length];

        for (int i = 0; i < session.Length; i++)
        {
            session[i] = tempsession[i];
        }

        Session newSession = new Session { name = name, bowlsPositions = session };
        SessionData.AddSession(newSession);
        PersistantData.Save(SessionData);
    }

    public void SaveRecording()
    {
    }

    public void SaveMp3()
    {
    }

    public void LoadSession(string name)
    {
        int[] tempSession = SessionData.GetSession(name);
        int[] session = new int[tempSession.Length];

        for (int i = 0; i < session.Length; i++)
        {
            session[i] = tempSession[i];
        }
        
        BowlsManager.Instance.activeBowlsIndexes = session;
        BowlsManager.Instance.SetUpBowls();
    }

    public void DeleteSession(string name)
    {
        SessionData.DeleteSession(name);
        PersistantData.Save(SessionData);
    }

    public string ValidateSessionName(string name)
    {
        Debug.Log(SessionData);
        bool unique = !SessionData.AlreadyExists(name);
        bool empty = name.Length.Equals(0);

        if (empty) return "Name size should be greater than zero.";
        else if (!unique) return "Name already exists.";


        return "Pass";
    }
}

[System.Serializable]
public class SessionData
{
    public List<Session> sessions = new List<Session>();

    public int Length { get => sessions.Count; }

    public Session Get(int i)
    {
        return sessions[i];
    }
    public void AddSession(Session session)
    {
        sessions.Add(session);
    }

    public int[] GetSession(string name)
    {
        foreach (var session in sessions)
        {
            if (session.name.Equals(name))
                return session.bowlsPositions;
        }

        throw new KeyNotFoundException();
    }

    public void DeleteSession(string name)
    {
        for (int i = 0; i < sessions.Count; i++)
        {
            if (sessions[i].name.Equals(name))
            {
                sessions.RemoveAt(i);
                break;
            }
        }
    }

    public bool AlreadyExists(string name)
    {
        for (int i = 0; i < sessions.Count; i++)
        {
            if (sessions[i].name.Equals(name))
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public class Session
{
    public string name;

    public int[] bowlsPositions;
}
