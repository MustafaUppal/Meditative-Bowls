using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ShopMenuEventListener : MonoBehaviour
{
    public int Index;
    private void OnEnable()
    {
        DockEventListener.ButtonsData data = new DockEventListener.ButtonsData
        {
            replayBG = false,
            changeCamera = false,
            saveSession = false
        };

        AllRefs.I.dock.ManageButtons(data);
    }
    private void Start()
    {


    }
    void LoadAllSet()
    {
        foreach (GameObject item in Inventory.Instance.BackGroundSoundDataButtons)
        {
            Instantiate(item, Inventory.Instance.ParentOfList.transform);
        }
        foreach (GameObject item in Inventory.Instance.CarpetsDataButtons)
        {
             Instantiate(item, Inventory.Instance.ParentOfList.transform);
        }
        foreach (GameObject item in Inventory.Instance.BowlDataButtons)
        {
            Instantiate(item, Inventory.Instance.ParentOfList.transform);
        }
        OnClickCarpetItemButton();
    }
    public void OnClickBackButton()
    {
        MenuManager.Instance.ChangeState(MenuManager.MenuStates.Main);
        GameManager.Instance.state = GameManager.State.Normal;
    }

    public void OnClickCarpetTilesButton(int index)
    {

        Inventory.Instance.ItemName.text = Inventory.Instance.Carpet[index].Name;
        Inventory.Instance.ItemImage.sprite = Inventory.Instance.Carpet[index].Bowl_Image;
        Inventory.Instance.ItemDescription.text = Inventory.Instance.Carpet[index].Description;

    }
    public void OnClickLoadBowl()
    {
        GameManager.Instance.state = GameManager.State.Load;
        GameManager.Instance.BowlToLoad = Inventory.Instance.BowlInventoryItems[Index].gameObject;
        GameManager.Instance.BowlToLoad.gameObject.SetActive(true);
        MenuManager.Instance.AllPanels[2].gameObject.SetActive(false);
    }
    public void OnClickBowlTilesButton(int index)
    {
        print("OnclickBowl");
        Index = index;
       Inventory.Instance.ItemName.text = Inventory.Instance.BowlInventoryItems[index].Name;
       Inventory.Instance.ItemImage.sprite = Inventory.Instance.BowlInventoryItems[index].Bowl_Image;
       Inventory.Instance.ItemDescription.text = Inventory.Instance.BowlInventoryItems[index].Description;
        
    }
    public void OnClickSoundTilesButton(int index)
    {
        print(Inventory.Instance);
        print("OnclickSound");
        Inventory.Instance.ItemName.text = Inventory.Instance.BackGroundSound[index].Name;
        Inventory.Instance.ItemImage.sprite = Inventory.Instance.BackGroundSound[index].Bowl_Image;
        Inventory.Instance.ItemDescription.text = Inventory.Instance.BackGroundSound[index].Description;
    }

    public void OnClickBowlsButton()
    {
        Inventory.Instance.CarpetScrollView.SetActive(false);
        Inventory.Instance.BackGroundMusicScrollView.SetActive(false);
        Inventory.Instance.BowlView.SetActive(true);
    }

    public void OnClickSoundsButton()
    {
        Inventory.Instance.CarpetScrollView.SetActive(false);
        Inventory.Instance.BackGroundMusicScrollView.SetActive(true);
        Inventory.Instance.BowlView.SetActive(false);
    }
    public void OnClickCarpetButton()
    {
        print("OnclickCarpetButtons");
        Inventory.Instance.CarpetScrollView.SetActive(false);
        Inventory.Instance.BackGroundMusicScrollView.SetActive(true);
        Inventory.Instance.BowlView.SetActive(false);
    
    }
    public void OnClickCarpetItemButton()
    {
        Inventory.Instance.BackGroundMusicScrollView.SetActive(false);
        Inventory.Instance.CarpetScrollView.SetActive(true);
        Inventory.Instance.BowlView.SetActive(false);
    }

}
