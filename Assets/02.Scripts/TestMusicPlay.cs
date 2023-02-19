using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMusicPlay : MonoBehaviour
{
    BGMManager bgm;
    public int musicTrack;

    void Start()
    {
        bgm = FindObjectOfType<BGMManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bgm.Play(musicTrack);
        this.gameObject.SetActive(false);
    }
}
