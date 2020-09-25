using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicsManager : MonoBehaviour
{
    public int activeMusicIndex = -1;

    InventoryManager Inventory => InventoryManager.Instance;

    private void Start()
    {
        int itemType = (int)ShopMenuEventListener.ShopStates.BG_Musics;

        for (int i = 0; i < Inventory.GetItemCount(itemType); i++)
        {
            if (!PlayerPreferencesManager.IsItemInitialized(itemType, i, false))
                PlayerPreferencesManager.SetPurchasedState
                (
                    itemType, i,
                    Inventory.allMusics[i].IsPurchased
                );
        }

        for (int i = 0; i < Inventory.GetItemCount(itemType); i++)
        {
            if(Inventory.allMusics[i].CurrentState != Item.State.Loaded)
                Inventory.allMusics[i].CurrentState 
                = PlayerPreferencesManager.GetPurchasedState(itemType, i, false) 
                ? Item.State.Purchased : Item.State.Locked;
        }
    }

    public void SetUpMusic()
    {
        // int itemType = (int)ShopMenuEventListener.ShopStates.BG_Musics;

        // audioSource.Clip = Inventory.GetItem(itemType, activeMusicIndex).audioClip;
        // audioSource.Play()
    }
}
