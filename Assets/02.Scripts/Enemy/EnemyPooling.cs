using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    public GameObject EnemyPrefab;
    List<GameObject> EnemyPool = new List<GameObject>(); //오브젝트 풀링
    public Transform[] SpawnPoints; //적 소환 위치
    public float timer = 0; //몹 리젠 시간
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
            if (EnemyPool[i].activeSelf == false) //적이 죽었을때
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
    #region 적 몬스터 오브젝트 풀링
    void CreateEnemyPool()   //오브젝트 풀링용 오브젝트 생성
    {
        for (int idx = 1; idx < 4; idx++)
        {
            GameObject _Enemy = Instantiate(EnemyPrefab, SpawnPoints[idx].transform);
            EnemyPool.Add(_Enemy);
        }
    }
    #endregion
}