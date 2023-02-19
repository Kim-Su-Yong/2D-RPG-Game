using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFadeMusic : MonoBehaviour
{
    BGMManager bgm;

    void Start()
    {
        bgm = FindObjectOfType<BGMManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        bgm.FadeOutMusic();
        yield return new WaitForSeconds(3.0f);

        bgm.FadeInMusic();
    }
}
