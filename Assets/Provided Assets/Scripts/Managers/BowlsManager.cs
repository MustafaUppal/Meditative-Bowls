using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BowlsManager : MonoBehaviour
{
    // -1 will be assigned if no bowl is on a place
    public int[] activeBowlsIndexes;
    public Vector3[] bowlsPositions;
    public float[] BowlPanningValues;

    InventoryManager Inventory => InventoryManager.Instance;
    public List<int> unusedBowls;

    /// <summary>
    /// Reposition all bowls according to active bowls indeces
    /// </summary>
    public void SetUpBowls()
    {
        int itemType = (int)ShopMenuEventListener.ShopStates.Bowls;
        unusedBowls = new List<int>();

        // Considering all bowls as unused
        for (int i = 0; i < Inventory.GetItemCount(itemType); i++)
        {
            unusedBowls.Add(i);
        }

        for (int i = 0; i < activeBowlsIndexes.Length; i++)
        {
            // -1 means no bowl is there, so look for next bowl
            if (activeBowlsIndexes[i].Equals(-1))
                continue;

            // Removing used bowls from the list
            unusedBowls.Remove(activeBowlsIndexes[i]);

            // Managing used bowls
            Inventory.allBowls[activeBowlsIndexes[i]].gameObject.SetActive(true);
            Inventory.allBowls[activeBowlsIndexes[i]].transform.localPosition = bowlsPositions[i];
            Inventory.allBowls[activeBowlsIndexes[i]].CurrentState = Item.State.Loaded;
        }

        BowlPanningValues = new float[activeBowlsIndexes.Length];

        for (int i = 0; i < activeBowlsIndexes.Length; i++)
        {
            BowlPanningValues[i] = Inventory.allBowls[activeBowlsIndexes[i]].GetComponent<AudioSource>().panStereo;
        }

        // Disabling all used bowls
        for (int i = 0; i < unusedBowls.Count; i++)
        {
            Inventory.allBowls[unusedBowls[i]].CurrentState = unusedBowls[i] < 25 ? Item.State.Purchased : Item.State.Locked;
            Inventory.allBowls[unusedBowls[i]].gameObject.SetActive(false);
        }
    }

    public void PlaySound(Transform hit)
    {
        Debug.Log("Playing sound: " + hit.name);
        hit.GetChild(0).gameObject.SetActive(true);
        hit.transform.GetChild(0).GetComponent<AudioLightSync>().emit = true;

        if (hit.GetComponent<AudioSource>().isPlaying)
            hit.GetComponent<AudioSource>().Stop();

        hit.GetComponent<AudioSource>().Play();
        //hit.transform.GetChild(0).gameObject.SetActive(true);
        // return hit;
    }
}
