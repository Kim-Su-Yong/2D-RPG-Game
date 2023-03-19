using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMove
{
    [Tooltip("NPC move를 체크하면 NPC 가 움직임")]
    public bool NPCmove;
    public string[] direction; // NPC 가 움직일 방향 설정

    [Range(1, 5)]
    [Tooltip("1 = 천천히, 2 = 조금 천천히, 3 = 보통, 4 = 빠르게, 5 = 연속적으로")]
    public int frequency; // NPC 가 움직일 방향으로 얼마나 빠른 방향으로 움직일 것인가

}

public class NPCManager : MovingObject
{
    public NPCMove npc;
    private OrderManager theOrder;
    private DialogueManager theDM;

    public string[] texts;
    public GameObject theShop;
    private bool isShop; 
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theDM = FindObjectOfType<DialogueManager>();
        queue = new Queue<string>();
        StartCoroutine(MoveCoroutine());
    }
    void Update()
    {
        if(isShop)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                theShop.SetActive(false);
                isShop = false;
            }
        }
    }
    public void SetNotMove()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveCoroutine()
    {
        if (npc.direction.Length != 0)
        {
            for (int i = 0; i < npc.direction.Length; i++)
            {
                yield return new WaitUntil(() => queue.Count < 2);

                base.Move(npc.direction[i], npc.frequency);

                if (i == npc.direction.Length - 1)
                    i = -1;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(ACoroutine());
        }
    }
    IEnumerator ACoroutine()
    {
        theOrder.NotMove();
        theDM.ShowText(texts);

        yield return new WaitUntil(() => !theDM.talking);
        theShop.SetActive(true);
        isShop = true;
    }
}
