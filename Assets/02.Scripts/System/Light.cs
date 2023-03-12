using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public GameObject go;
    public bool IsLight;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && !IsLight)
        {
            go.SetActive(true);
            IsLight = true;
        }
        else if (Input.GetKeyDown(KeyCode.V) && IsLight)
        {
            go.SetActive(false);
            IsLight = false;
        }
    }
}
