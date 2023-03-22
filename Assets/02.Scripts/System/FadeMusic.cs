using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMusic : MonoBehaviour
{
    public BGMManager bgm;

    void Start()
    {
        bgm = FindObjectOfType<BGMManager>();
    }

    IEnumerator Fade()
    {
        bgm.FadeOutMusic();
        yield return new WaitForSeconds(3.0f);

        bgm.FadeInMusic();
    }
}
