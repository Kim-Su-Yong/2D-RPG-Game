using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransferScene : MonoBehaviour
{
    public string transferMapName; //이동할 맵의 이름
    private MovingObject1 thePlayer;
    void Start()
    {
        thePlayer = FindObjectOfType<MovingObject1>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            thePlayer.currentMapName = transferMapName;
            SceneManager.LoadScene(transferMapName); 
            //씬전환 방식 
        }
    }
}