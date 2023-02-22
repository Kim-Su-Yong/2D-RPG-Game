using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransferMap : MonoBehaviour
{
    public string transferMapName; //이동할 맵의 이름
    private PlayerManager thePlayer;
    public Transform target;
    public BoxCollider2D TargetBound;
    private CameraManager theCamera;
    private FadeManager theFade;
    private OrderManager theOrder;
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theCamera = FindObjectOfType<CameraManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(TransferCoroutine());
        }
    }

    IEnumerator TransferCoroutine()
    {
        theOrder.NotMove();
        theFade.FadeOut();
        yield return new WaitForSeconds(1f);
        thePlayer.currentMapName = transferMapName;
        theCamera.SetBound(TargetBound);
        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
        //맵이동 방식(씬전환이랑 섞어쓰는건 추천하지 않음)
        thePlayer.transform.position = target.transform.position;
        theFade.FadeIn();
        yield return new WaitForSeconds(0.5f);
        theOrder.Move();
    }
}