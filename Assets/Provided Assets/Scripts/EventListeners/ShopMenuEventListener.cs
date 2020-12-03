using SerializeableClasses;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuEventListener : MonoBehaviour
{
    public enum ShopStates
    {
        Carpets,
        Bowls,
        SlideShows
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
    public GameObject largeView;

    [Space]
    public ContentHandler content;
    public Text Footertext;
    public Touch touch1;
    public Touch touch2;
    public Vector2 t1Position;
    public Vector2 t2Position;
    public Camera mainCamera;

    [Space]
    public float rotationSpeed;
    public float panningSpeed;

    public InventoryManager Inventory => InventoryManager.Instance;
    public int[] activeBowls => InventoryManager.Instance.bowlsManager.activeBowlsIndexes;

    private void OnEnable()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer || Application.platform != RuntimePlatform.OSXPlayer)
            restoreButton.SetActive(false);
        else
            restoreButton.SetActive(!PlayerPreferencesManager.GetItemsRestored(false));

        selectedItem.index = 0;
        ChangeState(defaultState);
        //IAPManager.Instance.InitializeIAPManager(InitializeResultCallback);
        MessageSender("Himalayan Bowls Store");
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
        // content.SetDropdown((int)currentState);
        content.SetItems((int)currentState);
        OnClickItemButton(0); // select first tile in the start
    }

    #region Button Clicks
    public void OnClickBackButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
        if (GameManager.Instance)
            GameManager.Instance.State1 = GameManager.State.Normal;
    }

    public void OnClickCarpetButton()
    {
        ChangeState(ShopStates.Carpets);
        MessageSender("Purchase Tibetan Carpets");
    }

    public void OnClickBowlsButton()
    {
        MessageSender("Purchase Additional Singing Bowls");
        ChangeState(ShopStates.Bowls);
    }

    public void OnClickBGMusicsButton()
    {
        MessageSender("Purchase Sets of Slideshow Images");
        ChangeState(ShopStates.SlideShows);
    }

    public void OnClickItemButton(GameObject itemObj)
    {
        OnClickItemButton(itemObj.GetComponent<TileHandler>().index);
    }
    public void OnClickItemButton(int index)
    {
        // Debug.Log("index: " + index);
        selectedItem.prevIndex = selectedItem.index;
        selectedItem.index = index;
        // Debug.Log("index: " + selectedItem.index);

        try { content.GetTile(selectedItem.prevIndex).Highlight = HighlightProperties.Normal; } catch (System.Exception e) { };
        content.GetTile(selectedItem.index).Highlight = HighlightProperties.Selected;

        Item item = Inventory.GetItem((int)currentState, index);
        // string setName = item.setName.Equals("") ? "" : " (" + item.setName + ")";

        // Image
        selectedItem.largeImage.sprite = item.image;
        selectedItem.image.sprite = item.image;

        // Description
        selectedItem.description.text = "<size=35><color=#FF7900>" + item.name /* + setName */ + "</color></size>\n" + item.description;
        
        // Button
        selectedItem.SetButton((int)item.CurrentState);
        
        // Price
        selectedItem.SetPrice((int)item.CurrentState, item.price);

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

        content.SetItems((int)currentState);
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

    public void OnClickExpandImage(bool enable)
    {
        selectedItem.isLargeView = enable;
        largeView.SetActive(enable);
        ChangeView(false);
        if (!enable)
        {
            selectedItem.bowlObj.GetComponent<RotateObj>().rotate = true;
            selectedItem.bowlObj.GetComponent<RotatingPaningTest>().Reset();
        }
    }
    bool isSoundPlaying;
    public void OnClickPlaySound()
    {
        isSoundPlaying = !isSoundPlaying;

        int time = Inventory.allBowls[selectedItem.index].CurrentState == Item.State.Locked ? 3 : -1;
        AudioClip clip = Inventory.allBowls[selectedItem.index].AudioSource.clip;

        // selectedItem.playStopSound.SetIcon(isSoundPlaying);
        // audioHandler.Play(isSoundPlaying, clip, time);
    }

    void DistinctFunctionality()
    {
        switch ((int)currentState)
        {
            case 0: // Carpets
                ChangeView(false);

                selectedItem.resetButton.SetActive(false);
                selectedItem.EnableExpandButton(false);
                // selectedItem.playStopSound.root.SetActive(false);
                break;
            case 1: // Bowls
                ChangeView(false);

                selectedItem.EnableExpandButton(true);
                // selectedItem.playStopSound.root.SetActive(true);

                ChangeSoundPlay(false);
                break;
            case 2: // Musics
                ChangeView(true);

                selectedItem.resetButton.SetActive(false);
                selectedItem.EnableExpandButton(false);
                // selectedItem.playStopSound.root.SetActive(false);
                break;
        }
    }

    #endregion Button Clicks end

    public void OnItemPurchased(int type, int index)
    {
        PopupManager.Instance.spinnerLoading.Hide();
        // PopupManager.Instance.messagePopup.Show("Congratulations!", "Your purchase was successful.");
        
        // Debug.Log("Selected Index: " + selectedItem.index);
        // Debug.Log("Index: " + index);

        switch (type)
        {
            case 0: // carpet
                Inventory.allCarpets[index].CurrentState = Item.State.Purchased;
                OnClickItemButton(index);
                content.SetItems((int)currentState);
                break;
            case 1: // bowl
                Inventory.allBowls[index].CurrentState = Item.State.Purchased;
                OnClickItemButton(index);
                content.SetItems((int)currentState);
                break;
            case 2: // slideshow
                Inventory.allSlideShows[index].CurrentState = Item.State.Purchased;
                OnClickItemButton(index);
                Inventory.slideShowManager.activeMusicIndex = 0;
                content.SetItems((int)currentState);
                break;
        }

        PlayerPreferencesManager.SetPurchasedState((int)currentState, index, true);
    }

    public void OnClickLoadItem()
    {
        // GameManager.Instance.FooterText.text = "Place The Bowl to yor desire location";

        // Debug.Log("State: " + (int)currentState);
        // Debug.Log("Index: " + selectedItem.position);

        switch (currentState)
        {
            case ShopStates.Bowls:
                HandleLoadBowlClick();
                break;
            case ShopStates.SlideShows:
                HandleLoadSlideShowClick();
                break;
            case ShopStates.Carpets:
                HandleLoadCarpetClick();
                break;
        }
    }

    public void OnClickRestoreButton()
    {
        PopupManager.Instance.questionPopup.SetButton("Yes", "No");
        PopupManager.Instance.questionPopup.Show("Warning!", "Do You want to restore all purcahse?", RestoreCallBack);
    }

    public void RestoreCallBack(bool responce)
    {
        if(responce) MeditativeBowls.IAPManager.instance.RestorePurchases();
    }

    void HandleLoadBowlClick()
    {
        switch (Inventory.allBowls[selectedItem.index].CurrentState)
        {
            case Item.State.Locked:
                // purchase bowl
                MeditativeBowls.IAPManager.instance.PurchaseItem(selectedItem.index, (int)currentState);
                break;
            // case Item.State.Purchased:
            //     // load selected bowl
            //     MessageSender("Tip: Select a position to place or replace. Click on carpet to close Placement setting.");
            //     bowlPlacementSettings.Enable = true;
            //     bowlPlacementSettings.Init();
            //     break;
        }
    }

    void HandleLoadCarpetClick()
    {
        switch (Inventory.allCarpets[selectedItem.index].CurrentState)
        {
            case Item.State.Locked:
                // purchase bowl
                MeditativeBowls.IAPManager.instance.PurchaseItem(selectedItem.index, (int)currentState);
                break;
            case Item.State.Purchased:
                // load selected bowl

                for(int i = 0; i < Inventory.allCarpets.Count; i++)
                {
                    if(Inventory.allCarpets[i].CurrentState != Item.State.Locked)
                        Inventory.allCarpets[i].CurrentState = Item.State.Purchased;
                }

                // Inventory.allCarpets[selectedItem.prevIndex].CurrentState = Item.State.Purchased;
                Inventory.allCarpets[selectedItem.index].CurrentState = Item.State.Loaded;

                InventoryManager.Instance.carpetsManager.activeCarpetIndex = selectedItem.index;
                OnClickItemButton(selectedItem.index);

                content.SetItems((int)currentState/* , Inventory.allCarpets[selectedItem.index].set */);
                break;
        }
    }

    void HandleLoadSlideShowClick()
    {
        switch (Inventory.allSlideShows[selectedItem.index].CurrentState)
        {
            case Item.State.Locked:
                // purchase bowl
                MeditativeBowls.IAPManager.instance.PurchaseItem(selectedItem.index, (int)currentState);
                break;
        }
    }

    void ChangeView(bool imageView)
    {
        this.imageView = imageView;
        selectedItem.EnableImage((int)currentState, !imageView);

        if(selectedItem.isLargeView)
            selectedItem.largeImage.transform.parent.gameObject.SetActive(imageView);
        else
            selectedItem.image.transform.parent.gameObject.SetActive(imageView);
    }

    void ChangeSoundPlay(bool isSoundPlaying)
    {
        this.isSoundPlaying = isSoundPlaying;
        
        // selectedItem.playStopSound.SetIcon(isSoundPlaying);
        // audioHandler.Play(isSoundPlaying);
    }
}
