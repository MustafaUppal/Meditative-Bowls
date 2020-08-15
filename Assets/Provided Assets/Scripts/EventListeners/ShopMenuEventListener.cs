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
        if (currentState.Equals(ShopStates.Bowls))
        {
            float x = selectedItem.bowlObj.transform.rotation.x;
            float y = selectedItem.bowlObj.transform.rotation.y;
            float z = selectedItem.bowlObj.transform.rotation.z;
            float w = selectedItem.bowlObj.transform.rotation.w;

            if (!Application.isEditor)
            {
                touch1 = Input.GetTouch(0);
                touch2 = Input.GetTouch(1);

                if (touch1.phase.Equals(TouchPhase.Began))
                {
                    t1Position = touch1.position;
                }

                if (touch2.phase.Equals(TouchPhase.Began))
                {
                    t2Position = touch2.position;
                }

                // Spin bowl
                if (Input.touchCount.Equals(1))
                {
                    if (touch1.phase.Equals(TouchPhase.Moved))
                    {
                        x += touch1.deltaPosition.x * rotationSpeed * Time.unscaledDeltaTime;
                        y += touch1.deltaPosition.x * rotationSpeed * Time.unscaledDeltaTime;
                        z += touch1.deltaPosition.x * rotationSpeed * Time.unscaledDeltaTime;

                        selectedItem.bowlObj.transform.rotation = new Quaternion(x, y, z, w);
                    }
                }
                else if (Input.touchCount > 1) // pan bowl
                {
                    if (touch1.phase.Equals(TouchPhase.Moved) || touch2.phase.Equals(TouchPhase.Moved))
                    {
                        float change = 0;
                        float cx1 = t1Position.x - touch1.position.x;
                        float cx2 = t2Position.x - touch2.position.x;
                        float cy1 = t1Position.y - touch1.position.y;
                        float cy2 = t2Position.y - touch2.position.y;

                        if (cx1 > 0 || cx2 > 0 || cy1 > 0 || cy2 > 0)
                        {
                            change += .1f;
                        }
                        else if (cx1 < 0 || cx2 < 0 || cy1 < 0 || cy2 < 0)
                            change -= .1f;


                        mainCamera.fieldOfView += change;
                        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 50, 70);
                    }

                }
            }
        }
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
        selectedItem.prevIndex = selectedItem.index;
        selectedItem.index = index;

        content.GetTile(selectedItem.prevIndex).Highlight = false;
        content.GetTile(selectedItem.index).Highlight = true;

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
                    Inventory.allCarpets[selectedItem.prevIndex].currentState = Item.State.Purchased;
                    Inventory.allCarpets[selectedItem.index].currentState = Item.State.Loaded;

                    InventoryManager.Instance.carpetsManager.activeCarpetIndex = selectedItem.index;
                    OnClickItemButton(selectedItem.index);
                }
                break;
        }
    }
}
