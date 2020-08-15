using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicsManager : MonoBehaviour
{
    public int activeMusicIndex;

    InventoryManager Inventory => InventoryManager.Instance;

    public void SetUpMusic()
    {
        int itemType = (int)ShopMenuEventListener.ShopStates.BG_Musics;

        // audioSource.Clip = Inventory.GetItem(itemType, activeMusicIndex).audioClip;
        // audioSource.Play()
    }
}
