using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    public GameObject EnemyPrefab;
    List<GameObject> EnemyPool = new List<GameObject>(); //������Ʈ Ǯ��
    public Transform[] SpawnPoints; //�� ��ȯ ��ġ
    public float timer = 0; //�� ���� �ð�
    void Awake()
    {
        EnemyPrefab = Resources.Load("Slime") as GameObject;
        SpawnPoints = GetComponentsInChildren<Transform>();
    }
    void Start()
    {
        CreateEnemyPool();
    }
    void Update()
    {
        for (int i = 0; i < EnemyPool.Count; i++)
        {
            if (EnemyPool[i].activeSelf == false) //���� �׾�����
            {
                timer += Time.deltaTime;
                if(timer > 3)
                {
                    EnemyPool[i].SetActive(true);
                    timer = 0;
                }
            }
        }
    }
    #region �� ���� ������Ʈ Ǯ��
    void CreateEnemyPool()   //������Ʈ Ǯ���� ������Ʈ ����
    {
        for (int idx = 1; idx < 4; idx++)
        {
            GameObject _Enemy = Instantiate(EnemyPrefab, SpawnPoints[idx].transform);
            EnemyPool.Add(_Enemy);
        }
    }
    #endregion
}