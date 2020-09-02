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

        public void ActivateImage(int index)
        {

            carpet.gameObject.SetActive(index.Equals(0));
            bowl.gameObject.SetActive(index.Equals(1));
            thumbnail.gameObject.SetActive(index.Equals(2));

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
}
