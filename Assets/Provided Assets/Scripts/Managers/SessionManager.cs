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

    Coroutine recordingC;

    private SessionData sessionData;

    public InventoryManager Inventory => InventoryManager.Instance;

    public SessionData SessionData
    {
        get
        {
            if (sessionData == null)
                sessionData = PersistantData.Load();
            else if (sessionData.Length == 0)
                sessionData = PersistantData.Load();

            return sessionData;
        }
        set
        {
            Debug.Log("Saving session");
            sessionData = value;
        }
    }

    public bool RecordingIsPlaying
    {
        get
        {
            return recordingC != null;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public void SaveSession(string name, Recording recording = null)
    {
        int[] tempsession = Inventory.bowlsManager.activeBowlsIndexes;
        int[] session = new int[tempsession.Length];

        for (int i = 0; i < session.Length; i++)
        {
            session[i] = tempsession[i];
        }

        SessionData.Snipt newSession = new SessionData.Snipt { name = name, bowlsPositions = session, recording = recording };
        SessionData.AddSession(newSession);
        PersistantData.Save(SessionData);
    }

    public void SaveMp3()
    {
    }

    public void LoadSession(string name, bool playRecording = false)
    {
        SessionData.Snipt sessionSnipt = SessionData.GetSession(name);
        int[] tempSession = sessionSnipt.bowlsPositions;
        int[] session = new int[tempSession.Length];

        for (int i = 0; i < session.Length; i++)
        {
            session[i] = tempSession[i];
        }

        Inventory.bowlsManager.activeBowlsIndexes = session;
        Inventory.bowlsManager.SetUpBowls();

        if (playRecording && sessionSnipt.recording != null)
        {
            if (recordingC != null)
                StopCoroutine(recordingC);

            recordingC = StartCoroutine(PlayRecording(sessionSnipt.recording));
        }
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

    IEnumerator PlayRecording(Recording recording)
    {
        foreach (var snipt in recording.recodingSnipts)
        {
            yield return new WaitForSecondsRealtime(snipt.time);
            Transform bowl = InventoryManager.Instance.allBowls[snipt.bowlIndex].transform;

            InventoryManager.Instance.bowlsManager.PlaySound(bowl);
        }
    }
}

[System.Serializable]
public class SessionData
{
    [System.Serializable]
    public struct Snipt
    {
        public string name;

        public int[] bowlsPositions;

        public Recording recording;
    }

    public List<Snipt> sessionSnipts = new List<Snipt>();

    public int Length { get => sessionSnipts.Count; }

    public Snipt GetSession(int index)
    {
        return sessionSnipts[index];
    }

    public void AddSession(Snipt session)
    {
        sessionSnipts.Add(session);
    }

    public Snipt GetSession(string name)
    {
        foreach (var session in sessionSnipts)
        {
            if (session.name.Equals(name))
                return session;
        }

        throw new KeyNotFoundException();
    }

    public void DeleteSession(string name)
    {
        for (int i = 0; i < sessionSnipts.Count; i++)
        {
            if (sessionSnipts[i].name.Equals(name))
            {
                sessionSnipts.RemoveAt(i);
                break;
            }
        }
    }

    public bool AlreadyExists(string name)
    {
        for (int i = 0; i < sessionSnipts.Count; i++)
        {
            if (sessionSnipts[i].name.Equals(name))
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public class Recording
{
    [System.Serializable]
    public struct Snipt
    {
        public float time;
        public int bowlIndex;
    }

    public List<Snipt> recodingSnipts = new List<Snipt>();

    public void Add(Snipt snipt)
    {
        recodingSnipts.Add(snipt);
    }

    public void Clear()
    {
        recodingSnipts.Clear();
    }
}
