using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class RecordingSnipt
{
    public double time;
    public int bowlIndex;
}

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
        public List<RecordingSnipt> recordingData;
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

    void Record()
    {
        if (!recordingSettings.stopwatch.IsRunning)
            recordingSettings.stopwatch.Start();

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Bowl"))
            {
                AddRecordingSnipt(hit.transform);
            }
        }
    }

    public void AddRecordingSnipt(Transform hit)
    {
        int hitItemIndex = Array.FindIndex(Inventory.Instance.allBowls, x => x == hit.GetComponent<Bowl>());

        Debug.Log("hitItemIndex: " + hitItemIndex);
        Debug.Log("Seonds: " + recordingSettings.stopwatch.Elapsed.Seconds);
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
                headerSettings.buttons[1].interactable = true;

                headerSettings.icon.sprite = headerSettings.startRecoding;

                recordingSettings.stopwatch.Stop();
                recordingSettings.stopwatch.Reset();
                break;
            case RecordingStates.Started:
                headerSettings.buttons[0].interactable = true;
                headerSettings.buttons[1].interactable = false;

                headerSettings.icon.sprite = headerSettings.pauseRecording;

                recordingSettings.stopwatch.Start();
                break;
            case RecordingStates.Paused:
                headerSettings.buttons[0].interactable = true;
                headerSettings.buttons[1].interactable = false;

                headerSettings.icon.sprite = headerSettings.startRecoding;

                recordingSettings.stopwatch.Stop();
                break;
            case RecordingStates.Saving:
                headerSettings.buttons[0].interactable = false;
                headerSettings.buttons[1].interactable = true;

                headerSettings.icon.sprite = headerSettings.startRecoding;

                recordingSettings.stopwatch.Stop();
                break;
        }
    }


    public void OnClickBackButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
    }

    public void OnClickRecordButton()
    {
        switch (currentState)
        {
            case RecordingStates.None:
                ChangeState(RecordingStates.Started);
                break;
            case RecordingStates.Started:
                ChangeState(RecordingStates.Paused);
                break;
            case RecordingStates.Paused:
                ChangeState(RecordingStates.Started);
                break;
        }
    }

    public void OnClickSaveButton()
    {
        if (currentState.Equals(RecordingStates.Paused) || currentState.Equals(RecordingStates.Started))
            ChangeState(RecordingStates.Saving);
    }
}
