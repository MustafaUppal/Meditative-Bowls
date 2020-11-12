using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using SerializeableClasses;


public class RecordingMenuEventListener : MonoBehaviour
{
    public RecordingStates currentState;
    public RecordingSettings recordingSettings;
    public ButtonOnOffSettings playStopButton;

    [Header("Footer")]
    public Text Footertext;
    public Text timer;
    public Image fill;

    // Private Variables
    AudioClip newAudio;
    Coroutine microPhoneC;
    bool wavIncluded = false;
    bool isButtonPressed;
    Coroutine savingC;

    // *******************
    // * Unity Callbacks *
    // *******************

    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData
        {
            saveSession = false
        };

        AllRefs.I.dock.ManageButtons(data);

        recordingSettings.stopwatch = new Stopwatch();

        PopupManager.Instance.cancelAtion += OnRecordingDeleted;
        AllRefs.I.objectSelection.EnableClick(true);
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

    // ************************
    // * Buttons Click Events *
    // ************************

    public void OnClickBackButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
    }

    public void OnClickRecordButton()
    {
        switch (currentState)
        {
            case RecordingStates.None:
                recordingSettings.recordingData.Clear();
                // if(microPhoneC != null)
                //     StopCoroutine(microPhoneC);

                // microPhoneC = StartCoroutine(GetMicrophone());
                ChangeState(RecordingStates.Started);
                break;
            case RecordingStates.Started:
                ChangeState(RecordingStates.Saving);
                break;
                // case RecordingStates.Paused:
                //     ChangeState(RecordingStates.Started);
                //     break;
        }
    }

    public void OnClickSaveButton()
    {
        if (currentState.Equals(RecordingStates.Paused) || currentState.Equals(RecordingStates.Started))
            ChangeState(RecordingStates.Saving);
    }

    void OnClickMessagePopupButton()
    {
        ChangeState(RecordingStates.Started);
    }

    // *******************
    // * Functionalities *
    // *******************

    void MessageSender(string Message)
    {
        Footertext.text = Message;
    }

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
                playStopButton.SetIcon(false);

                recordingSettings.stopwatch.Stop();
                recordingSettings.stopwatch.Reset();
                break;
            case RecordingStates.Started:
                playStopButton.SetIcon(true);

                recordingSettings.stopwatch.Start();
                break;
            case RecordingStates.Paused:
                // playStopButton.icon.sprite = playStopButton.startRecoding;

                recordingSettings.stopwatch.Stop();
                break;
            case RecordingStates.Saving:
                playStopButton.SetIcon(false);

                if (wavIncluded) Microphone.End(string.Empty);
                double hitTime = recordingSettings.stopwatch.Elapsed.TotalSeconds;
                recordingSettings.recordingData.endTime = (float)hitTime;
                recordingSettings.stopwatch.Stop();
                
                PopupManager.Instance.Show("Save Recording", SaveRecording);
                break;
        }
    }

    IEnumerator GetMicrophone()
    {
        Debug.Log("Microphone count: " + Microphone.devices.Length);
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Microphone.devices.Length > 0 && Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Debug.Log("We received the mic");
            ChangeState(RecordingStates.Started);
            newAudio = Microphone.Start(string.Empty, false, 300, 44100);
            wavIncluded = true;
            //StartRecording
        }
        else
        {
            Debug.Log("We encountered an error");
            wavIncluded = false;
            PopupManager.Instance.messagePopup.Show("Access Denied!", "Failed to get microphone access from device. Please press record button again.", "OK", OnClickMessagePopupButton);
            //Error
        }

        microPhoneC = null;
    }

    void SaveRecording(string name)
    {
        string status = SessionManager.Instance.ValidateSessionName(name);

        if (status.Equals("Pass"))
        {
            SessionManager.Instance.SaveSession(name, recordingSettings.recordingData);

            if (savingC != null)
                StopCoroutine(savingC);

            savingC = StartCoroutine(SaveSound(name));
        }
        else
            PopupManager.Instance.ShowError(status);
    }

    void OnRecordingDeleted() // not saved from popup
    {
        ChangeState(RecordingStates.None);
    }

    IEnumerator SaveSound(string name)
    {
        // Loading true
        PopupManager.Instance.loading.Show(true, "Saving Audio...");

        // yield return SavWav.Save(name, newAudio);
        yield return null;


        PopupManager.Instance.loading.Show(false);
        PopupManager.Instance.Hide();
        ChangeState(RecordingStates.None);

        savingC = null;
    }

    // ************************
    // * Serializable Classes *
    // ************************

    [Serializable]
    public class RecordingSettings
    {
        public Recording recordingData;
        public Stopwatch stopwatch = new Stopwatch();

        [Range(120, 300)]
        public int recordingMaxTime;
        public float currentTime;
    }

    public enum RecordingStates
    {
        None,
        Started,
        Paused,
        Saving
    }
}
