using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentHandler : MonoBehaviour
{
    public GameObject tilePrefab;

    public void Init(int currentState)
    {
        int tilesCount = 0;
        
        for (int i = 0; i < transform.childCount; i++, tilesCount++)
        {
            if (i < Inventory.Instance.GetItemCount(currentState))
            {
                Item item = Inventory.Instance.GetItem(currentState, i);

                transform.GetChild(i).GetComponent<TileHandler>().SetTile(item.image, item.name, item.set);
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                if(!transform.GetChild(i).gameObject.activeInHierarchy)
                    break;
                transform.GetChild(i).gameObject.SetActive(false);
            }

        }

        for (int i = tilesCount; i < Inventory.Instance.GetItemCount(currentState); i++)
        {
            Item item = Inventory.Instance.GetItem(currentState, i);
            Instantiate(tilePrefab, transform).transform.GetChild(i).GetComponent<TileHandler>().SetTile(item.image, item.name, item.set);
            transform.GetChild(i).GetComponent<Button>().onClick.AddListener( delegate { AllRefs.I.shopMenu.OnClickItemButton(i); });
        }
    }

    public TileHandler GetTile(int index)
    {
        return transform.GetChild(index).GetComponent<TileHandler>();
    }
}
