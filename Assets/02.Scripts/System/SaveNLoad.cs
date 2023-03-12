using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveNLoad : MonoBehaviour
{
    [System.Serializable]
    public class Data //Data 클래스로 정의
    {
        public float playerX; //캐릭터의 X좌표값
        public float playerY; //캐릭터의 Y좌표값
        public float playerZ; //캐릭터의 Z좌표값

        public int playerLv; //캐릭터 레벨
        public int playerHP; //캐릭터 최대 Hp
        public int playerMP; //캐릭터 최대 Mp

        public int playerCurrentHP; //캐릭터의 현재 체력
        public int playerCurrentMP; //캐릭터의 현재 마나
        public int playerCurrentEXP; //캐릭터의 경험치

        public int playerHPR; //플레이어 초당 체력회복력
        public int playerMPR; //플레이어 초당 마나회복력

        public int playerATK; //캐릭터의 공격력
        public int playerDEF; //캐릭터의 방어력

        public int added_atk;
        public int added_def;
        public int added_hpr;
        public int added_mpr;

        public int playerMoney; //캐릭터의 소지금

        public List<int> playerItemInventory; //플레이어 인벤토리
        public List<int> playerItemInventoryCount; //플레이어 인벤토리 아이템 수
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

        data.playerX = thePlayer.transform.position.x; //현재 캐릭터의 X좌표값 저장
        data.playerY = thePlayer.transform.position.y; //현재 캐릭터의 Y좌표값 저장
        data.playerZ = thePlayer.transform.position.z; //현재 캐릭터의 Z좌표값 저장

        data.playerLv = thePlayerStat.character_Lv; //현재 캐릭터의 레벨 저장
        data.playerHP = thePlayerStat.hp; //현재 캐릭터의 캐릭터 최대 hp 저장
        data.playerHP = thePlayerStat.mp; //현재 캐릭터의 캐릭터 최대 hp 저장
        data.playerCurrentHP = thePlayerStat.currentHP; //현재 캐릭터의 현재 Hp 저장
        data.playerCurrentHP = thePlayerStat.currentMP; //현재 캐릭터의 현재 Mp 저장
        data.playerCurrentEXP = thePlayerStat.currentEXP; //현재 캐릭터의 경험치 저장
        data.playerATK = thePlayerStat.atk; //현재 캐릭터의 공격력 저장
        data.playerDEF = thePlayerStat.def; //현재 캐릭터의 방어력 저장
        //data.playerMoney = thePlayerStat.money; //현재 캐릭터의 소지금 저장

        data.mapName = thePlayer.currentMapName;
        data.sceneName = thePlayer.currentSceneName;

        Debug.Log("기초 데이터 성공");

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

        BinaryFormatter bf = new BinaryFormatter(); //직렬화 변수 생성
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat"); //SaveFile.dat이라는 파일을 생성
        bf.Serialize(file, data); //객체를 데이터로 변환
        file.Close(); //파일 닫기
    }
    public void CallLoad()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open); //SaveFile.dat이라는 파일 열기

        if (file != null && file.Length > 0)
        {
            data = (Data)bf.Deserialize(file); //데이터를 객체로 변환

            theDatabase = FindObjectOfType<DatabaseManager>();
            thePlayer = FindObjectOfType<PlayerManager>();
            thePlayerStat = FindObjectOfType<PlayerStat>();
            theInven = FindObjectOfType<Inventory>();

            thePlayer.currentMapName = data.mapName;
            thePlayer.currentSceneName = data.sceneName;

            vector.Set(data.playerX, data.playerY, data.playerZ); //데이터에 저장된 플레이어의 x,y,z 값으로 설정
            thePlayer.transform.position = vector;

            thePlayerStat.character_Lv = data.playerLv; //데이터에 있는 레벨값을 플레이어 레벨에 넣어줌
            thePlayerStat.hp = data.playerHP; //데이터에 있는 최대 HP를 플레이어 최대 HP에 넣어줌
            thePlayerStat.mp = data.playerMP; //데이터에 있는 최대 HP를 플레이어 최대 MP에 넣어줌
            thePlayerStat.currentHP = data.playerCurrentHP;
            thePlayerStat.currentMP = data.playerCurrentMP;
            thePlayerStat.currentEXP = data.playerCurrentEXP; //데이터에 있는 경험치를 플레이어 경험치에 넣어줌
            thePlayerStat.atk = data.playerATK; //데이터에 있는 공격력을 플레이어 공격력에 넣어줌
            thePlayerStat.def = data.playerDEF; //데이터에 있는 방어력을 플레이어 방어력에 넣어줌
            //thePlayerStat.money = data.playerMoney; //데이터에 있는 소지금을 플레이어 소지금에 넣어줌
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
                        Debug.Log("인벤토리 아이템을 로드했습니다 : " + theDatabase.itemList[x].itemID);
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
            Debug.Log("저장된 세이브 파일이 없습니다");
        }
        file.Close();
    }
}