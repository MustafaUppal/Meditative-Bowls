﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileHandler : MonoBehaviour
{
    public Image image;
    public new Text name;
    public Text set;
    public Image bg;

    private void Awake()
    {
        bg = GetComponent<Image>();
    }

    public bool Highlight
    {
        set
        {
            bg.color = value 
            ? AllRefs.I.shopMenu.selectedItem.tileHighlight : 
            AllRefs.I.shopMenu.selectedItem.tileNormal;
        }
    }

    public void SetTile(Sprite sprite, string name, string set = "")
    {
        image.sprite = sprite;
        this.name.text = name;

        this.set.gameObject.SetActive(!set.Equals(""));
        this.set.text = set;
    }
}