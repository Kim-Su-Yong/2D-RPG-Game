using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Bound[] bounds;
    private PlayerManager thePlayer;
    private CameraManager theCamera;
    private FadeManager theFade;
    private Menu theMenu;
    private DialogueManager theDialogue;
    private Camera theCam;

    public GameObject HpBar;
    public GameObject MpBar;
    public void LoadStart()
    {
        StartCoroutine(LoadWaitCoroutine());
    }
    IEnumerator LoadWaitCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        thePlayer = FindObjectOfType<PlayerManager>();
        bounds = FindObjectsOfType<Bound>();
        theCamera = FindObjectOfType<CameraManager>();
        theFade = FindObjectOfType<FadeManager>();
        theMenu = FindObjectOfType<Menu>();
        theDialogue = FindObjectOfType<DialogueManager>();
        theCam = FindObjectOfType<Camera>();

        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;

        theCamera.target = GameObject.Find("Player");
        theMenu.GetComponent<Canvas>().worldCamera = theCam;
        theDialogue.GetComponent<Canvas>().worldCamera = theCam;

        for (int i = 0; i < bounds.Length; i++)
        {
            if(bounds[i].boundName == thePlayer.currentMapName)
            {
                bounds[i].SetBound();
                break;
            }
        }
        HpBar.SetActive(true);
        MpBar.SetActive(false);

        theFade.FadeIn();
    }
}
