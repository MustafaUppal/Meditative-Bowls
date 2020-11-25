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

        // if item is locked don't select
        if(tile.currentState.Equals(Item.State.Locked)) return;

        // if (!tile.currentState.Equals(Item.State.Loaded))
        OnClickBowlButton(tile.index);
    }

    public void OnClickBowlButton(int index)
    {
        // Item item = Inventory.GetItem(1, index);

        // Debug.Log("index: " + index);
        prevIndex = currentIndex;
        currentIndex = index;

        try { content.GetTile(prevIndex).Highlight = content.GetTile(prevIndex).GetNormal(); } catch (System.Exception e) { };
        content.GetTile(currentIndex).Highlight = HighlightProperties.Selected;
    }

    public void OnClickPlaceBowlButton(int index)
    {
        // currentIndex = selected bowl index
        // bowl position to place selected bowl = index

        if (currentIndex.Equals(-1))
            return;


        // IF BOWL TO BE PLACED IS ALREADY PLACED ON OTHER POSITION, TURN IT OFF
        for (int i = 0; i < activeBowls.Length; i++)
        {
            if (activeBowls[i].Equals(currentIndex))
            {
                activeBowls[i] = -1;
                bowlPlacementSettings.SetText(i);
                break;
            }
        }

        // Also remove already placed bowl on same position
        if (activeBowls[index] != -1)
        {
            Inventory.allBowls[activeBowls[index]].CurrentState = Item.State.Purchased;
        }

        activeBowls[index] = currentIndex;
        Inventory.allBowls[activeBowls[index]].CurrentState = Item.State.Loaded;
        Inventory.allBowls[activeBowls[index]].AudioSource.panStereo = InventoryManager.Instance.bowlsManager.BowlPanningValues[index];
        // Inventory.allBowls[activeBowls[index]].transform.localPosition = Inventory.bowlsManager.bowlsPositions[index];
        OnClickBowlButton(currentIndex);
        bowlPlacementSettings.SetText(index);

        // currentIndex = -1;
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

    public void Highlight(bool highlight)
    {

    }
}
