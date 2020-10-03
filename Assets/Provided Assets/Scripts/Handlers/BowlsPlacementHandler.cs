﻿using System.Collections;
using System.Collections.Generic;
using SerializeableClasses;
using UnityEngine;
using UnityEngine.UI;

public class BowlsPlacementHandler : MonoBehaviour
{
    public ContentHandler content;

    public BowlPlacementSettings bowlPlacementSettings;

    int currentIndex = -1;
    int prevIndex = -1;
    float panningvalue = 0;

    InventoryManager Inventory => InventoryManager.Instance;
    int[] activeBowls => InventoryManager.Instance.bowlsManager.activeBowlsIndexes;

    private void OnEnable()
    {
        content.SetPurchasedBowls();
        bowlPlacementSettings.Enable = true;
        bowlPlacementSettings.Init();
        // MenuManager.Instance.currentState = MenuManager.MenuStates.BowlPlacement;
    }

    private void OnDisable()
    {
        Inventory.InitScene(true);
        // MenuManager.Instance.currentState = MenuManager.MenuStates.Main;
    }

    public void Enable(bool enable)
    {
        bowlPlacementSettings.Enable = enable;
    }

    public void OnClickBowlButton(GameObject itemObj)
    {
        if (!itemObj.GetComponent<TileHandler>().IsLoaded)
            OnClickBowlButton(itemObj.GetComponent<TileHandler>().Index);
    }

    public void OnClickBowlButton(int index)
    {
        // Item item = Inventory.GetItem(1, index);

        Debug.Log("index: " + index);
        prevIndex = currentIndex;
        currentIndex = index;

        try { content.GetTile(prevIndex).Highlight = false; } catch (System.Exception e) { };
        content.GetTile(currentIndex).Highlight = true;
    }

    public void OnClickPlaceBowlButton(int index)
    {
        if (currentIndex.Equals(-1))
            return;
        for (int i = 0; i < activeBowls.Length; i++)
        {
            if (activeBowls[i] != -1 && activeBowls[i].Equals(activeBowls[index]))
            {
                Inventory.allBowls[activeBowls[i]].CurrentState = Item.State.Purchased;
                panningvalue = Inventory.bowlsManager.BowlPanningValues[activeBowls[i]] = Inventory.allBowls[activeBowls[i]].GetComponent<AudioSource>().panStereo;
                break;
                // OnClickItemButton(activeBowls[i]);
                // activeBowls[i] = -1;
                // bowlPlacementSettings.SetText(i);
            }
        }

        activeBowls[index] = currentIndex;
        Inventory.allBowls[activeBowls[index]].CurrentState = Item.State.Loaded;
        Inventory.allBowls[activeBowls[index]].GetComponent<AudioSource>().panStereo = panningvalue;
        OnClickBowlButton(currentIndex);
        bowlPlacementSettings.SetText(index);

        currentIndex = -1;
        content.SetPurchasedBowls();
    }
}
