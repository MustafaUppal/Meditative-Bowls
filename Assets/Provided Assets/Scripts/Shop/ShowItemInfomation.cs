using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItemInfomation : MonoBehaviour
{
    public string Item;
    public string money;
    public Sprite ImageSprite;
    public string Category;

    public void Onpressed()
    {
        GameManager.Instance.SubInformationInventoryPanel.SetActive(true);
        GameManager.Instance.ItemnameSlot.text = Item;
        GameManager.Instance.ShowItemImageSlot.sprite = ImageSprite;
        GameManager.Instance.priceSlot.text = money;
        GameManager.Instance.CategorySlot.text = Category;
    }
}
