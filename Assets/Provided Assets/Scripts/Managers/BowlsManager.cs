using System.Collections;
using System.Collections.Generic;
using SerializeableClasses;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BowlsManager : MonoBehaviour
{
    // -1 will be assigned if no bowl is on a place
    public int[] activeBowlsIndexes;
    public Vector3[] bowlsPositions;
    public float[] BowlPanningValues;
    public float soundFactor;

    InventoryManager Inventory => InventoryManager.Instance;
    public List<int> unusedBowls;

    public AudioSourceList playingAudios = new AudioSourceList();

    //  public int[] loadedBowlClips = new int[10];
    public List<bool> loadingBowls = new List<bool>();
    public bool showNotified = false;
    public bool closeNotified = false;
    public int bowlsCount = -1;
    public int spareAudioIndex = -1;

    // public BowlAudioSystem BowlAudioSystem = new BowlAudioSystem();

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

    private void Update()
    {
        for (int i = 0; i < playingAudios.Count; i++)
        {
            if (!playingAudios.sources[i].source.isPlaying)
            {
                playingAudios.Remove(i);
                AudioListener.volume = 1 - playingAudios.Count * soundFactor;
                i--;
                // Debug.Log("playingAudios: " + playingAudios.Count);
            }
        }

        // if(bowlsCount != loadingBowls.Count)
        //     Debug.Log("Loading Count: " + loadingBowls.Count);

        // bowlsCount = loadingBowls.Count;
        
        if(loadingBowls.Count > 0 && !showNotified)
        {
            showNotified = true;
            closeNotified = false;

            PopupManager.Instance.spinnerLoading.Show("Preparing Bowls...");
            AllRefs.I.objectSelection.EnableClick(false);
        }
        else if(loadingBowls.Count == 0 && !closeNotified)
        {
            showNotified = false;
            closeNotified = true;

            PopupManager.Instance.spinnerLoading.Hide();
            AllRefs.I.objectSelection.EnableClick(true);
        }
    }

    public void AddPlayingAudio(int id, AudioSource source)
    {
        if (playingAudios.ContainsKey(id) == -1)
        {
            playingAudios.Add(id, source);
            AudioListener.volume = 1 - playingAudios.Count * soundFactor;
            // Debug.Log("playingAudios: " + playingAudios.Count);
        }
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
            float y = Inventory.allBowls[activeBowlsIndexes[i]].transform.localPosition.y;
            Vector3 newPos = new Vector3(bowlsPositions[i].x, y, bowlsPositions[i].z);
            Inventory.allBowls[activeBowlsIndexes[i]].transform.localPosition = newPos;
            Inventory.allBowls[activeBowlsIndexes[i]].CurrentState = Item.State.Loaded;

            Inventory.allBowls[activeBowlsIndexes[i]].AudioSource.panStereo = SessionManager.Instance.SessionData.defaultSnipt.panings[i];
            Inventory.allBowls[activeBowlsIndexes[i]].AudioSource.volume = SessionManager.Instance.SessionData.defaultSnipt.volumes[i];
        }

        // BowlPanningValues = new float[Inventory.allBowls.Count];

        // for (int i = 0; i < Inventory.allBowls.Count; i++)
        // {
        //     BowlPanningValues[i] = Inventory.allBowls[i].GetComponent<AudioSource>().panStereo;
        // }

        // Disabling all used bowls
        for (int i = 0; i < unusedBowls.Count; i++)
        {
            if(Inventory.allBowls[unusedBowls[i]].CurrentState != Item.State.Locked)
                Inventory.allBowls[unusedBowls[i]].CurrentState = Item.State.Purchased;
            Inventory.allBowls[unusedBowls[i]].gameObject.SetActive(false);
        }
    }

    public void PlaySound(Transform hit)
    {
        // Debug.Log("Playing sound: " + hit.name);
        // hit.GetChild(0).gameObject.SetActive(true);
        // hit.transform.GetChild(0).GetComponent<AudioLightSync>().emit = true;
        
        hit.GetComponent<Bowl>().PlaySound();

        // if (hit.GetComponent<AudioSource>().isPlaying)
        //     hit.GetComponent<AudioSource>().Stop();

        // hit.GetComponent<AudioSource>().Play();
        //hit.transform.GetChild(0).gameObject.SetActive(true);
        // return hit;
    }

    public void AddBowlLoading()
    {
        showNotified = false;
        loadingBowls.Add(true);
    }

    public void RemoveBowlLoading()
    {
        closeNotified = false;
        loadingBowls.RemoveAt(loadingBowls.Count - 1);
    }
}
