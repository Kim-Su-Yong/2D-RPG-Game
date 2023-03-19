using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    public List<GameObject> ShopItems = new List<GameObject>();
    public GameObject[] chkBuy;
    public bool isClick;
    int SlotNum;
    int ShopItemID;
    public Button[] btn;
    public bool isShop;
    void Update()
    {
        if (this.gameObject.activeSelf == false)
            isShop = false;
        
        else
            isShop = true;
    }
    public void Buy()
    {
        if(isClick)
        {
            ShopItemID = ShopItems[SlotNum].GetComponent<ItemPickUp>().itemID;
            Inventory.instance.GetAnItem(ShopItemID, 1);
            chkBuy[SlotNum].SetActive(false);
            isClick = false;
            btn[SlotNum].interactable = false;
        }
    }
    public void Click1()
    {
        SlotNum = 0;
        ClickItem();
    }
    public void Click2()
    {
        SlotNum = 1;
        ClickItem();
    }
    public void Click3()
    {
        SlotNum = 2;
        ClickItem();
    }
    void ClickItem()
    {
        if (!isClick)
        {
            chkBuy[SlotNum].SetActive(true);
            isClick = true;
        }
        else
        {
            chkBuy[SlotNum].SetActive(false);
            isClick = false;
        }
    }
}
