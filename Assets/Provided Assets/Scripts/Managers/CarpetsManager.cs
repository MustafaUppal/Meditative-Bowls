using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetsManager : MonoBehaviour
{
    public int activeCarpetIndex;

    InventoryManager Inventory => InventoryManager.Instance;

    public void SetUpCarpets()
    {
        int itemType = (int)ShopMenuEventListener.ShopStates.Carpets;

        for (int i = 0; i < Inventory.GetItemCount(itemType); i++)
        {
            Inventory.GetItem(itemType, i).gameObject.SetActive(i.Equals(activeCarpetIndex));
        }
    }
}
