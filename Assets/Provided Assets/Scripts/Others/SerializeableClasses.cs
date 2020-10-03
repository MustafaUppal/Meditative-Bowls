using System;
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

            int mins = timer / 60;
            int secs = timer - mins * 60;

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
