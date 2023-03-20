using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Light : MonoBehaviour
{
    public GameObject go;
    public bool IsLight;

    private static Light instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
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
