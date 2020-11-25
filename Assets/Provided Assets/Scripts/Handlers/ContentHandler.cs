using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using static UnityEngine.UI.Dropdown;

public class ContentHandler : MonoBehaviour
{
    public GameObject tilePrefab;
    public Dropdown categoryDropdown;
    public Dictionary<int, TileHandler> activeTiles = new Dictionary<int, TileHandler>();

    public InventoryManager Inventory => InventoryManager.Instance;
    TileHandler currentTile;

    [Header("Tile Settings")]
    public Sprite[] buttonIcons;
    public Color[] buttonColors;

    public void SetDropdown(int currentState, int setNumber = 1)
    {
        categoryDropdown.ClearOptions();

        Dictionary<int, bool> categories = new Dictionary<int, bool>();
        List<OptionData> categoryOptions = new List<OptionData>();

        for (int i = 0; i < Inventory.GetItemCount(currentState); i++)
        {
            Item item = Inventory.GetItem(currentState, i);

            if (!categories.ContainsKey(item.set))
            {
                categories.Add(item.set, true);
                categoryOptions.Add(new OptionData { text = item.setName });
                // Debug.Log("item.set:" + item.set);
            }
        }

        SetItems(currentState, setNumber);
        categoryDropdown.AddOptions(categoryOptions);
    }


    public int SetItems(int currentState, int setNumber = -1)
    {
        activeTiles.Clear();
        int tilesCount = 0;
        int firstIndex = -1;

        // Dictionary<int, int> loadedItem = GetLoadedItems(currentState);

        for (int i = 0; i < Inventory.GetItemCount(currentState); i++)
        {
            Item item = Inventory.GetItem(currentState, i);
            
            if(tilesCount < transform.childCount)
            {
                    // Debug.Log("item.Name:" + item.name);
                    // Debug.Log("item.Index: " + item.Index);
                    currentTile = transform.GetChild(tilesCount).GetComponent<TileHandler>();
                    currentTile.currentState = item.currentState;
                    currentTile.SetTile(item.image, item.name, item.Index);
                    currentTile.Highlight = currentTile.GetNormal();

                    activeTiles.Add(item.Index, currentTile);
                    transform.GetChild(tilesCount).gameObject.SetActive(true);
                    
                    tilesCount++;
                    if(firstIndex.Equals(-1)) firstIndex = item.Index;
            }
        }
        
        for(int i = tilesCount; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        return firstIndex;
    }

    Dictionary<int, int> GetLoadedItems(int type)
    {
        Dictionary<int, int> loadedItem = new Dictionary<int, int>();

        switch (type)
        {
            case 0: // carpets
                loadedItem.Add(Inventory.carpetsManager.activeCarpetIndex, 0);
                break;
            case 1: // bowls
                for (int i = 0; i < Inventory.bowlsManager.activeBowlsIndexes.Length; i++)
                {
                    if(Inventory.bowlsManager.activeBowlsIndexes[i] != -1)
                        loadedItem.Add(Inventory.bowlsManager.activeBowlsIndexes[i], i);
                }
                break;
            case 2: // slide show
                loadedItem.Add(Inventory.slideShowManager.activeMusicIndex, 0);
                break;
        }

        return loadedItem;
    }

    public TileHandler GetTile(int index)
    {
        // Debug.Log("Index: " + index);
        return activeTiles[index];
    }
}