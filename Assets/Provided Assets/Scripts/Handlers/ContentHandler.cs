using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentHandler : MonoBehaviour
{
    public GameObject tilePrefab;

    public InventoryManager Inventory => InventoryManager.Instance;

    public void Init(int currentState)
    {
        int tilesCount = 0;
        
        for (int i = 0; i < transform.childCount; i++, tilesCount++)
        {
            if (i < Inventory.GetItemCount(currentState))
            {
                Item item = Inventory.GetItem(currentState, i);

                transform.GetChild(i).GetComponent<TileHandler>().SetTile(item.image, item.name, item.Index);
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                if(!transform.GetChild(i).gameObject.activeInHierarchy)
                    break;
                transform.GetChild(i).gameObject.SetActive(false);
            }

        }

        for (int i = tilesCount; i < Inventory.GetItemCount(currentState); i++)
        {
            Item item = Inventory.GetItem(currentState, i);
            Instantiate(tilePrefab, transform).transform.GetChild(i).GetComponent<TileHandler>().SetTile(item.image, item.name, item.set);
            transform.GetChild(i).GetComponent<Button>().onClick.AddListener( delegate { AllRefs.I.shopMenu.OnClickItemButton(i); });
        }
    }

    public TileHandler GetTile(int index)
    {
        return transform.GetChild(index).GetComponent<TileHandler>();
    }
}







// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class ContentHandler : MonoBehaviour
// {
//     public GameObject tilePrefab;

//     public SetHandler[] sets;
//     public List<TileHandler> activeTiles = new List<TileHandler>();

//     public InventoryManager Inventory => InventoryManager.Instance;

//     public void Init(int currentState)
//     {
//         activeTiles.Clear();
//         int tilesCount = 0;
//         int index = 0;
//         Dictionary<int, bool> setsInitialized = new Dictionary<int, bool>() { { 0, false }, { 1, false }, { 2, false }, { 3, false } };

//         for (int i = 0; i < sets.Length; i++, tilesCount++)
//         {
//             for (int j = 0; j < transform.GetChild(i).childCount - 1; j++)
//             {
//                 index = i * transform.GetChild(i).childCount + j;
//                 Debug.Log("Index: " + index);
//                 if (index < Inventory.GetItemCount(currentState))
//                 {
//                     // first tile of evey set will define set name

//                     Item item = Inventory.GetItem(currentState, index);
//                     Debug.Log("item: " + (item.gameObject.name));
//                     Debug.Log("item.name: " + (item.name));
//                     Debug.Log("item.setName: " + (item.setName));
//                     Debug.Log("----------------------------------");
//                     if (!setsInitialized[item.set - 1])
//                     {
//                         setsInitialized[item.set - 1] = true;
//                         transform.GetChild(item.set - 1).GetChild(0).GetChild(0).GetComponent<Text>().text = item.setName;
//                     }
//                     activeTiles.Add(transform.GetChild(item.set - 1).GetChild(j + 1).GetComponent<TileHandler>());
//                     activeTiles[index].SetTile(item.image, item.name, item.Index);
//                     transform.GetChild(item.set - 1).GetChild(j + 1).gameObject.SetActive(true);
//                     sets[i].childrenInUse[j] = true;
//                 }
//                 else
//                 {
//                     if (!transform.GetChild(i).GetChild(j + 1).gameObject.activeInHierarchy)
//                         break;
//                     transform.GetChild(i).GetChild(j + 1).gameObject.SetActive(false);
//                     sets[i].childrenInUse[j] = false;
//                 }
//             }
//             transform.GetChild(i).gameObject.SetActive(setsInitialized[i]);
//         }

//         // for (int i = tilesCount; i < Inventory.GetItemCount(currentState); i++)
//         // {
//         //     Item item = Inventory.GetItem(currentState, i);
//         //     Instantiate(tilePrefab, transform).transform.GetChild(i).GetComponent<TileHandler>().SetTile(item.image, item.name);
//         //     transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { AllRefs.I.shopMenu.OnClickItemButton(i); });
//         // }
//     }

//     public TileHandler GetTile(int index)
//     {
//         return activeTiles[index];
//     }
// }
