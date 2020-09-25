using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetsManager : MonoBehaviour
{
    public int activeCarpetIndex;

    InventoryManager Inventory => InventoryManager.Instance;

    private void Start()
    {
        int itemType = (int)ShopMenuEventListener.ShopStates.Carpets;

        for (int i = 0; i < Inventory.GetItemCount(itemType); i++)
        {
            if (!PlayerPreferencesManager.IsItemInitialized(itemType, i, false))
                PlayerPreferencesManager.SetPurchasedState
                (
                    itemType, i,
                    Inventory.allCarpets[i].IsPurchased
                );
        }

        for (int i = 0; i < Inventory.GetItemCount(itemType); i++)
        {
            if(Inventory.allCarpets[i].CurrentState != Item.State.Loaded)
                Inventory.allCarpets[i].CurrentState 
                = PlayerPreferencesManager.GetPurchasedState(itemType, i, false) 
                ? Item.State.Purchased : Item.State.Locked;
        }
    }

    public void SetUpCarpets()
    {
        int itemType = (int)ShopMenuEventListener.ShopStates.Carpets;

        for (int i = 0; i < Inventory.GetItemCount(itemType); i++)
        {
            Inventory.GetItem(itemType, i).gameObject.SetActive(i.Equals(activeCarpetIndex));
        }
    }
}
