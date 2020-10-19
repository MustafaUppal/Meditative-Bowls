﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace SerializeableClasses
{
    [Serializable]
    public struct HeaderSettings
    {
        public Image[] buttons;

        public Color highlighted;
        public Color unhighlighted;
    }

    [Serializable]
    public struct SelectedItemSettings
    {
        public int index;
        public int prevIndex;
        // private int position;

        [Header("Item References")]

        public Image largeImage;
        public Image image;

        public Text description;
        public Button b_itemActionButton;
        public Text t_itemActionButton;
        public Image i_itemActionButton;

        [Header("Values")]
        public Color[] buttonColors;
        public Sprite[] buttonIcons;

        [Space]
        public Sprite playSprite;
        public Sprite stopSprite;

        [Space]
        public Image thumbnail;
        public RawImage carpet;
        public RawImage bowl;
        public RawImage Largebowl;
        public bool isLargeView;

        [Header("Important References")]
        public GameObject bowlObj;
        public GameObject carpetObj;
        public GameObject bowlCam;
        public GameObject carpetCam;
        public GameObject resetButton;
        public GameObject expandImageButton;
        [Space]
        public GameObject playSoundButton;
        public Image playSoundIcon;

        // public int Position { get => position; set { Debug.Log("position " + position + " -> " + value); position = value;} }

        public void EnableImage(int index, bool enable = true)
        {
            carpet.gameObject.SetActive(index.Equals(0) ? enable : false);

            if (isLargeView)
                Largebowl.gameObject.SetActive(index.Equals(1) ? enable : false);
            else
                bowl.gameObject.SetActive(index.Equals(1) ? enable : false);

            thumbnail.gameObject.SetActive(index.Equals(2) ? enable : false);
        }

        public void EnableResetButton(bool enable)
        {
            resetButton.SetActive(enable);
        }

        public void EnableExpandButton(bool enable)
        {
            expandImageButton.SetActive(enable);
        }

        public void SetPlaySprite(bool play)
        {
            playSoundIcon.sprite = play ? stopSprite : playSprite;
        }
    }

    [Serializable]
    public struct BowlPlacementSettings
    {
        [SerializeField] private GameObject panel;

        public GameObject[] bowlsIcons;
        public Text[] bowlsText;
        public Image[] bGs;

        public bool Enable { set => panel.SetActive(value); }

        Bowl[] bowls => InventoryManager.Instance.allBowls.ToArray();
        int[] activeBowls => InventoryManager.Instance.bowlsManager.activeBowlsIndexes;

        public void Init()
        {
            for (int i = 0; i < activeBowls.Length; i++)
            {
                SetText(i);
            }
        }

        public void SetText(int index)
        {
            int i = activeBowls[index];

            Color color = Color.white;

            string text = "";

            if (i > -1)
            {
                color = bowls[i].lightColor;
                string name = bowls[i].name;
                string[] parts = name.Split(' ');
                text = parts[0] + (i < 7 ? (parts[2][1] - 32) : parts[2][0]);
                bowlsIcons[index].SetActive(true);
            }
            else
                bowlsIcons[index].SetActive(false);

            bowlsText[index].text = text;
            bGs[index].color = color;
        }
    }

    [System.Serializable]
    public class RecordingFooter
    {
        public GameObject root;

        [Header("Loop settings")]
        public bool loop;
        public Color loopingEnabled;
        public Color loopingDisabled;
        public Image loopButton;

        [Header("Timer Settings")]
        public Image timerFill;
        public Text timer;

        public void Enable(bool enable)
        {
            root.gameObject.SetActive(true);
        }

        public void SetLoop(bool overide, bool loop = false)
        {
            this.loop = overide ? loop : !this.loop;
            loopButton.color = this.loop ? loopingEnabled : loopingDisabled;
        }

        public void UpdateTimer(float percentage, int timer)
        {
            timerFill.fillAmount = percentage;

            int mins = timer / 60;
            int secs = timer - mins * 60;

            this.timer.text = mins + ":" + (secs < 10 ? "0" + secs : secs.ToString());
        }
    }

        [System.Serializable]
    public class BowlRandomizationSettings
    {
        public GameObject root;
        public NumberHandler hours;
        public NumberHandler mins;
        public NumberHandler secs;

        public float timer;

        [Header("Icon Settings")]
        public Image icon;
        public Sprite startIcon;
        public Sprite stopIcon;

        public void SetIcon(bool isStared)
        {
            icon.sprite = isStared ? stopIcon : startIcon;
        }

        public float TimeLimit => (hours.number * 60 * 60) + (mins.number * 60) + secs.number;
    }

    [System.Serializable]
    public class BowlEditingSettings
    {
        public GameObject root;
        public Text selectedBowlName;
        public Slider paning;
        public Slider volume;

        public void Set(string bowlName, float paningVal = 0, float volumeVal = 0)
        {
            selectedBowlName.text = bowlName;
            paning.value = paningVal;
            volume.value = volumeVal;
        }
    }

    [Serializable]
    public class MainMenuModes
    {
        public bool playingRecording;
        public bool slideShow;
        public bool placeBowls;
    }

    [Serializable]
    public class HighlightSettings
    {
        public Color tileHighlight;
        public Color tileNormal;
    }
}
