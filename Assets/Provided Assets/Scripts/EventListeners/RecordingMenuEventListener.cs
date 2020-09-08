using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


public class RecordingMenuEventListener : MonoBehaviour
{
    public enum RecordingStates
    {
        None,
        Started,
        Paused,
        Saving
    }

    public RecordingStates currentState;

    [System.Serializable]
    public class HeaderSettings
    {
        public Button[] buttons;
        public Image icon;
        public Sprite startRecoding;
        public Sprite pauseRecording;
    }
    public HeaderSettings headerSettings;

    [Serializable]
    public class RecordingSettings
    {
        public Recording recordingData;
        public Stopwatch stopwatch = new Stopwatch();

        [Range(120, 300)]
        public int recordingMaxTime;
        public float currentTime;
    }
    public RecordingSettings recordingSettings;

    [Header("Footer")]
    public Text Footertext;
    public Text timer;
    public Image fill;

    void MessageSender(string Message)
    {
        Footertext.text = Message;
    }
    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData
        {
            saveSession = false
        };

        AllRefs.I.dock.ManageButtons(data);

        recordingSettings.stopwatch = new Stopwatch();

        PopupManager.Instance.cancelAtion += OnRecordingDeleted;
    }

    private void OnDisable()
    {
        PopupManager.Instance.cancelAtion -= OnRecordingDeleted;
    }

    private void Update()
    {
        switch (currentState)
        {
            case RecordingStates.None:
                recordingSettings.stopwatch.Stop();
                recordingSettings.currentTime = 0;
                break;
            case RecordingStates.Started:
                Record();
                break;
            case RecordingStates.Saving:
                recordingSettings.stopwatch.Stop();
                recordingSettings.currentTime = 1;
                break;
        }

        SetTimer((int)recordingSettings.stopwatch.Elapsed.TotalSeconds);
    }

    bool isButtonPressed;

    void Record()
    {
        if (!recordingSettings.stopwatch.IsRunning)
            recordingSettings.stopwatch.Start();

        if (Input.GetMouseButtonDown(0) && !isButtonPressed)
        {
            isButtonPressed = true;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Bowl"))
            {
                AddRecordingSnipt(hit.transform);
            }
        }

        if (Input.GetMouseButtonUp(0) && isButtonPressed)
        {
            isButtonPressed = false;
        }
    }

    public void AddRecordingSnipt(Transform hit)
    {
        int hitItemIndex = Array.FindIndex(InventoryManager.Instance.allBowls.ToArray(), x => x == hit.GetComponent<Bowl>());
        double hitTime = recordingSettings.stopwatch.Elapsed.TotalSeconds;
        Debug.Log("hitItemIndex: " + hitItemIndex);
        Debug.Log("Seonds: " + hitTime);

        Recording.Snipt newSnipt = new Recording.Snipt { bowlIndex = hitItemIndex, time = (float)hitTime };
        recordingSettings.recordingData.Add(newSnipt);
    }

    void SetTimer(int seconds)
    {
        int minutes = seconds / 60;
        int remainingSeconds = seconds - minutes * 60;

        timer.text = minutes + ":" + (remainingSeconds > 9 ? "" + remainingSeconds : "0" + remainingSeconds);
        fill.fillAmount = (float)seconds / recordingSettings.recordingMaxTime;
    }

    void ChangeState(RecordingStates newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case RecordingStates.None:
                headerSettings.buttons[0].interactable = true;
                headerSettings.buttons[1].interactable = false;

                headerSettings.icon.sprite = headerSettings.startRecoding;

                recordingSettings.stopwatch.Stop();
                recordingSettings.stopwatch.Reset();
                break;
            case RecordingStates.Started:
                headerSettings.buttons[0].interactable = true;
                headerSettings.buttons[1].interactable = true;

                headerSettings.icon.sprite = headerSettings.pauseRecording;

                recordingSettings.stopwatch.Start();
                break;
            case RecordingStates.Paused:
                headerSettings.buttons[0].interactable = true;
                headerSettings.buttons[1].interactable = true;

                headerSettings.icon.sprite = headerSettings.startRecoding;

                recordingSettings.stopwatch.Stop();
                break;
            case RecordingStates.Saving:
                Debug.Log("Saving");
                headerSettings.buttons[0].interactable = false;
                headerSettings.buttons[1].interactable = true;

                headerSettings.icon.sprite = headerSettings.startRecoding;

                Microphone.End(string.Empty);
                recordingSettings.stopwatch.Stop();
                PopupManager.Instance.Show("Name Session", SaveRecording);
                break;
        }
    }


    public void OnClickBackButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
    }

    AudioClip newAudio;
    public void OnClickRecordButton()
    {
        switch (currentState)
        {
            case RecordingStates.None:
                recordingSettings.recordingData.Clear();
                ChangeState(RecordingStates.Started);
                try{newAudio = Microphone.Start(string.Empty, false, 300, 44100);}
                catch(Exception e){Debug.LogWarning("No microphone connected"); newAudio = null;};
                break;
                // case RecordingStates.Started:
                //     ChangeState(RecordingStates.Paused);
                //     break;
                // case RecordingStates.Paused:
                //     ChangeState(RecordingStates.Started);
                //     break;
        }
    }

    Coroutine savingC;

    void SaveRecording(string name)
    {
        string status = SessionManager.Instance.ValidateSessionName(name);

        if (status.Equals("Pass"))
        {
            SessionManager.Instance.SaveSession(name, recordingSettings.recordingData);
            
            if(savingC != null)
                StopCoroutine(savingC);

            savingC = StartCoroutine(SaveSound(name));
        }
        else
            PopupManager.Instance.ShowError(status);
    }

    public void OnClickSaveButton()
    {
        if (currentState.Equals(RecordingStates.Paused) || currentState.Equals(RecordingStates.Started))
            ChangeState(RecordingStates.Saving);
    }

    void OnRecordingDeleted() // not saved from popup
    {
        ChangeState(RecordingStates.None);
    }

    IEnumerator SaveSound(string name)
    {
        // Loading true
        PopupManager.Instance.loading.Show(true, "Saving Audio...");

        if(newAudio != null)
            yield return SavWav.Save(name, newAudio);
        // catch(Exception e){};

        PopupManager.Instance.loading.Show(false);
        PopupManager.Instance.Hide();
        ChangeState(RecordingStates.None);

        savingC = null;
    }
}
