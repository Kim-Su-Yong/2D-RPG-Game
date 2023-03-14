using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public List<GameObject> EnemyPool = new List<GameObject>(); //오브젝트 풀링
    int maxCount = 3; //최대 소환 개수
    public Transform[] SpawnPoints; //적 소환 위치
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
                    Debug.Log("리스폰");
                    EnemyPool[i].SetActive(true);
                    timer = 0;
                }
            }
        }
    }
    #region 적 몬스터 오브젝트 풀링
    void CreateEnemyPool()   //오브젝트 풀링용 오브젝트 생성
    {
        for (int i = 0; i < maxCount; i++)
        {
            int idx = Random.Range(1, 4);
            _Enemy = Instantiate(EnemyPrefab, SpawnPoints[idx].transform);
            _Enemy.name = "Enemy" + i.ToString("0");
            EnemyPool.Add(_Enemy);
        }
    }
    public GameObject GetEnemy() //오브젝트 풀링된 적 정보를 가져오는 함수
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