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

        [Header("Item References")]

        public Image image;
        public Text description;
        public Button b_itemActionButton;
        public Text t_itemActionButton;
        public Image i_itemActionButton;

        [Header("Values")]
        public Color[] buttonColors;
        public Sprite[] buttonIcons;

        public Color tileHighlight;
        public Color tileNormal;

        [Header("Important References")]
        public GameObject bowlObj;
        public GameObject carpetObj;
        public GameObject bowlCam;
        public GameObject carpetCam;
        public Image thumbnail;
        public RawImage carpet;
        public RawImage bowl;
        public GameObject resetButton;

        public void EnableImage(int index, bool enable = true)
        {
            carpet.gameObject.SetActive(index.Equals(0) ? enable: false);
            bowl.gameObject.SetActive(index.Equals(1) ? enable: false);
            thumbnail.gameObject.SetActive(index.Equals(2) ? enable: false);
        }
    }

    [Serializable]
    public struct BowlPlacementSettings
    {
        [SerializeField] private GameObject panel;
        public Text[] bowlsText;

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

            Color color = Color.black;
            string text = "Empty\nPosition";

            if (i > -1)
            {
                color = bowls[i].lightColor;
                text = bowls[i].name + "\n(" + bowls[i].setName + ")";
            }

            bowlsText[index].text = text;
            bowlsText[index].color = color;
        }
    }

    [System.Serializable]
    public class RecordingFooter
    {
        public GameObject root;
        public int loopCount = 0;
        public int currentLoop = 1;
        public Text loopCountText;
        public Button minusButton;
        public Image timerFill;
        public Text timer;

        public void Enable(bool enable)
        {
            root.gameObject.SetActive(true);
            minusButton.interactable = false;
        }

        public void UpdateLoopCount(int value, bool useExact = false)
        {
            loopCount = useExact ? value : loopCount + value;

            minusButton.interactable = loopCount > currentLoop;
            loopCountText.text = loopCount.ToString();
        }

        public void UpdateTimer(float percentage, int timer)
        {
            timerFill.fillAmount = percentage;

            int mins = timer/60;
            int secs = timer - mins*60;

            this.timer.text = mins + ":" + (secs < 10 ? "0" + secs : secs.ToString());
        }

        public void InitLoopCount(int val)
        {
            loopCount = currentLoop = val;
        }
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
}
