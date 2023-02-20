using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMove
{
    [Tooltip("NPC move�� üũ�ϸ� NPC �� ������")]
    public bool NPCmove;
    public string[] direction; // NPC �� ������ ���� ����

    [Range(1, 5)]
    [Tooltip("1 = õõ��, 2 = ���� õõ��, 3 = ����, 4 = ������, 5 = ����������")]
    public int frequency; // NPC �� ������ �������� �󸶳� ���� �������� ������ ���ΰ�

}

public class NPCManager : MovingObject1
{
    [SerializeField]
    public NPCMove npc;
    void Start()
    {
        queue = new Queue<string>();
        StartCoroutine(MoveCoroutine());
    }

    public void SetMove()
    {
        //StartCoroutine(MoveCoroutine());
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
}
