using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChoice : MonoBehaviour
{
    [SerializeField]
    public Choice choice;
    private OrderManager theOrder;
    private ChoiceManager theChoice;
    public bool flag;

    // public Dialogue dialogue1; // �б� ���鶧 ���� ����
    // public Dialogue dialogue2; // �� ��° �б� ���...

    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
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
        theChoice.ShowChoice(choice);
        yield return new WaitUntil(() => !theChoice.choiceIng);
        theOrder.Move();
        Debug.Log(theChoice.GetResult());
    }
}
