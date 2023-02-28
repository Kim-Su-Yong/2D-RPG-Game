using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChoice : MonoBehaviour
{
    private OrderManager theOrder;
    //private NumberSystem theNumber;
    private DialogueManager theDM;

    public bool flag;
    public string[] texts;

    // public Dialogue dialogue1; // �б� ���鶧 ���� ����
    // public Dialogue dialogue2; // �� ��° �б� ���...

    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        //theNumber = FindObjectOfType<NumberSystem>();
        theDM = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!flag)
        {
            StartCoroutine(ACoroutine());
        }
    }

    IEnumerator ACoroutine()
    {
        flag = true;
        theOrder.NotMove();
        theDM.ShowText(texts);
        //theNumber.ShowNumber(correctNumber);
        //yield return new WaitUntil(() => !theNumber.activated);

        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Move();
    }
}
