using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideShowManager : MonoBehaviour
{
    public int activeMusicIndex = -1;

    InventoryManager Inventory => InventoryManager.Instance;

    private void Start()
    {
        int itemType = (int)ShopMenuEventListener.ShopStates.SlideShows;

        for (int i = 0; i < Inventory.GetItemCount(itemType); i++)
        {
            if (!PlayerPreferencesManager.IsItemInitialized(itemType, i, false))
                PlayerPreferencesManager.SetPurchasedState
                (
                    itemType, i,
                    Inventory.allSlideShows[i].IsPurchased
                );
        }

        for (int i = 0; i < Inventory.GetItemCount(itemType); i++)
        {
            if (Inventory.allSlideShows[i].CurrentState != Item.State.Loaded)
                Inventory.allSlideShows[i].CurrentState
                = PlayerPreferencesManager.GetPurchasedState(itemType, i, false)
                ? Item.State.Purchased : Item.State.Locked;
        }
    }

    public void SetUpSlideShow()
    {
        // int itemType = (int)ShopMenuEventListener.ShopStates.BG_Musics;

        // audioSource.Clip = Inventory.GetItem(itemType, activeMusicIndex).audioClip;
        // audioSource.Play()
    }
}
