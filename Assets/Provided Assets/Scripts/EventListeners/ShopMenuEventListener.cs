using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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

    public ShopStates defaultState;
    public ShopStates currentState;
    public ShopStates prevState;

    [System.Serializable]
    public struct HeaderSettings
    {
        public Image[] buttons;

        public Color highlighted;
        public Color unhighlighted;
    }

    public HeaderSettings headerSettings;

    [System.Serializable]
    public struct SelectedItemSettings
    {
        public int index;
        public Image thumbnail;
        public RawImage carpet;
        public RawImage bowl;
        public Text description;
        public Button b_itemActionButton;
        public Text t_itemActionButton;

        public Color[] buttonColors;
        public Color tileHighlight;
        public Color tileNormal;
        public void ActivateImage(int index)
        {

            carpet.gameObject.SetActive(index.Equals(0));
            bowl.gameObject.SetActive(index.Equals(1));
            thumbnail.gameObject.SetActive(index.Equals(2));

        }
    }
    public int SelectedItemIndex;

    public SelectedItemSettings selectedItem;

    public ContentHandler content;

    public Inventory shop => Inventory.Instance;

    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData
        {
            replayBG = false,
            changeCamera = false,
            saveSession = false
        };

        AllRefs.I.dock.ManageButtons(data);

        ChangeState(defaultState);
    }

    void ChangeState(ShopStates newState)
    {
        GameManager.Instance.gameObject.GetComponent<BowlReposition>().StopEveryThing();

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
        GameManager.Instance.state = GameManager.State.Normal;
    }

    public void OnClickCarpetButton()
    {
        ChangeState(ShopStates.Carpets);
    }

    public void OnClickBowlsButton()
    {
        ChangeState(ShopStates.Bowls);
    }

    public void OnClickBGMusicsButton()
    {
        ChangeState(ShopStates.BG_Musics);
    }

    public void OnClickItemButton(int index)
    {
        SelectedItemIndex = index;
        content.GetTile(selectedItem.index).Highlight = false;
        content.GetTile(index).Highlight = true;

        selectedItem.index = index;
        Item item = shop.GetItem((int)currentState, index);

        string setName = item.set.Equals("") ? "" : " (" + item.set + ")";
        selectedItem.description.text = "<size=35>" + item.name + setName + "</size>\n" + item.description;
        selectedItem.b_itemActionButton.image.color = selectedItem.buttonColors[(int)item.currentState];
        selectedItem.b_itemActionButton.interactable = !item.currentState.Equals(Bowl.State.Loaded);
        selectedItem.t_itemActionButton.text = item.StateText;

        shop.Manage3DItems((int)currentState, index);
        DistinctFunctionality();
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

        switch (currentState)
        {
            case ShopStates.Bowls:
                if (Inventory.Instance.allBowls[SelectedItemIndex].GetComponent<Bowl>().currentState == Item.State.Purchased)
                {
                    MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
                    GameManager.Instance.BowlToLoad = Inventory.Instance.allBowls[SelectedItemIndex].gameObject;
                    GameManager.Instance.state = GameManager.State.Load;
                }
                break;
            case ShopStates.BG_Musics:
                if (Inventory.Instance.allBowls[SelectedItemIndex].GetComponent<BG_Music>().currentState == Item.State.Purchased)
                {
                    GameManager.Instance.BackgroundMusic.GetComponent<AudioSource>().clip
                    = Inventory.Instance.allMusics[SelectedItemIndex].SoundClip;
                }
                break;
            case ShopStates.Carpets:
                if (Inventory.Instance.allBowls[SelectedItemIndex].GetComponent<Carpet>().currentState == Item.State.Purchased)
                {
                    GameManager.Instance.carpetPlane.GetComponent<Renderer>().material = Inventory.Instance.allCarpets[SelectedItemIndex].material;
                }
                break;
        }
    }
}
