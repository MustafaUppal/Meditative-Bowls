using System.Collections;
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
        content.SetItems((int)ShopMenuEventListener.ShopStates.Bowls);
        bowlPlacementSettings.Enable = true;
        bowlPlacementSettings.Init();
        // MenuManager.Instance.currentState = MenuManager.MenuStates.BowlPlacement;
    }

    private void OnDisable()
    {
        Inventory.bowlsManager.SetUpBowls(true);
        // MenuManager.Instance.currentState = MenuManager.MenuStates.Main;
    }

    public void Enable(bool enable)
    {
        bowlPlacementSettings.Enable = enable;
    }

    public void OnClickBowlButton(GameObject itemObj)
    {
        TileHandler tile = itemObj.GetComponent<TileHandler>();

        // if dock is not enabled means this bowl is not purchased, so do not select this bwol/l.
        if(!tile.toggle.gameObject.activeInHierarchy) return;

        if (!itemObj.GetComponent<TileHandler>().IsLoaded)
            OnClickBowlButton(itemObj.GetComponent<TileHandler>().index);
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
        // currentIndex = selected bowl index
        // bowl position to place selected bowl = index

        if (currentIndex.Equals(-1) || activeBowls[index] != -1)
            return;

        for (int i = 0; i < activeBowls.Length; i++)
        {
            if (activeBowls[i] != -1 && activeBowls[i].Equals(activeBowls[index]))
            {
                Inventory.allBowls[activeBowls[i]].CurrentState = Item.State.Purchased;
                panningvalue = Inventory.bowlsManager.BowlPanningValues[activeBowls[i]] = Inventory.allBowls[activeBowls[i]].PanStereo;
                break;
                // OnClickItemButton(activeBowls[i]);
                // activeBowls[i] = -1;
                // bowlPlacementSettings.SetText(i);
            }
        }

        activeBowls[index] = currentIndex;
        Inventory.allBowls[activeBowls[index]].CurrentState = Item.State.Loaded;
        Inventory.allBowls[activeBowls[index]].PanStereo = panningvalue;
        // Inventory.allBowls[activeBowls[index]].transform.localPosition = Inventory.bowlsManager.bowlsPositions[index];
        OnClickBowlButton(currentIndex);
        bowlPlacementSettings.SetText(index);

        currentIndex = -1;
        content.SetItems((int)ShopMenuEventListener.ShopStates.Bowls);
        // Inventory.bowlsManager.SetUpBowls(true);
    }

    public void OnClickOffBowl(int index)
    {
        for (int i = 0; i < activeBowls.Length; i++)
        {
            // find bowl in active bowls
            if (activeBowls[i] != -1 && activeBowls[i].Equals(index))
            {
                // set it to purchased
                Inventory.allBowls[activeBowls[i]].CurrentState = Item.State.Purchased;
                panningvalue = Inventory.bowlsManager.BowlPanningValues[activeBowls[i]] = Inventory.allBowls[activeBowls[i]].PanStereo;
                activeBowls[i] = -1;
                // Set it on placement button
                bowlPlacementSettings.SetText(i);
                content.SetItems((int)ShopMenuEventListener.ShopStates.Bowls);
                break;
                // OnClickItemButton(activeBowls[i]);
                // activeBowls[i] = -1;
                // bowlPlacementSettings.SetText(i);
            }
        }
    }
}
