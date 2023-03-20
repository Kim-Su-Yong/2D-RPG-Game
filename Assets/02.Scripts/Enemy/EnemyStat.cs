using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStat : MonoBehaviour
{
    public EnemyPooling theEnemy;
    public static EnemyStat instance; 

    public int hp;
    public int currentHp;
    public int atk;
    public int def;
    public int exp;

    public GameObject healthBarBackground;
    public Image healthBarFilled;

    public bool isDie = false;

    public GameObject[] items; //죽으면 떨어질 아이템
    private Transform tr;
    public bool isItem;
    public Quest theQuest;
    void Start()
    {
        currentHp = hp;
        healthBarFilled.fillAmount = 1f;
        theEnemy = FindObjectOfType<EnemyPooling>();
        items = Resources.LoadAll<GameObject>("Items");
        tr = GetComponent<Transform>();
        theQuest = FindObjectOfType<Quest>();
    }
    public int Hit(int _playerAtk)
    {
        int playerAtk = _playerAtk;
        int dmg;
        if (def >= playerAtk)
            dmg = 1;
        else
            dmg = playerAtk - def;

        currentHp -= dmg;

        if (currentHp <= 0)
        {
            if(!isItem)
                itemdrop();

            currentHp = hp;
            healthBarFilled.fillAmount = 1f;
            gameObject.SetActive(false);
            PlayerStat.instance.currentEXP += exp;
            healthBarBackground.SetActive(false);
            isDie = true;
            theQuest.DieCount();
        }

        else
        {
            isDie = false;
            isItem = false;
            healthBarFilled.fillAmount = (float)currentHp / hp;
            healthBarBackground.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(WaitCoroutine());
        }
        return dmg;     
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(3f);
        healthBarBackground.SetActive(false);
    }
    void itemdrop() //아이템 드랍 함수
    {
        GameObject Itemdrop = Instantiate(items[Random.Range(0, items.Length)], new Vector3(tr.position.x + 40f, tr.position.y, 0), Quaternion.identity);
        //적 처치시 랜덤 아이템 생성
        isItem = true;
    }
}
