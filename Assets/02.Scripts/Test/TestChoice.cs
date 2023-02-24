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

    // public Dialogue dialogue1; // 분기 만들때 쓰면 좋다
    // public Dialogue dialogue2; // 두 번째 분기 등등...

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
