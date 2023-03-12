using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    private BoxCollider2D bound;
    public string boundName;
    private CameraManager theCamera;
    void Start()
    {
        bound = GetComponent<BoxCollider2D>();
        theCamera = FindObjectOfType<CameraManager>();
    }
    public void SetBound()
    {
        if(theCamera != null)
        {
            theCamera.SetBound(bound);
        }
    }
}
