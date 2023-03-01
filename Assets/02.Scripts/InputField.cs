using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputField : MonoBehaviour
{
    private PlayerManager thePlayer;

    public Text text;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            thePlayer.characterName = text.text;
            Destroy(this.gameObject);
        }
    }
}
