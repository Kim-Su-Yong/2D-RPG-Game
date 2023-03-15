using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public EnemyStat theEnemy;
    public GameObject[] items; //������ ������ ������
    private Transform tr;
    void Start()
    {
        items = Resources.LoadAll<GameObject>("Items");
        tr = GetComponent<Transform>();
    }
    void Update()
    {
        if (theEnemy.isDie)
            itemdrop();
    }
    void itemdrop()
    {
        Debug.Log("������ ����");
        GameObject Itemdrop = Instantiate(items[Random.Range(0, items.Length + 1)], new Vector3(tr.position.x + 40f, tr.position.y, 0), Quaternion.identity);
        //isItem = true;
    }
}
