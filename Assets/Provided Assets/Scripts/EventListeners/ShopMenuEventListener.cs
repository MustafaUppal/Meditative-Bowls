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

    public InventoryManager Inventory => InventoryManager.Instance;
    public int[] activeBowls => InventoryManager.Instance.bowlsManager.activeBowlsIndexes;

    private void OnEnable()
    {
        selectedItem.index = 0;
        ChangeState(defaultState);
        MessageSender("Carpet");
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

    public void OnClickItemButton(int index)
    {
        content.GetTile(selectedItem.index).Highlight = false;
        content.GetTile(index).Highlight = true;

        selectedItem.index = index;
        Item item = Inventory.GetItem((int)currentState, index);

        string setName = item.set.Equals("") ? "" : " (" + item.set + ")";
        selectedItem.description.text = "<size=35>" + item.name + setName + "</size>\n" + item.description;
        selectedItem.b_itemActionButton.image.color = selectedItem.buttonColors[(int)item.currentState];
        selectedItem.b_itemActionButton.interactable = !item.currentState.Equals(Bowl.State.Loaded);
        selectedItem.t_itemActionButton.text = item.StateText;

        Inventory.Manage3DItems((int)currentState, index);
        DistinctFunctionality();
    }

    public void OnClickPlaceBowlButton(int index)
    {
        for (int i = 0; i < activeBowls.Length; i++)
        {
            if (activeBowls[i].Equals(selectedItem.index))
            {
                activeBowls[i] = -1;
                bowlPlacementSettings.SetText(i);
            }
        }

        activeBowls[index] = selectedItem.index;
        bowlPlacementSettings.SetText(index);
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
                if (Inventory.allBowls[selectedItem.index].currentState == Item.State.Purchased)
                {
                    MessageSender("Tip: Select a position to place or replace. Click on carpet to close Placement setting.");
                    bowlPlacementSettings.Enable = true;
                    bowlPlacementSettings.Init();
                }
                break;
            case ShopStates.BG_Musics:
                if (Inventory.allMusics[selectedItem.index].currentState == Item.State.Purchased)
                {
                    // GameManager.Instance.BackgroundMusic.GetComponent<AudioSource>().clip
                    // = Inventory.Instance.allMusics[selectedItem.index].SoundClip;
                }
                break;
            case ShopStates.Carpets:
                if (Inventory.allCarpets[selectedItem.index].currentState == Item.State.Purchased)
                {
                    // GameManager.Instance.carpetPlane.GetComponent<Renderer>().material = Inventory.Instance.AllCarpets[selectedItem.index].material;
                }
                break;
        }
    }
}
