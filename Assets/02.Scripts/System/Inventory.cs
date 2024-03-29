using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    private DatabaseManager theDatabase;
    private AudioManager theAudio;
    private OrderManager theOrder;
    private OkOrCancel theOOC;
    private Equipment theEquip;

    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;
    public string item_sound;

    private InventorySlot[] slots; // 인벤토리 슬롯들

    public List<Item> inventoryItemList; // 플레이어가 소지한 아이템 리스트
    public List<Item> inventoryTabList; // 선택한 탭에 따라 다르게 보여질 아이템 리스트

    public Text Description_Text; // 부연 설명
    public string[] tabDescription; // 탭 부연 설명

    public Transform tf; // Slot 부모객체

    public GameObject go; // 인벤토리 활성화 비활성화
    public GameObject[] selectedTabImages;
    public GameObject go_OOC; // 선택지 활성화, 비활성화
    public GameObject prefab_floating_Text;

    private int selectedItem; // 선택된 아이템
    private int selectedTab; // 선택된 탭

    private int page; //페이지
    private int SlotCount; //활성화된 슬롯의 갯수
    private const int MAX_SLOTS_COUNT = 12; //최대 슬롯 갯수

    private bool activated; // 인벤토리 활성화시 true
    private bool tabActivated; // 탭 활성화시 true
    private bool itemActivated; // 아이템 활성화시 true
    private bool stopKeyInput; // 키입력 제한(소비할 때 질문이 나올 텐데, 그 때 키입력 방지)
    private bool preventExec; // 중복실행 제한

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    void Start()
    {
        instance = this;
        theAudio = FindObjectOfType<AudioManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        theOOC = FindObjectOfType<OkOrCancel>();
        theEquip = FindObjectOfType<Equipment>();

        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<InventorySlot>();
    }
    public List<Item> SaveItem()
    {
        return inventoryItemList;
    }
    public void LoadItem(List<Item> _itemList)
    {
        inventoryItemList = _itemList;
    }
    public void EquipToInventory(Item _item)
    {
        inventoryItemList.Add(_item);
    }

    public void GetAnItem(int _itemID, int _count = 1)
    {
        for (int i = 0; i < theDatabase.itemList.Count; i++) // 데이터베이스 아이템 검색
        {
            if(_itemID == theDatabase.itemList[i].itemID) // 데이터베이스 아이템 발견
            {
                var clone = Instantiate(prefab_floating_Text, PlayerManager.instance.transform.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = theDatabase.itemList[i].itemName + " " + _count + "개 획득 +";
                clone.transform.SetParent(this.transform);

                for (int j = 0; j < inventoryItemList.Count; j++) // 소지품에 같은 아이템이 있는지 확인
                {
                    if(inventoryItemList[j].itemID == _itemID) // 소지품에 같은 아이템이 있다면 개수만 증감
                    {
                        inventoryItemList[j].itemCount += _count;
                    }
                }
                inventoryItemList.Add(theDatabase.itemList[i]); // 없으면 새로 넣는다
                return;
            }
        }
        Debug.LogError("데이터베이스에 해당 ID값을 가진 아이템이 존재하지 않습니다."); // 데이터베이스에 ItemID 없음
    }

    public void ShowTab() // 탭 활성화
    {
        RemoveSlot();
        SelectedTab();
    }
    public void RemoveSlot() // 인벤토리 슬롯 초기화
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    }

    public void SelectedTab() // 선택된 탭 말고 나머지는 알파값 0 로 조정함
    {
        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;

        for (int i = 0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }

        Description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    }

    IEnumerator SelectedTabEffectCoroutine() // 선택된 탭 반짝임 효과
    {
        while (tabActivated)
        {
            Color color = selectedTabImages[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
    public void ShowPage()
    {
        SlotCount = -1;

        for (int i = page * MAX_SLOTS_COUNT; i < inventoryTabList.Count; i++) // 인벤토리 탭 리스트의 내용을 인벤토리 슬롯에 추가함
        {
            SlotCount = i - (page * MAX_SLOTS_COUNT);
            slots[SlotCount].gameObject.SetActive(true);
            slots[SlotCount].Additem(inventoryTabList[i]);

            if (SlotCount == MAX_SLOTS_COUNT - 1)
                break;
        }   //인벤토리 템 리스트의 내용을, 인벤토리 슬롯에 추가
    }
    public void ShowItem() // 아이템 활성화(조건에 맞는 아이템 넣고 인벤토리 슬롯에 출력)
    {
        inventoryTabList.Clear();
        RemoveSlot();
        selectedItem = 0;
        page = 0;

        switch(selectedTab) // 탭에 따른 아이템 분류를 인벤토리 탭 리스트에 추가함
        {
            case 0:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Equip == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
        }

        ShowPage();
        SelectedItem();
    }

    public void SelectedItem() // 선택된 아이템을 제외하고 다른 모든 아이템의 알파값 0 로 조정
    {
        StopAllCoroutines();
        if (SlotCount > -1)
        {
            Color color = slots[0].selected_item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i <= SlotCount; i++)
                slots[i].selected_item.GetComponent<Image>().color = color;

            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
        {
            Description_Text.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
        }
    }

    IEnumerator SelectedItemEffectCoroutine() // 선택된 아이템 반짝임 효과
    {
        while (itemActivated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selected_item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_item.GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    void Update()
    {
        if(!stopKeyInput)
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                activated = !activated;

                if (activated)
                {
                    theAudio.Play(open_sound);
                    theOrder.NotMove();
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = false;
                    ShowTab();
                }
                else
                {
                    theAudio.Play(cancel_sound);
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    theOrder.Move();
                }
            }

            if(activated)
            {
                if (tabActivated)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedTab < selectedTabImages.Length - 1)
                            selectedTab++;
                        else
                            selectedTab = 0;
                        theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedTab > 0)
                            selectedTab--;
                        else
                            selectedTab = selectedTabImages.Length - 1;
                        theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        theAudio.Play(enter_sound);
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                        itemActivated = true;
                        tabActivated = false;
                        preventExec = true;
                        ShowItem();
                    }
                } // 탭 활성화 시 키 입력

                else if (itemActivated) // 아이템 활성화 시 키 입력
                {
                    if (inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if(selectedItem + 2 > SlotCount)
                            {
                                if(page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT)
                                    page++;
                                else
                                    page = 0;

                                RemoveSlot();
                                ShowPage();
                                selectedItem = -2;
                            }
                            if (selectedItem < SlotCount - 1)
                            {
                                selectedItem += 2;
                            }
                            else
                                selectedItem %= 2;

                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if (selectedItem - 2 < 0)
                            {
                                if (page != 0)
                                    page--;
                                else
                                    page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;

                                RemoveSlot();
                                ShowPage();
                            }

                            if (selectedItem > 1)
                            {
                                selectedItem -= 2;
                            }
                            else
                                selectedItem = SlotCount - selectedItem;

                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (selectedItem - 2 < 0)
                            {
                                if (page != 0)
                                    page--;
                                else
                                    page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;

                                RemoveSlot();
                                ShowPage();
                            }
                            if (selectedItem > 0)
                                selectedItem--;
                            else
                                selectedItem = SlotCount;

                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (selectedItem + 1 > SlotCount)
                            {
                                if (page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT)
                                    page++;
                                else
                                    page = 0;

                                RemoveSlot();
                                ShowPage();
                                selectedItem = -1;
                            }

                            if (selectedItem < SlotCount)
                                selectedItem++;
                            else
                                selectedItem = 0;

                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {
                            if (selectedTab == 0) // 소모품
                            {
                                StartCoroutine(OkOrCancelCoroutine("사용", "판매"));
                            }
                            else if (selectedTab == 1)
                            {
                                StartCoroutine(OkOrCancelCoroutine("장착", "판매"));
                            }
                            else
                            {
                                theAudio.Play(beep_sound);
                            }
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        theAudio.Play(cancel_sound);
                        StopAllCoroutines();
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab();
                    }
                }
                if (Input.GetKeyUp(KeyCode.Z)) // 중복 실행 방지
                    preventExec = false;
            }
        }
    }

    IEnumerator OkOrCancelCoroutine(string _up, string _down)
    {
        theAudio.Play(enter_sound);
        stopKeyInput = true;

        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice(_up, _down);
        yield return new WaitUntil(() => !theOOC.activated);

        if(theOOC.GetResult())
        {
            RemoveItem();
        }
        else
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {
                    inventoryItemList.RemoveAt(i);
                    ShowItem();
                    break;
                }
            }
        }
        stopKeyInput = false;
        go_OOC.SetActive(false);
    }
    public void RemoveItem()
    {
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
            {
                if (selectedTab == 0) // 소모품을 사용했을 경우
                {
                    theDatabase.UseItem(inventoryItemList[i].itemID);

                    if (inventoryItemList[i].itemCount > 1)
                    {
                        inventoryItemList[i].itemCount--;
                    }
                    else
                        inventoryItemList.RemoveAt(i);

                    theAudio.Play(item_sound); //아이템 먹는 소리
                    ShowItem();
                    break;
                }
                else if (selectedTab == 1) // 장비 아이템을 사용했을 경우
                {
                    theEquip.EquipItem(inventoryItemList[i]);
                    inventoryItemList.RemoveAt(i);
                    ShowItem();
                    break;
                }
            }
        }
    }
}