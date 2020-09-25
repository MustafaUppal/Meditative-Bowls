using SerializeableClasses;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuEventListener : MonoBehaviour
{
    public enum ShopStates
    {
        Carpets,
        Bowls,
        BG_Musics
    }

    [Header("State Settings")]
    public ShopStates defaultState;
    public ShopStates currentState;
    public ShopStates prevState;

    [Space]
    public HeaderSettings headerSettings;
    public SelectedItemSettings selectedItem;
    public BowlPlacementSettings bowlPlacementSettings;
    public GameObject restoreButton;

    [Space]
    public ContentHandler content;
    public Text Footertext;
    public Touch touch1;
    public Touch touch2;
    public Vector2 t1Position;
    public Vector2 t2Position;
    public Camera mainCamera;
    public AudioHandler audioHandler;

    [Space]
    public float rotationSpeed;
    public float panningSpeed;

    public InventoryManager Inventory => InventoryManager.Instance;
    public int[] activeBowls => InventoryManager.Instance.bowlsManager.activeBowlsIndexes;

    private void OnEnable()
    {
        restoreButton.SetActive(!PlayerPreferencesManager.GetItemsRestored(false));

        selectedItem.index = 0;
        ChangeState(defaultState);
        //IAPManager.Instance.InitializeIAPManager(InitializeResultCallback);
        MessageSender("Carpet");
    }

    private void Update()
    {

    }

    void MessageSender(string Message)
    {
        Footertext.text = Message;
    }

    void ChangeState(ShopStates newState)
    {
        prevState = currentState;
        currentState = newState;

        // Highlighting and unhighlighting header buttons
        for (int i = 0; i < headerSettings.buttons.Length; i++)
        {
            headerSettings.buttons[i].color = i.Equals((int)currentState)
            ? headerSettings.highlighted : headerSettings.unhighlighted;
        }

        // Activating image
        selectedItem.EnableImage((int)currentState);

        //Setting tiles
        content.SetDropdown((int)currentState);
        OnClickItemButton(0); // select first tile in the start
    }

    #region Button Clicks
    public void OnClickBackButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
        if (GameManager.Instance)
            GameManager.Instance.state = GameManager.State.Normal;
    }

    public void OnClickCarpetButton()
    {
        ChangeState(ShopStates.Carpets);
        MessageSender("Carpet");
    }

    public void OnClickBowlsButton()
    {
        MessageSender("Bowl");
        ChangeState(ShopStates.Bowls);
    }

    public void OnClickBGMusicsButton()
    {
        MessageSender("BGMusic");
        ChangeState(ShopStates.BG_Musics);
    }

    public void OnClickItemButton(GameObject itemObj)
    {
        OnClickItemButton(itemObj.GetComponent<TileHandler>().Index);
    }
    public void OnClickItemButton(int index)
    {
        // Debug.Log("index: " + index);
        selectedItem.prevIndex = selectedItem.index;
        selectedItem.index = index;

        try{content.GetTile(selectedItem.prevIndex).Highlight = false;}catch(System.Exception e){};
        content.GetTile(selectedItem.index).Highlight = true;

        Item item = Inventory.GetItem((int)currentState, index);
        
        if((int)currentState != 2)
            selectedItem.position = int.Parse(item.name.Split(' ')[1]);
        else 
            selectedItem.position = index;

        string setName = item.setName.Equals("") ? "" : " (" + item.setName + ")";

        // Image
        selectedItem.image.sprite = item.image;
        // Description
        selectedItem.description.text = "<size=35>" + item.name + setName + "</size>\n" + item.description;
        // Button
        selectedItem.b_itemActionButton.transform.GetChild(0).GetComponent<Image>().color = selectedItem.buttonColors[(int)item.CurrentState];
        selectedItem.i_itemActionButton.sprite = selectedItem.buttonIcons[(int)item.CurrentState];
        selectedItem.b_itemActionButton.interactable = !item.CurrentState.Equals(Bowl.State.Loaded);
        // Price
        selectedItem.t_itemActionButton.text = item.price + "$";

        Inventory.Manage3DItems((int)currentState, index);
        DistinctFunctionality();
    }

    public void OnClickPlaceBowlButton(int index)
    {
        for (int i = 0; i < activeBowls.Length; i++)
        {
            if (activeBowls[i] != -1 && activeBowls[i].Equals(activeBowls[index]))
            {
                Inventory.allBowls[activeBowls[i]].CurrentState = Item.State.Purchased;
                break;
                // OnClickItemButton(activeBowls[i]);
                // activeBowls[i] = -1;
                // bowlPlacementSettings.SetText(i);
            }
        }

        activeBowls[index] = selectedItem.index;
        Inventory.allBowls[activeBowls[index]].CurrentState = Item.State.Loaded;
        OnClickItemButton(selectedItem.index);
        bowlPlacementSettings.SetText(index);

        bowlPlacementSettings.Enable = false;

        content.SetDropdown((int)currentState, Inventory.allBowls[activeBowls[index]].set);
    }

    public void OnClickCloseBowlsPlacementButton()
    {
        bowlPlacementSettings.Enable = false;
    }

    public void OnSetSelected(System.Int32 index)
    {
        OnClickItemButton(content.SetItems((int)currentState, index + 1));
        ChangeSoundPlay(false);
    }

    bool imageView;
    public void OnClickChangeViewButton()
    {
        imageView = !imageView;
        ChangeView(imageView);
    }

    bool isSoundPlaying;
    public void OnClickPlaySound()
    {
        isSoundPlaying = !isSoundPlaying;

        int time = Inventory.allBowls[selectedItem.index].CurrentState == Item.State.Locked ? 3 : -1;
        AudioClip clip = Inventory.allBowls[selectedItem.index].AudioSource.clip;

        selectedItem.SetPlaySprite(isSoundPlaying);
        audioHandler.Play(isSoundPlaying, clip, time);
    }

    void DistinctFunctionality()
    {
        switch ((int)currentState)
        {
            case 0: // Carpets
                ChangeView(false);

                selectedItem.EnableSwitchButton(true);
                selectedItem.playSoundButton.SetActive(false);
                break;
            case 1: // Bowls
                ChangeView(false);

                selectedItem.EnableSwitchButton(true);
                selectedItem.playSoundButton.SetActive(true);

                ChangeSoundPlay(false);
                break;
            case 2: // Musics
                ChangeView(true);

                selectedItem.EnableSwitchButton(false);
                selectedItem.playSoundButton.SetActive(false);
                break;
        }
    }

    #endregion Button Clicks end

    public void OnItemPurchased(int type)
    {
        PopupManager.Instance.messagePopup.Show("Purchase Successfull!", "Seleted item has been successfully purchased.");
        switch (type)
        {
            case 0: // carpet
                Inventory.allCarpets[selectedItem.index].CurrentState = Item.State.Purchased;
                OnClickItemButton(selectedItem.index);
                break;
            case 1: // bowl
                Inventory.allBowls[selectedItem.index].CurrentState = Item.State.Purchased;
                OnClickItemButton(selectedItem.index);
                break;
            case 2: // slideshow
                Inventory.allMusics[selectedItem.index].CurrentState = Item.State.Purchased;
                OnClickItemButton(selectedItem.index);
                Inventory.musicsManager.activeMusicIndex = 0;
                break;
        }

        PlayerPreferencesManager.SetPurchasedState((int)currentState, selectedItem.index, true);
    }

    public void OnClickLoadItem()
    {
        // GameManager.Instance.FooterText.text = "Place The Bowl to yor desire location";

        Debug.Log("State: " + (int)currentState);
        Debug.Log("Index: " + selectedItem.position);


        switch (currentState)
        {
            case ShopStates.Bowls:
                HandleLoadBowlClick();
                break;
            case ShopStates.BG_Musics:
                HandleLoadSlideShowClick();
                break;
            case ShopStates.Carpets:
                HandleLoadCarpetClick();
                break;
        }
    }

    public void OnClickRestoreButton()
    {
        MeditativeBowls.IAPManager.instance.RestorePurchases();
    }

    void HandleLoadBowlClick()
    {
        switch (Inventory.allBowls[selectedItem.index].CurrentState)
        {
            case Item.State.Locked:
                // purchase bowl
                MeditativeBowls.IAPManager.instance.PurchaseItem(selectedItem.position - 1, (int)currentState);
                break;
            case Item.State.Purchased:
                // load selected bowl
                MessageSender("Tip: Select a position to place or replace. Click on carpet to close Placement setting.");
                bowlPlacementSettings.Enable = true;
                bowlPlacementSettings.Init();
                break;
        }
    }

    void HandleLoadCarpetClick()
    {
        switch (Inventory.allCarpets[selectedItem.index].CurrentState)
        {
            case Item.State.Locked:
                // purchase bowl
                MeditativeBowls.IAPManager.instance.PurchaseItem(selectedItem.position - 1, (int)currentState);
                break;
            case Item.State.Purchased:
                // load selected bowl
                Inventory.allCarpets[selectedItem.prevIndex].CurrentState = Item.State.Purchased;
                Inventory.allCarpets[selectedItem.index].CurrentState = Item.State.Loaded;

                InventoryManager.Instance.carpetsManager.activeCarpetIndex = selectedItem.index;
                OnClickItemButton(selectedItem.index);

                content.SetDropdown((int)currentState, Inventory.allCarpets[selectedItem.index].set);
                break;
        }
    }

    void HandleLoadSlideShowClick()
    {
        switch (Inventory.allMusics[selectedItem.index].CurrentState)
        {
            case Item.State.Locked:
                // purchase bowl
                MeditativeBowls.IAPManager.instance.PurchaseItem(selectedItem.position, (int)currentState);
                break;
        }
    }

    void ChangeView(bool imageView)
    {
        this.imageView = imageView;
        selectedItem.EnableImage((int)currentState, !imageView);
        selectedItem.image.transform.parent.gameObject.SetActive(imageView);
    }

    void ChangeSoundPlay(bool isSoundPlaying)
    {
        this.isSoundPlaying = isSoundPlaying;
        
        selectedItem.SetPlaySprite(isSoundPlaying);
        audioHandler.Play(isSoundPlaying);
    }
}
