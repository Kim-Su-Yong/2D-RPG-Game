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

    private void OnTriggerEnter2D(Collider other)
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
