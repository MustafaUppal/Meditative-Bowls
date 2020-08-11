using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Managers")]
    public BowlsManager bowlsManager;
    public CarpetsManager carpetsManager;
    public MusicsManager musicsManager;

    [Header("Items")]
    public List<Bowl> allBowls;
    public List<Carpet> allCarpets;
    public List<BG_Music> allMusics;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance)
            Destroy(gameObject);
        else
        {
            Instance = this;

            bowlsManager = transform.GetChild(0).GetComponent<BowlsManager>();
            carpetsManager = transform.GetChild(1).GetComponent<CarpetsManager>();
            musicsManager = transform.GetChild(2).GetComponent<MusicsManager>();

            allBowls = new List<Bowl>();
            for (int i = 0; i < bowlsManager.transform.childCount; i++)
            {
                allBowls.Add(bowlsManager.transform.GetChild(i).GetComponent<Bowl>());
            }

            allCarpets = new List<Carpet>();
            for (int i = 0; i < carpetsManager.transform.childCount; i++)
            {
                allCarpets.Add(carpetsManager.transform.GetChild(i).GetComponent<Carpet>());
            }

            // allBowls = new List<Bowl>();
            // for (int i = 0; i < bowlsManager.transform.childCount; i++)
            // {
            //     allBowls.Add(bowlsManager.transform.GetChild(i).GetComponent<Bowl>());
            // }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isGameplayScene = scene.buildIndex.Equals(0);

        for (int i = 0; i < allBowls.Count; i++)
        {
            if (allBowls[i].gameObject.activeInHierarchy)
            {
                allBowls[i].GetComponent<MeshRenderer>().enabled = isGameplayScene;
                allBowls[i].GetComponent<AudioLoudness>().enabled = isGameplayScene;
                allBowls[i].GetComponent<AudioSource>().enabled = isGameplayScene;
                allBowls[i].GetComponent<BoxCollider>().enabled = isGameplayScene;
            }
        }

        if (isGameplayScene)
        {
            bowlsManager.SetUpBowls();
        }
    }

    public void Manage3DItems(int currentState, int materialIndex)
    {
        AllRefs.I.shopMenu.selectedItem.carpetObj.SetActive(currentState.Equals(0));
        AllRefs.I.shopMenu.selectedItem.bowlObj.SetActive(currentState.Equals(1));

        switch (currentState)
        {
            case 0:
                AllRefs.I.shopMenu.selectedItem.carpetObj.GetComponent<Renderer>().material = allCarpets[materialIndex].material;
                break;
            case 1:
                AllRefs.I.shopMenu.selectedItem.bowlObj.GetComponent<Renderer>().material = allBowls[materialIndex].material;
                break;
        }
    }

    public Item GetItem(int itemType, int index)
    {
        switch (itemType)
        {
            case 0:
                return allCarpets[index].GetComponent<Item>();
            case 1:
                return allBowls[index].GetComponent<Item>();
            case 2:
                return allMusics[index].GetComponent<Item>();
            default:
                return null;
        }
    }

    public int GetItemCount(int itemType)
    {
        switch (itemType)
        {
            case 0:
                return allCarpets.Count;
            case 1:
                return allBowls.Count;
            case 2:
                return allMusics.Count;
            default:
                return 0;
        }
    }
}
