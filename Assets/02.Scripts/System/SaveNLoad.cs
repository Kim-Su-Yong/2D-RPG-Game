using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveNLoad : MonoBehaviour
{
    [System.Serializable]
    public class Data //Data Ŭ������ ����
    {
        public float playerX; //ĳ������ X��ǥ��
        public float playerY; //ĳ������ Y��ǥ��
        public float playerZ; //ĳ������ Z��ǥ��

        public int playerLv; //ĳ���� ����
        public int playerHP; //ĳ���� �ִ� Hp
        public int playerMP; //ĳ���� �ִ� Mp

        public int playerCurrentHP; //ĳ������ ���� ü��
        public int playerCurrentMP; //ĳ������ ���� ����
        public int playerCurrentEXP; //ĳ������ ����ġ

        public int playerHPR; //�÷��̾� �ʴ� ü��ȸ����
        public int playerMPR; //�÷��̾� �ʴ� ����ȸ����

        public int playerATK; //ĳ������ ���ݷ�
        public int playerDEF; //ĳ������ ����

        public int added_atk;
        public int added_def;
        public int added_hpr;
        public int added_mpr;

        public int playerMoney; //ĳ������ ������

        public List<int> playerItemInventory; //�÷��̾� �κ��丮
        public List<int> playerItemInventoryCount; //�÷��̾� �κ��丮 ������ ��
        public List<int> playerEquipItem;

        public string mapName;
        public string sceneName;

        public List<bool> swList;
        public List<string> swNameList;
        public List<string> varNameList;
        public List<float> varNumberList;
    }
    private PlayerManager thePlayer;
    private PlayerStat thePlayerStat;
    private DatabaseManager theDatabase;
    private Inventory theInven;
    private Equipment theEquip;

    public Data data;

    private Vector3 vector;

    public void CallSave()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theInven = FindObjectOfType<Inventory>();
        theEquip = FindObjectOfType<Equipment>();

        data.playerX = thePlayer.transform.position.x; //���� ĳ������ X��ǥ�� ����
        data.playerY = thePlayer.transform.position.y; //���� ĳ������ Y��ǥ�� ����
        data.playerZ = thePlayer.transform.position.z; //���� ĳ������ Z��ǥ�� ����

        data.playerLv = thePlayerStat.character_Lv; //���� ĳ������ ���� ����
        data.playerHP = thePlayerStat.hp; //���� ĳ������ ĳ���� �ִ� hp ����
        data.playerHP = thePlayerStat.mp; //���� ĳ������ ĳ���� �ִ� hp ����
        data.playerCurrentHP = thePlayerStat.currentHP; //���� ĳ������ ���� Hp ����
        data.playerCurrentHP = thePlayerStat.currentMP; //���� ĳ������ ���� Mp ����
        data.playerCurrentEXP = thePlayerStat.currentEXP; //���� ĳ������ ����ġ ����
        data.playerATK = thePlayerStat.atk; //���� ĳ������ ���ݷ� ����
        data.playerDEF = thePlayerStat.def; //���� ĳ������ ���� ����
        //data.playerMoney = thePlayerStat.money; //���� ĳ������ ������ ����

        data.mapName = thePlayer.currentMapName;
        data.sceneName = thePlayer.currentSceneName;

        Debug.Log("���� ������ ����");

        data.playerItemInventory.Clear();
        data.playerItemInventoryCount.Clear();
        data.playerEquipItem.Clear();

        for (int i = 0; i < theDatabase.var_name.Length; i++)
        {
            data.varNameList.Add(theDatabase.var_name[i]);
            data.varNumberList.Add(theDatabase.var[i]);
        }
        for (int i = 0; i < theDatabase.switch_name.Length; i++)
        {
            data.swNameList.Add(theDatabase.switch_name[i]);
            data.swList.Add(theDatabase.switches[i]);
        }

        List<Item> itemList = theInven.SaveItem();

        for(int i = 0; i < itemList.Count; i++)
        {
            data.playerItemInventory.Add(itemList[i].itemID);
            data.playerItemInventoryCount.Add(itemList[i].itemCount);
        }
        for(int i = 0; i < theEquip.equipItemList.Length; i++)
        {
            data.playerEquipItem.Add(theEquip.equipItemList[i].itemID);
        }

        BinaryFormatter bf = new BinaryFormatter(); //����ȭ ���� ����
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat"); //SaveFile.dat�̶�� ������ ����
        bf.Serialize(file, data); //��ü�� �����ͷ� ��ȯ
        file.Close(); //���� �ݱ�
    }
    public void CallLoad()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open); //SaveFile.dat�̶�� ���� ����

        if (file != null && file.Length > 0)
        {
            data = (Data)bf.Deserialize(file); //�����͸� ��ü�� ��ȯ

            theDatabase = FindObjectOfType<DatabaseManager>();
            thePlayer = FindObjectOfType<PlayerManager>();
            thePlayerStat = FindObjectOfType<PlayerStat>();
            theInven = FindObjectOfType<Inventory>();

            thePlayer.currentMapName = data.mapName;
            thePlayer.currentSceneName = data.sceneName;

            vector.Set(data.playerX, data.playerY, data.playerZ); //�����Ϳ� ����� �÷��̾��� x,y,z ������ ����
            thePlayer.transform.position = vector;

            thePlayerStat.character_Lv = data.playerLv; //�����Ϳ� �ִ� �������� �÷��̾� ������ �־���
            thePlayerStat.hp = data.playerHP; //�����Ϳ� �ִ� �ִ� HP�� �÷��̾� �ִ� HP�� �־���
            thePlayerStat.mp = data.playerMP; //�����Ϳ� �ִ� �ִ� HP�� �÷��̾� �ִ� MP�� �־���
            thePlayerStat.currentHP = data.playerCurrentHP;
            thePlayerStat.currentMP = data.playerCurrentMP;
            thePlayerStat.currentEXP = data.playerCurrentEXP; //�����Ϳ� �ִ� ����ġ�� �÷��̾� ����ġ�� �־���
            thePlayerStat.atk = data.playerATK; //�����Ϳ� �ִ� ���ݷ��� �÷��̾� ���ݷ¿� �־���
            thePlayerStat.def = data.playerDEF; //�����Ϳ� �ִ� ������ �÷��̾� ���¿� �־���
            //thePlayerStat.money = data.playerMoney; //�����Ϳ� �ִ� �������� �÷��̾� �����ݿ� �־���
            //thePlayerStat.moneyText.text = thePlayerStat.money.ToString();
            theEquip.added_atk = data.added_atk;
            theEquip.added_def = data.added_def;
            theEquip.added_hpr = data.added_hpr;
            theEquip.added_mpr = data.added_mpr;

            theDatabase.var = data.varNumberList.ToArray();
            theDatabase.var_name = data.varNameList.ToArray();
            theDatabase.switches = data.swList.ToArray();
            theDatabase.switch_name = data.swNameList.ToArray();

            for (int i = 0; i < data.playerItemInventory.Count; i++)
            {
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    if (data.playerItemInventory[i] == theDatabase.itemList[x].itemID)
                    {
                        theEquip.equipItemList[i] = theDatabase.itemList[x];                   
                        break;
                    }
                }
            }
            List<Item> itemList = new List<Item>();

            for (int i = 0; i < data.playerItemInventory.Count; i++)
            {
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    if (data.playerItemInventory[i] == theDatabase.itemList[x].itemID)
                    {
                        itemList.Add(theDatabase.itemList[x]);
                        Debug.Log("�κ��丮 �������� �ε��߽��ϴ� : " + theDatabase.itemList[x].itemID);
                        break;
                    }
                }
            }
            for (int i = 0; i < data.playerItemInventoryCount.Count; i++)
            {
                itemList[i].itemCount = data.playerItemInventoryCount[i];
            }
            theInven.LoadItem(itemList);
            theEquip.ShowTxT();

            GameManager theGM = FindObjectOfType<GameManager>();
            theGM.LoadStart();

            SceneManager.LoadScene(data.sceneName);
        }
        else
        {
            Debug.Log("����� ���̺� ������ �����ϴ�");
        }
        file.Close();
    }
}