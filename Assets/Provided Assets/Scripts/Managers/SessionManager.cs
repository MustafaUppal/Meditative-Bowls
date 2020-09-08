﻿using System.Collections;
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
    Coroutine recordingTimerC;

    [SerializeField]private SessionData sessionData;

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
        int[] tempSession = Inventory.bowlsManager.activeBowlsIndexes;
        int[] session = new int[tempSession.Length];
        float[] panings = new float[session.Length];
        float[] volumes = new float[session.Length];

        for (int i = 0; i < session.Length; i++)
        {
            // for creating a deep copy of session
            session[i] = tempSession[i];

            panings[i] = Inventory.allBowls[session[i]].GetComponent<AudioSource>().panStereo;
            volumes[i] = Inventory.allBowls[session[i]].GetComponent<AudioSource>().volume;
        }

        SessionData.Snipt newSession = new SessionData.Snipt 
        { 
            name = name, 
            volumes = volumes, 
            panings = panings, 
            bowlsPositions = session, 
            recording = recording 
        };
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
        float[] panings = sessionSnipt.panings;
        float[] volumes = sessionSnipt.volumes;
        int[] session = new int[tempSession.Length];

        for (int i = 0; i < session.Length; i++)
        {
            // to create a deep copy
            session[i] = tempSession[i];
            Inventory.allBowls[session[i]].GetComponent<AudioSource>().panStereo = panings[i];
            Inventory.allBowls[session[i]].GetComponent<AudioSource>().volume = volumes[i];
        }

        Inventory.bowlsManager.activeBowlsIndexes = session;
        Inventory.bowlsManager.SetUpBowls();

        if (playRecording && sessionSnipt.recording != null)
        {
            // if (recordingC != null)
            //     StopCoroutine(recordingC);

            // recordingC = StartCoroutine(PlayRecording(sessionSnipt.recording));
            
            if (recordingTimerC != null)
                StopCoroutine(recordingTimerC);

            recordingTimerC = StartCoroutine(RecordingTimer(sessionSnipt.recording));
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

    // IEnumerator PlayRecording(Recording recording)
    // {
        
            

    //         if (recordingTimerC != null)
    //             StopCoroutine(recordingTimerC);

    //         recordingTimerC = StartCoroutine(RecordingTimer(recording));

    //         foreach (var snipt in recording.recodingSnipts)
    //         {
    //             yield return new WaitForSecondsRealtime(snipt.time - lastSniptTime);
    //             lastSniptTime = snipt.time;
                
    //         }
        

    //     recordingC = null;
    //     AllRefs.I.mainMenu.ManageFooter(false);
    // }

    IEnumerator RecordingTimer(Recording recording)
    {
        float timer = 0;
        int lastIndex = recording.recodingSnipts.Count - 1;
        if(lastIndex < 0)
            StopAllCoroutines();
        float totalTime = recording.recodingSnipts[lastIndex].time;
        int i = 0;

        AllRefs.I.mainMenu.recordingFooter.InitLoopCount(1);
        AllRefs.I.mainMenu.ManageFooter(true);

        while (AllRefs.I.mainMenu.recordingFooter.currentLoop <= AllRefs.I.mainMenu.recordingFooter.loopCount)
        {
            AllRefs.I.mainMenu.recordingFooter.currentLoop++;
            timer = 0;

            for (i = 0 ; i < recording.recodingSnipts.Count;)
            {
                timer += Time.deltaTime;
                Debug.Log("timer/totalTime: " + timer / totalTime);
                AllRefs.I.mainMenu.recordingFooter.UpdateTimer(timer / totalTime, (int)timer);
                yield return null;

                if(recording.recodingSnipts[i].time < timer)
                {
                    Transform bowl = InventoryManager.Instance.allBowls[recording.recodingSnipts[i].bowlIndex].transform;
                    InventoryManager.Instance.bowlsManager.PlaySound(bowl);
                    i++;
                }

                if (!AllRefs.I.mainMenu.modes.playingRecording)
                {
                    StopAllCoroutines();
                }
            }
        }

        recordingTimerC = null;
        AllRefs.I.mainMenu.ManageFooter(false);
    }
}

[System.Serializable]
public class SessionData
{
    [System.Serializable]
    public struct Snipt
    {
        public string name;
        public float[] panings;
        public float[] volumes;

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
