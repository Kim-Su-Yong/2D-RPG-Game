using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Quest : MonoBehaviour
{
    public Button btn1;
    public Button btn2;
    public Text questText1;
    public Text questText2;
    public int count;
    public int lvlcount;
    private bool isQuest;
    private bool Questapt1;
    private bool Questapt2;
    public GameObject theQuest;
    public PlayerStat theStat;

    private static Quest instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }

        theStat = FindObjectOfType<PlayerStat>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && !isQuest)
        {
            theQuest.SetActive(true);
            isQuest = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isQuest)
        {
            theQuest.SetActive(false);
            isQuest = false;
        }
        QuestClear();
    }
    public void FirstQuest()
    {
        btn1.interactable = false;
        if (count >= 3)
            theStat.money += 300;

        Questapt1 = true;
    }
    public void DieCount()
    {
        if (Questapt1)
            count++;

        if (count == 3)
        {
            questText1.text = "완료하기";
            btn1.interactable = true;
        }
    }
    public void SecondQuest()
    {
        btn2.interactable = false;
        if (lvlcount == 0)
            theStat.money += 500;

        Questapt2 = true;
    }
    
    public void LevelCount()
    {
        lvlcount++;
    }
    void QuestClear()
    {
        if(lvlcount == 3 && Questapt2)
        {
            questText2.text = "완료하기";
            btn2.interactable = true;
            lvlcount = 0;
        }
    }
}
