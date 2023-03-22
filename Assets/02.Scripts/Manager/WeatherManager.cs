using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private AudioManager theAudio;
    public ParticleSystem rain;
    public string rain_sound;

    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            RainStop();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Rain();
        }
    }
    public void Rain()
    {
        theAudio.Play(rain_sound);
        rain.Play();
    }

    public void RainStop()
    {
        theAudio.Stop(rain_sound);
        rain.Stop();
    }

    public void RainDrop()
    {
        rain.Emit(10);
    }
}
