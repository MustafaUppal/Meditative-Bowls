using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using SerializeableClasses;
using UnityEngine;
using UnityEngine.Assertions.Must;
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
        selectedItem.index = 0;
        ChangeState(defaultState);
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
        selectedItem.ActivateImage((int)currentState);

        //Setting tiles
        content.Init((int)currentState);
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
        Debug.Log("index: " + index);
        selectedItem.prevIndex = selectedItem.index;
        selectedItem.index = index;

        content.GetTile(selectedItem.prevIndex).Highlight = false;
        content.GetTile(selectedItem.index).Highlight = true;

        Item item = Inventory.GetItem((int)currentState, index);

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
            if (activeBowls[i].Equals(selectedItem.index))
            {
                Inventory.allBowls[activeBowls[i]].CurrentState = Item.State.Purchased;
                OnClickItemButton(activeBowls[i]);
                activeBowls[i] = -1;
                bowlPlacementSettings.SetText(i);
            }
        }

        activeBowls[index] = selectedItem.index;
        Inventory.allBowls[activeBowls[index]].CurrentState = Item.State.Loaded;
        OnClickItemButton(selectedItem.index);
        bowlPlacementSettings.SetText(index);

        bowlPlacementSettings.Enable = false;
    }

    public void OnClickCloseBowlsPlacementButton()
    {
        bowlPlacementSettings.Enable = false;
    }

    void DistinctFunctionality()
    {
        switch ((int)currentState)
        {
            case 0: // Carpets
                break;
            case 1: // Bowls
                break;
            case 2: // Musics
                break;
        }
    }

    #endregion Button Clicks end

    public void OnClickLoadItem()
    {
        // GameManager.Instance.FooterText.text = "Place The Bowl to yor desire location";

        switch (currentState)
        {
            case ShopStates.Bowls:
                if (Inventory.allBowls[selectedItem.index].CurrentState == Item.State.Purchased)
                {
                    MessageSender("Tip: Select a position to place or replace. Click on carpet to close Placement setting.");
                    bowlPlacementSettings.Enable = true;
                    bowlPlacementSettings.Init();
                }
                break;
            case ShopStates.BG_Musics:
                if (Inventory.allMusics[selectedItem.index].CurrentState == Item.State.Purchased)
                {
                    // GameManager.Instance.BackgroundMusic.GetComponent<AudioSource>().clip
                    // = Inventory.Instance.allMusics[selectedItem.index].SoundClip;
                }
                break;
            case ShopStates.Carpets:
                if (Inventory.allCarpets[selectedItem.index].CurrentState == Item.State.Purchased)
                {
                    Inventory.allCarpets[selectedItem.prevIndex].CurrentState = Item.State.Purchased;
                    Inventory.allCarpets[selectedItem.index].CurrentState = Item.State.Loaded;

                    InventoryManager.Instance.carpetsManager.activeCarpetIndex = selectedItem.index;
                    OnClickItemButton(selectedItem.index);
                }
                break;
        }
    }
}
