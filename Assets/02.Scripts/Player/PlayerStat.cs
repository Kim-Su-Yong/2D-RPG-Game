using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    public int character_Lv;    // �÷��̾� ����
    public int[] needExp;       // ������ ���� �ʿ� ����ġ
    public int currentEXP;      // ���� ����ġ

    public int hp;          // ���� ü��
    public int currentHP;           // �ִ� ü��
    public int mp;
    public int currentMP;

    public int atk;             // ���ݷ�
    public int def;             // ����
    public int money;           // ������

    public int recover_hp;
    public int recover_mp;

    public string dmgSound;

    public float time;
    private float current_time;

    public GameObject prefabs_Floating_Text;
    public GameObject parent;

    public Slider hpSlider;
    public Slider mpSlider;
    public Image EXPImage;

    public Text HpTxt;
    public Text MpTxt;
    public Text EXPTxt;
    public Text ChaLv;
    public Text MoneyTxt;

    public Quest theQuest;
    void Start()
    {
        instance = this;
        currentHP = hp;
        currentMP = 10;
        money = 500;
        current_time = time;
        theQuest = FindObjectOfType<Quest>();
    }
    public void Hit(int _enemyAtk)
    {
        int dmg;

        if (def >= _enemyAtk)
            dmg = 1;
        else
            dmg = _enemyAtk - def;

        currentHP -= dmg;

        if (currentHP <= 0)
            Debug.Log("ü�� 0 �̸�, ���ӿ���");

        AudioManager.instance.Play(dmgSound);

        Vector3 vector = transform.position;
        vector.y += 60;

        GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = dmg.ToString();
        clone.GetComponent<FloatingText>().text.color = Color.red;
        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);
        StopAllCoroutines();
        StartCoroutine(HitCoroutine());
    }
    IEnumerator HitCoroutine()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }

    void Update()
    {
        hpSlider.maxValue = hp;
        mpSlider.maxValue = mp;

        HpTxt.text = "(" + currentHP + "/" + hp + ")";
        MpTxt.text = "(" + currentMP + "/" + mp + ")";
        EXPTxt.text = "(" + currentEXP + "/" + needExp[character_Lv] + ")";
        ChaLv.text = character_Lv.ToString();
        MoneyTxt.text = money.ToString();

        hpSlider.value = currentHP;
        mpSlider.value = currentMP;
        EXPImage.fillAmount = (float)currentEXP / needExp[character_Lv];

        if (currentEXP >= needExp[character_Lv])    // ���� ����ġ�� �������� �ʿ����ġ �̻��̸�
        {
            Vector3 vector = transform.position;
            vector.y += 60;

            var clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingText>().text.text = "Level Up";
            clone.transform.SetParent(parent.transform);

            character_Lv++;
            hp += character_Lv * 2;
            hp += character_Lv + 2;

            currentHP = hp;
            currentMP = mp;
            atk++;
            def++;
            currentEXP = 0;
            theQuest.LevelCount();
        }

        current_time -= Time.deltaTime;

        if(current_time <= 0)
        {
            if(recover_hp > 0)
            {
                if (currentHP + recover_hp <= hp)
                    currentHP += recover_hp;
                else
                    currentHP = hp;
            }
            current_time = time;
        }
    }
}
