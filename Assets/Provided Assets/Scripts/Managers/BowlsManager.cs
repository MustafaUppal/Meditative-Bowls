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

    private void Start() 
    {
        int itemType = (int)ShopMenuEventListener.ShopStates.Bowls;

        for (int i = 0; i < Inventory.GetItemCount(itemType); i++)
        {
            if (!PlayerPreferencesManager.IsItemInitialized(itemType, i, false))
                PlayerPreferencesManager.SetPurchasedState
                (
                    itemType, i,
                    Inventory.allBowls[i].IsPurchased
                );
        }

        for (int i = 0; i < Inventory.GetItemCount(itemType); i++)
        {
            if(Inventory.allBowls[i].CurrentState != Item.State.Loaded)
                Inventory.allBowls[i].CurrentState 
                = PlayerPreferencesManager.GetPurchasedState(itemType, i, false) 
                ? Item.State.Purchased : Item.State.Locked;
        }
    }

    private void OnDestroy() 
    {
        SessionManager.Instance.SessionData.InitDefault(activeBowlsIndexes);
        SessionManager.Instance.Save();
    }

    /// <summary>
    /// Reposition all bowls according to active bowls indeces
    /// </summary>
    public void SetUpBowls(bool overwirte = false)
    {
        SessionManager.Instance.SessionData.InitDefault(activeBowlsIndexes, overwirte);

        int itemType = (int)ShopMenuEventListener.ShopStates.Bowls;
        unusedBowls = new List<int>();

        // Considering all bowls as unused
        for (int i = 0; i < Inventory.GetItemCount(itemType); i++)
        {
            unusedBowls.Add(i);
        }

        for (int i = 0; i < activeBowlsIndexes.Length; i++)
        {
            // Debug.Log(i + " -> " + SessionManager.Instance.SessionData.defaultSnipt.bowlsPositions[i]);
            activeBowlsIndexes[i] = SessionManager.Instance.SessionData.defaultSnipt.bowlsPositions[i];

            // -1 means no bowl is there, so look for next bowl
            if (activeBowlsIndexes[i].Equals(-1))
                continue;

            // Removing used bowls from the list
            unusedBowls.Remove(activeBowlsIndexes[i]);

            // Managing loaded bowls
            Inventory.allBowls[activeBowlsIndexes[i]].gameObject.SetActive(true);
            Inventory.allBowls[activeBowlsIndexes[i]].transform.localPosition = bowlsPositions[i];
            Inventory.allBowls[activeBowlsIndexes[i]].CurrentState = Item.State.Loaded;

            Inventory.allBowls[activeBowlsIndexes[i]].PanStereo = SessionManager.Instance.SessionData.defaultSnipt.panings[i];
            Inventory.allBowls[activeBowlsIndexes[i]].Volume = SessionManager.Instance.SessionData.defaultSnipt.volumes[i];
        }

        BowlPanningValues = new float[Inventory.allBowls.Count];

        for (int i = 0; i < Inventory.allBowls.Count; i++)
        {
            BowlPanningValues[i] = Inventory.allBowls[i].GetComponent<AudioSource>().panStereo;
        }

        // Disabling all used bowls
        for (int i = 0; i < unusedBowls.Count; i++)
        {
            // Inventory.allBowls[unusedBowls[i]].CurrentState = /* unusedBowls[i] < 25 ? Item.State.Purchased :  */Item.State.Locked;
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
