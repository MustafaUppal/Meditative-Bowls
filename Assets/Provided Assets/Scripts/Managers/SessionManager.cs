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

    public SessionData.Snipt currentSessionSnipt;
    Coroutine recordingC;
    Coroutine recordingTimerC;

    [SerializeField] private SessionData sessionData;

    public InventoryManager Inventory => InventoryManager.Instance;

    public SessionData SessionData
    {
        get
        {
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
        // Debug.Log("Awake");

        Instance = this;
    }

    public void Init()
    {
        if (sessionData == null || sessionData.Length == 0)
        {
            sessionData = PersistantData.Load();
        }
    }

    public void Save()
    {
        PersistantData.Save(SessionData);
    }

    public void SaveSession(string name, Recording recording = null)
    {
        int[] tempSession = Inventory.bowlsManager.activeBowlsIndexes;

        SessionData.Snipt newSession = new SessionData.Snipt { name = name };

        if (recording != null)
        {
            newSession.recording = new Recording();
            newSession.recording.DeepCopy(recording);
        }

        newSession.bowlsPositions = new int[tempSession.Length];
        newSession.panings = new float[tempSession.Length];
        newSession.volumes = new float[tempSession.Length];

        for (int i = 0; i < tempSession.Length; i++)
        {
            // for creating a deep copy of session
            newSession.bowlsPositions[i] = tempSession[i];

            if (newSession.bowlsPositions[i] != -1)
            {
                Debug.Log("Saving: " + Inventory.allBowls[newSession.bowlsPositions[i]].AudioSource.panStereo);
                newSession.panings[i] = Inventory.allBowls[newSession.bowlsPositions[i]].AudioSource.panStereo;
                newSession.volumes[i] = Inventory.allBowls[newSession.bowlsPositions[i]].AudioSource.volume;
            }
        }

        // SessionData.Snipt newSession = new SessionData.Snipt {
        //     name = name,
        //     volumes = volumes,
        //     panings = panings,
        //     bowlsPositions = session,
        //     recording = recording
        // };

        SessionData.AddSession(newSession);
        PersistantData.Save(SessionData);
    }

    public void SaveMp3() { }

    public void LoadSession(string name, bool playRecording = false)
    {
        currentSessionSnipt = SessionData.GetSession(name);

        Inventory.bowlsManager.activeBowlsIndexes = new int[currentSessionSnipt.bowlsPositions.Length];

        for (int i = 0; i < Inventory.bowlsManager.activeBowlsIndexes.Length; i++)
        {
            // to create a deep copy
            Inventory.bowlsManager.activeBowlsIndexes[i] = currentSessionSnipt.bowlsPositions[i];

            if (Inventory.bowlsManager.activeBowlsIndexes[i] != -1)
            {
                Inventory.allBowls[Inventory.bowlsManager.activeBowlsIndexes[i]].AudioSource.panStereo = currentSessionSnipt.panings[i];
                Inventory.allBowls[Inventory.bowlsManager.activeBowlsIndexes[i]].AudioSource.volume = currentSessionSnipt.volumes[i];
            }
        }

        Inventory.bowlsManager.SetUpBowls(true);

        PlayRecording(playRecording);
    }

    public void PlayRecording(bool play)
    {
        if(currentSessionSnipt.recording != null)
        {
            if (recordingTimerC != null)
            {
                StopCoroutine(recordingTimerC);
                recordingTimerC = null;
            }

            if(play)
                recordingTimerC = StartCoroutine(RecordingTimer(currentSessionSnipt.recording));
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
        if (lastIndex < 0)
        {
            StopCoroutine(recordingTimerC);
            recordingTimerC = null;
        }

        // totalTime = last click + length of last sound
        float totalTime = recording.endTime;
        int i = 0;
        bool isPlaying = true;

        AllRefs.I.mainMenu.recordingFooter.SetLoop(true, false);
        AllRefs.I.mainMenu.ManageFooter(true);
        AllRefs.I.mainMenu.isPlaying = isPlaying;
        AllRefs.I.mainMenu.recordindPlayBtn.SetIcon(isPlaying);

        while (true)
        {
            timer = 0;

            for (i = 0; isPlaying;)
            {
                timer += Time.deltaTime;
                // Debug.Log("timer/totalTime: " + timer / totalTime);
                AllRefs.I.mainMenu.recordingFooter.UpdateTimer(timer / totalTime, (int)timer);
                yield return null;

                if (i <= lastIndex && recording.recodingSnipts[i].time < timer)
                {
                    InventoryManager.Instance.allBowls[recording.recodingSnipts[i].bowlIndex].PlaySound();
                    i++;
                }

                if (!AllRefs.I.mainMenu.modes.playingRecording)
                    StopAllCoroutines();

                isPlaying = recording.endTime > timer;
                // if(i - 1 == lastIndex)
                // {
                //     isPlaying = InventoryManager.Instance.allBowls[recording.recodingSnipts[i - 1].bowlIndex].AudioSource.isPlaying;
                // }
            }

            if (!AllRefs.I.mainMenu.recordingFooter.loop)
                break;
            else 
                isPlaying = true;
        }

        recordingTimerC = null;
        AllRefs.I.mainMenu.isPlaying = true;
        AllRefs.I.mainMenu.OnClickPlayButton();
        GameManager.Instance.SoundRestart();
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
    public Snipt defaultSnipt;

    public void InitDefault(int[] activeBowls, bool overwrite = false)
    {
        if (defaultSnipt.name == null || overwrite)
        {
            if (defaultSnipt.name == null)
            {
                defaultSnipt.bowlsPositions = new int[activeBowls.Length];
                defaultSnipt.volumes = new float[activeBowls.Length];
                defaultSnipt.panings = new float[activeBowls.Length];
            }


            defaultSnipt.name = "Default";

            for (int i = 0; i < activeBowls.Length; i++)
            {
                defaultSnipt.bowlsPositions[i] = activeBowls[i];

                if (activeBowls[i] != -1)
                {
                    defaultSnipt.volumes[i] = InventoryManager.Instance.allBowls[activeBowls[i]].AudioSource.volume;
                    defaultSnipt.panings[i] = InventoryManager.Instance.allBowls[activeBowls[i]].AudioSource.panStereo;
                }
            }
        }
    }

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
    public float endTime;

    public void DeepCopy(Recording recording)
    {
        if(recording == null) return;
        recodingSnipts = new List<Snipt>();

        foreach (var recodingSnipt in recording.recodingSnipts)
        {
            Add(new Snipt{ time = recodingSnipt.time, bowlIndex = recodingSnipt.bowlIndex});
        }

        endTime = recording.endTime;
    }

    public void Add(Snipt snipt)
    {
        recodingSnipts.Add(snipt);
    }

    public void Clear()
    {
        recodingSnipts.Clear();
    }
}