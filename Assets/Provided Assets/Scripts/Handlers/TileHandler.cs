using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileHandler : MonoBehaviour
{
    public Image image;
    public new Text name;
    public Image stateIcon;
    public Image bg;
    public int Index;
    public ShopProductNames MyName;

    private void Awake()
    {
        bg = GetComponent<Image>();
    }

    public bool Highlight
    {
        set
        {
            bg.color = value
            ? AllRefs.I.highlightSettings.tileHighlight :
            AllRefs.I.highlightSettings.tileNormal;
        }
    }

    bool isLoaded;
    public bool IsLoaded
    {
        get => isLoaded;
        set { isLoaded = value; }
    }

    public void SetTile(Sprite sprite, string name, int index, int iconIndex)
    {
        image.sprite = sprite;
        this.name.text = name;
        this.Index = index;

        if (iconIndex != -1)
        {
            this.stateIcon.sprite = AllRefs.I.tilesContainer.buttonIcons[iconIndex];
            this.stateIcon.color = AllRefs.I.tilesContainer.buttonColors[iconIndex];
        }
    }
}
