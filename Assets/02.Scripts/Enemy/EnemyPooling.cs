using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public List<GameObject> EnemyPool = new List<GameObject>(); //������Ʈ Ǯ��
    int maxCount = 3; //�ִ� ��ȯ ����
    public Transform[] SpawnPoints; //�� ��ȯ ��ġ
    public GameObject _Enemy;
    public float timer = 0;

    public EnemyStat theStat;
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
            if (EnemyPool[i].activeSelf == false)
            {
                timer += Time.deltaTime;
                if(timer > 3)
                {
                    Debug.Log("������");
                    EnemyPool[i].SetActive(true);
                    timer = 0;
                }
            }
        }
    }
    #region �� ���� ������Ʈ Ǯ��
    void CreateEnemyPool()   //������Ʈ Ǯ���� ������Ʈ ����
    {
        for (int i = 0; i < maxCount; i++)
        {
            int idx = Random.Range(1, 4);
            _Enemy = Instantiate(EnemyPrefab, SpawnPoints[idx].transform);
            _Enemy.name = "Enemy" + i.ToString("0");
            EnemyPool.Add(_Enemy);
        }
    }
    public GameObject GetEnemy() //������Ʈ Ǯ���� �� ������ �������� �Լ�
    {
        for (int i = 0; i < EnemyPool.Count; i++)
        {
            if (EnemyPool[i].activeSelf == false)
            {
                return EnemyPool[i];
            }
        }
        return null;
    }
    #endregion
}