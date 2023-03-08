using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory1 : MonoBehaviour
{
    public static Inventory1 instance;
    private DatabaseManager theDatabase;
    private AudioManager theAudio;
    private OrderManager theOrder;
    private OkOrCancel theOOC;

    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;

    private InventorySlot[] slots; // �κ��丮 ���Ե�

    private List<Item> inventoryItemList; // �÷��̾ ������ ������ ����Ʈ
    private List<Item> inventoryTabList; // ������ �ǿ� ���� �ٸ��� ������ ������ ����Ʈ

    public Text Description_Text; // �ο� ����
    public string[] tabDescription; // �� �ο� ����

    public Transform tf; // Slot �θ�ü

    public GameObject go; // �κ��丮 Ȱ��ȭ ��Ȱ��ȭ
    public GameObject[] selectedTabImages;
    public GameObject go_OOC; // ������ Ȱ��ȭ, ��Ȱ��ȭ
    public GameObject prefab_floating_Text;

    private int selectedItem; // ���õ� ������
    private int selectedTab; // ���õ� ��

    private bool activated; // �κ��丮 Ȱ��ȭ�� true
    private bool tabActivated; // �� Ȱ��ȭ�� true
    private bool itemActivated; // ������ Ȱ��ȭ�� true
    private bool stopKeyInput; // Ű�Է� ����(�Һ��� �� ������ ���� �ٵ�, �� �� Ű�Է� ����)
    private bool preventExec; // �ߺ����� ����

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    void Start()
    {
        instance = this;
        theAudio = FindObjectOfType<AudioManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        theOOC = FindObjectOfType<OkOrCancel>();

        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<InventorySlot>();
    }

    public void GetAnItem(int _itemID, int _count = 1)
    {
        for (int i = 0; i < theDatabase.itemList.Count; i++) // �����ͺ��̽� ������ �˻�
        {
            if(_itemID == theDatabase.itemList[i].itemID) // �����ͺ��̽� ������ �߰�
            {
                var clone = Instantiate(prefab_floating_Text, PlayerManager.instance.transform.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = theDatabase.itemList[i].itemName + " " + _count + "�� ȹ�� +";
                clone.transform.SetParent(this.transform);

                for (int j = 0; j < inventoryItemList.Count; j++) // ����ǰ�� ���� �������� �ִ��� Ȯ��
                {
                    if(inventoryItemList[j].itemID == _itemID) // ����ǰ�� ���� �������� �ִٸ� ������ ����
                    {
                        inventoryItemList[j].itemCount += _count;
                    }
                }
                inventoryItemList.Add(theDatabase.itemList[i]); // ������ ���� �ִ´�
                return;
            }
        }
        Debug.LogError("�����ͺ��̽��� �ش� ID���� ���� �������� �������� �ʽ��ϴ�."); // �����ͺ��̽��� ItemID ����
    }

    public void ShowTab() // �� Ȱ��ȭ
    {
        RemoveSlot();
        SelectedTab();
    }
    public void RemoveSlot() // �κ��丮 ���� �ʱ�ȭ
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    }

    public void SelectedTab() // ���õ� �� ���� �������� ���İ� 0 �� ������
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

    IEnumerator SelectedTabEffectCoroutine() // ���õ� �� ��¦�� ȿ��
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

    public void ShowItem() // ������ Ȱ��ȭ(���ǿ� �´� ������ �ְ� �κ��丮 ���Կ� ���)
    {
        inventoryTabList.Clear();
        RemoveSlot();
        selectedItem = 0;

        switch(selectedTab) // �ǿ� ���� ������ �з��� �κ��丮 �� ����Ʈ�� �߰���
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

        for (int i =0; i<inventoryTabList.Count; i++) // �κ��丮 �� ����Ʈ�� ������ �κ��丮 ���Կ� �߰���
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Additem(inventoryTabList[i]);
        }

        SelectedItem();
    }

    public void SelectedItem() // ���õ� �������� �����ϰ� �ٸ� ��� �������� ���İ� 0 �� ����
    {
        StopAllCoroutines();
        if (inventoryTabList.Count > 0)
        {
            Color color = slots[0].selected_item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i < inventoryTabList.Count; i++)
                slots[i].selected_item.GetComponent<Image>().color = color;

            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
        {
            Description_Text.text = "�ش� Ÿ���� �������� �����ϰ� ���� �ʽ��ϴ�.";
        }
    }

    IEnumerator SelectedItemEffectCoroutine() // ���õ� ������ ��¦�� ȿ��
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
                } // �� Ȱ��ȭ �� Ű �Է�

                else if (itemActivated) // ������ Ȱ��ȭ �� Ű �Է�
                {
                    if (inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 2)
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
                            if (selectedItem > 1)
                            {
                                selectedItem -= 2;
                            }
                            else
                                selectedItem = inventoryTabList.Count - 1 - selectedItem;

                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (selectedItem > 0)
                                selectedItem--;
                            else
                                selectedItem = inventoryTabList.Count - 1;

                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 1)
                                selectedItem++;
                            else
                                selectedItem = 0;

                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {
                            if (selectedTab == 0) // �Ҹ�ǰ
                            {
                                theAudio.Play(enter_sound);
                                stopKeyInput = true;
                                StartCoroutine(OkOrCancelCoroutine());
                            }
                            else if (selectedTab == 1)
                            {
                                // ��� ����
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
                if (Input.GetKeyUp(KeyCode.Z)) // �ߺ� ���� ����
                    preventExec = false;
            }
        }
    }

    IEnumerator OkOrCancelCoroutine()
    {
        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice("���", "���");
        yield return new WaitUntil(() => !theOOC.activated);

        if(theOOC.GetResult())
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {
                    if (inventoryItemList[i].itemCount > 1)
                    {
                        inventoryItemList[i].itemCount--;
                    }
                    else
                        inventoryItemList.RemoveAt(i);

                    ShowItem();
                    break;
                }
            }
        }
        stopKeyInput = false;
        go_OOC.SetActive(false);
    }
}