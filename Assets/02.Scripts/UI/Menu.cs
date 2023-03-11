using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject go;
    public AudioManager theAudio;

    public string call_sound;
    public string cancel_sound;

    public OrderManager theOrder;

    private bool activated;

    public void Exit()
    {
        Application.Quit();
    }
    public void Continue()
    {
        activated = false;
        go.SetActive(false);
        theAudio.Play(cancel_sound);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            activated = !activated;

            if(activated)
            {
                go.SetActive(true);
                theAudio.Play(call_sound);
            }
            else
            {
                go.SetActive(false);
                theAudio.Play(cancel_sound);
            }
        }
    }
}
