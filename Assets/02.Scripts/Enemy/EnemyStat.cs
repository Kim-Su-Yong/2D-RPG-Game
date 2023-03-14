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
    void Start()
    {
        currentHp = hp;
        healthBarFilled.fillAmount = 1f;
        theEnemy = FindObjectOfType<EnemyPooling>();
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
            Debug.Log("óġ");
            currentHp = hp;
            healthBarFilled.fillAmount = 1f;
            gameObject.SetActive(false);
            PlayerStat.instance.currentEXP += exp;
            healthBarBackground.SetActive(false);
        }

        else
        {
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
}
