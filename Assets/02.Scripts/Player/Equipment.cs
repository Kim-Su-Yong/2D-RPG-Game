using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    private OrderManager theOrder;
    private AudioManager theAudio;
    private PlayerStat thePlayerStat;
    private Inventory theInven;
    private OkOrCancel theOOC;

    public string key_sound;
    public string enter_sound;
    public string open_sound;
    public string close_sound;
    public string takeoff_sound;
    public string equip_sound;

    private const int WEAPON = 0, SHIELD = 1, AMULET = 2, LEFT_RING = 3, RIGHT_RING = 4,
                      HELMET = 5, ARMOR = 6, LEFT_GLOVE = 7, RIGHT_GLOVE = 8, BELT = 9,
                      LEFT_BOOTS = 10, RIGHT_BOOTS = 11; // 각종 장비들

    private const int ATK = 0, DEF = 1, HPR = 6, MPR = 7;

    private int added_atk, added_def, added_hpr, added_mpr;

    public GameObject equipWeapon;
    public GameObject go;
    public GameObject go_OOC;
    public Text[] texts; // 스탯
    public Image[] img_slots; // 장비 슬롯 아이콘
    public GameObject go_selected_Slot_UI; // 선택된 장비 슬롯 UI
    public Item[] equipItemList; // 장착된 장비 리스트

    private int selectedSlot; // 선택된 장비 슬롯

    public bool activated = false;
    private bool inputKey = true;

    void Start()
    {
        theInven = FindObjectOfType<Inventory>();
        theOrder = FindObjectOfType<OrderManager>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theOOC = FindObjectOfType<OkOrCancel>();
    }
    public void ShowTxT()
    {
        if (added_atk == 0)
            texts[ATK].text = thePlayerStat.ToString();
        else
        {
            texts[ATK].text = thePlayerStat.atk.ToString() + "(" + added_atk + ")";
        }
        texts[ATK].text = thePlayerStat.atk.ToString();

        if (added_def == 0)
            texts[DEF].text = thePlayerStat.ToString();
        else
        {
            texts[DEF].text = thePlayerStat.def.ToString() + "(" + added_def + ")";
        }
        texts[DEF].text = thePlayerStat.def.ToString();

        if (added_hpr == 0)
            texts[HPR].text = thePlayerStat.ToString();
        else
        {
            texts[HPR].text = thePlayerStat.recover_hp.ToString() + "(" + added_hpr + ")";
        }
        texts[HPR].text = thePlayerStat.recover_hp.ToString();

        if (added_mpr == 0)
            texts[MPR].text = thePlayerStat.ToString();
        else
        {
            texts[MPR].text = thePlayerStat.recover_mp.ToString() + "(" + added_mpr + ")";
        }
        texts[MPR].text = thePlayerStat.recover_mp.ToString();
    }

    public void EquipItem(Item _item)
    {
        string temp = _item.itemID.ToString();
        temp = temp.Substring(0, 3);
        switch(temp)
        {
            case "200": // 무기류
                EquipItemCheck(WEAPON, _item);
                equipWeapon.SetActive(true);
                equipWeapon.GetComponent<SpriteRenderer>().sprite = _item.itemIcon;
                break;
            case "201": // 방패류
                EquipItemCheck(SHIELD, _item);
                break;
            case "202": // 아뮬렛
                EquipItemCheck(AMULET, _item);
                break;
            case "203": // 반지
                EquipItemCheck(LEFT_RING, _item);
                break;
        }
    }

    public void EquipItemCheck(int _count, Item _item)
    {
        if (equipItemList[_count].itemID == 0) 
        {
            equipItemList[_count] = _item;
        }
        else
        {
            theInven.EquipToInventory(equipItemList[_count]);
            equipItemList[_count] = _item;
        }

        EquipEffect(_item);
        theAudio.Play(equip_sound);
        ShowTxT();
    }

    public void ClearEquip()
    {
        Color color = img_slots[0].color;
        color.a = 0f;

        for (int i = 0; i < img_slots.Length; i++)
        {
            img_slots[i].sprite = null;
            img_slots[i].color = color;
        }
    }

    public void ShowEquip()
    {
        Color color = img_slots[0].color;
        color.a = 1f;

        for (int i = 0; i < img_slots.Length; i++)
        {
            if (equipItemList[i].itemID != 0)
            {
                img_slots[i].sprite = equipItemList[i].itemIcon;
                img_slots[i].color = color;
            }
        }
    }

    public void SelectedSlot()
    {
        go_selected_Slot_UI.transform.position = img_slots[selectedSlot].transform.position;
    }

    IEnumerator OkOrCancelCoroutine(string _up, string _down)
    {
        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice(_up, _down);
        yield return new WaitUntil(() => !theOOC.activated);

        if (theOOC.GetResult())
        {
            theInven.EquipToInventory(equipItemList[selectedSlot]);
            TakeOffEffect(equipItemList[selectedSlot]);
            if(selectedSlot == WEAPON)
            {
                equipWeapon.SetActive(false);
            }
            ShowTxT();
            equipItemList[selectedSlot] = new Item(0, "", "", Item.ItemType.Equip);
            theAudio.Play(takeoff_sound);
            ClearEquip();
            ShowEquip();
        }
        inputKey = true;
        go_OOC.SetActive(false);
    }

    private void EquipEffect(Item _item)
    {
        thePlayerStat.atk += _item.atk;
        thePlayerStat.def += _item.def;
        thePlayerStat.recover_hp += _item.recover_hp;
        thePlayerStat.recover_mp += _item.recover_mp;

        added_atk += _item.atk;
        added_def += _item.def;
        added_hpr += _item.recover_hp;
        added_mpr += _item.recover_mp;
    }
    private void TakeOffEffect(Item _item)
    {
        thePlayerStat.atk -= _item.atk;
        thePlayerStat.def -= _item.def;
        thePlayerStat.recover_hp -= _item.recover_hp;
        thePlayerStat.recover_mp -= _item.recover_mp;

        added_atk -= _item.atk;
        added_def -= _item.def;
        added_hpr -= _item.recover_hp;
        added_mpr -= _item.recover_mp;
    }
    void Update()
    {
        if (inputKey) 
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                activated = !activated;

                if (activated) 
                {
                    theOrder.NotMove();
                    theAudio.Play(open_sound);
                    go.SetActive(true);
                    selectedSlot = 0;
                    SelectedSlot();
                    ClearEquip();
                    ShowEquip();
                    ShowTxT();
                }
                else
                {
                    theOrder.Move();
                    theAudio.Play(close_sound);
                    go.SetActive(false);
                    ClearEquip();
                }
            }

            if (activated)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectedSlot < img_slots.Length - 1)
                    {
                        selectedSlot++;
                    }
                    else
                        selectedSlot = 0;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (selectedSlot > 0)
                    {
                        selectedSlot--;
                    }
                    else
                        selectedSlot = img_slots.Length - 1;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (selectedSlot > 0)
                    {
                        selectedSlot--;
                    }
                    else
                        selectedSlot = img_slots.Length - 1;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (selectedSlot < img_slots.Length - 1)
                    {
                        selectedSlot++;
                    }
                    else
                        selectedSlot = 0;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (equipItemList[selectedSlot].itemID != 0)
                    {
                        theAudio.Play(enter_sound);
                        inputKey = false;
                        StartCoroutine(OkOrCancelCoroutine("벗기", "취소"));
                    }
                }
            }
        }
    }
}
