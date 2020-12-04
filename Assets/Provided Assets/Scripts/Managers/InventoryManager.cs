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
    public SlideShowManager slideShowManager;

    [Header("Items")]
    public List<Bowl> allBowls;
    public List<Carpet> allCarpets;
    public List<SlideShow> allSlideShows;

    [Header("States")]
    public int prevState = -1;
    public int currentState = -1;

    // Arrays to calculate position of item in set
    int[] CSI = { 0, 3, 6 };
    int[] BSI = { 0, 7, 14, 21 };

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    private void OnEnable()
    {
        // Debug.Log("OnEnable");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Debug.Log("Start");
        SessionManager.Instance.Init();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeState(scene.buildIndex);
        bool isGameplayScene = currentState.Equals(1);

        InitScene(isGameplayScene);
    }

    public void InitScene(bool isGameplayScene)
    {
        if (isGameplayScene)
        {
            SessionManager.Instance.Init();
            bowlsManager.SetUpBowls(prevState.Equals(2));
            // carpetsManager.SetUpCarpets();
        }
        else
        {
            int bowlsCount = GetItemCount((int)ShopMenuEventListener.ShopStates.Bowls);
            // int carpetsCount = GetItemCount((int)ShopMenuEventListener.ShopStates.Carpets);

            // int largerNumber = bowlsCount > carpetsCount ? bowlsCount : carpetsCount;

            for (int i = 0; i < bowlsCount; i++)
            {
                if (i < bowlsCount)
                    allBowls[i].gameObject.SetActive(isGameplayScene);

                // if (i < carpetsCount)
                //     allCarpets[i].gameObject.SetActive(isGameplayScene);
            }
        }
    }

    void ChangeState(int state)
    {
        prevState = currentState;
        currentState = state;
    }

    public void Manage3DItems(int currentState, int materialIndex)
    {
        AllRefs.I.shopMenu.selectedItem.carpetObj.SetActive(currentState.Equals(0));
        AllRefs.I.shopMenu.selectedItem.carpetCam.SetActive(currentState.Equals(0));

        AllRefs.I.shopMenu.selectedItem.bowlObj.SetActive(currentState.Equals(1));
        AllRefs.I.shopMenu.selectedItem.bowlCam.SetActive(currentState.Equals(1));

        switch (currentState)
        {
            case 0:
                AllRefs.I.shopMenu.selectedItem.carpetObj.GetComponent<Renderer>().material = allCarpets[materialIndex].material;
                break;
            case 1:
                AllRefs.I.shopMenu.selectedItem.bowlObj.GetComponent<Renderer>().material = allBowls[materialIndex].material;
                AllRefs.I.shopMenu.selectedItem.bowlObj.GetComponent<MeshFilter>().sharedMesh = allBowls[materialIndex].Mesh.sharedMesh;
                AllRefs.I.shopMenu.selectedItem.bowlObj.transform.localScale = allBowls[materialIndex].transform.localScale;
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
                return allSlideShows[index].GetComponent<Item>();
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
                return allSlideShows.Count;
            default:
                return 0;
        }
    }

    public int GetItemPositionInSet(int type, int set, int index)
    {
        int position = 0;

        switch (type)
        {
            case 0:
                position = index - CSI[set - 1] + 1;
                break;
            case 1:
                position = index - BSI[set - 1] + 1;
                break;
            case 2:
                position = index + 1;
                break;
        }

        return position;
    }

    public bool IsAnySlideShowAvailible()
    {
        int itemType = (int)ShopMenuEventListener.ShopStates.SlideShows;

        for (int i = 0; i < GetItemCount(itemType); i++)
        {
            if (allSlideShows[i].currentState == Item.State.Purchased)
                return true;
        }

        return false;
    }

    public string GetItemProductId(int type, int index)
    {
        Item i = GetItem(type, index);

        int position = GetItemPositionInSet(type, i.set, index);
        string itemName = GetItemName(type);

        string productId = "com.HimalayanBowls.SingingBowls";

        if (type == 2)
            productId += "." + itemName + (position);
        else
            productId += ".Set" + i.set + "." + itemName + (position) + GetFloatingPoint(type, i.set, position);

        // Debug.Log(productId);
        return productId;
    }

    private string GetItemName(int type)
    {
        switch (type)
        {
            case 0:
                return "Carpet";
            case 1:
                return "Bowl";
            case 2:
                return "SlideShow";
            default:
                return "";
        }
    }

    private string GetFloatingPoint(int type, int set, int positionInSet)
    {
        if (type == 0 && set == 2 && positionInSet == 3)
            return ".1";
        else if (type == 1 && set == 2 && positionInSet == 2)
            return ".1";
        else if (type == 2)
            return "";
        else
            return ".0";
    }
}
