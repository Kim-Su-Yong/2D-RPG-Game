using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    private OrderManager theOrder;
    private DialogueManager theDM;
    public Dialogue dialogue;

    public GameObject theShop;
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theDM = FindObjectOfType<DialogueManager>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            theShop.SetActive(false);
            theOrder.Move();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(ACoroutine());
        }
    }
    IEnumerator ACoroutine()
    {
        theOrder.NotMove();
        theDM.ShowDialogue(dialogue);

        yield return new WaitUntil(() => !theDM.talking);
        theShop.SetActive(true);
    }
}
