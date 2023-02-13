using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPoint;
    private MovingObject1 thePlayer;
    private CameraManager theCamera;
    void Start()
    {
        thePlayer = FindObjectOfType<MovingObject1>();
        theCamera = FindObjectOfType<CameraManager>();

        if(startPoint == thePlayer.currentMapName)
        {
            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, theCamera.transform.position.z);
            thePlayer.transform.position = this.transform.position;
        }
    }
}
