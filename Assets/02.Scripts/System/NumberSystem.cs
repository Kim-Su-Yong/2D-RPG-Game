using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSystem : MonoBehaviour
{
    private AudioManager theAudio;
    public string key_sound; // 방향키 사운드
    public string enter_sound; // 결정 사운드
    public string cancel_sound; // 오답, 취소 사운드
    public string correct_sound; // 정답 사운드

    private int count; // 배열의 크기(몇 자릿수인지 확인하기 위함) -> 만약 1,000 이면 3자릿수
    private int selectedTextBox; // 선택된 자릿수
    private int result; // 플레이어가 도출해낸 값
    private int correctNumber; // 정답

    private string tempNumber; 

    public GameObject superObject; // 화면 가운데 정렬을 위한 선언
    public GameObject[] panel;
    public Text[] number_Text;

    public Animator anim;

    public bool activated; // return new WaitUntil 
    private bool keyInput; // 키 처리 활성화, 비활성화
    private bool correctFlag; // 정답인지 아닌지 여부 확인

    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void ShowNumber(int _correctNumber)
    {
        correctNumber = _correctNumber;
        activated = true;
        correctFlag = false;

        string temp = correctNumber.ToString(); /* 문자열로 치환하는 함수(length 를 사용하기 위한 사용)
                                                 * 아까 전에 배열의 크기를 정했기 때문에 배열의 크기를 넘기기 위해 선언 */
        for (int i = 0; i < temp.Length; i++) 
        {
            count = i;
            panel[i].SetActive(true);
            number_Text[i].text = "0";
        }

        superObject.transform.position = new Vector3(superObject.transform.position.x + (15 * count),
                                                     superObject.transform.position.y, 
                                                     superObject.transform.position.z);
        selectedTextBox = 0;
        result = 0;
        SetColor();
        anim.SetBool("Appear", true);
        keyInput = true;
    }

    public void SetNumber(string _arrow)
    {
        int temp = int.Parse(number_Text[selectedTextBox].text); // 선택된 자릿수의 텍스트를 int 로 강제 형변환
        if (_arrow == "DOWN")
        {
            if (temp == 0)
                temp = 9;
            else temp--;
        }
        else if (_arrow == "UP")
        {
            if (temp == 9)
                temp = 0;
            else temp++;
        }
        number_Text[selectedTextBox].text = temp.ToString();
    }

    public void SetColor()
    {
        Color color = number_Text[0].color;
        color.a = 0.3f;
        for (int i = 0; i <= count; i++)
        {
            number_Text[i].color = color;
        }
        color.a = 1f;
        number_Text[selectedTextBox].color = color;
    }

    public bool GetResult()
    {
        return correctFlag;
    }

    void Update()
    {
        if(keyInput)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                theAudio.Play(key_sound);
                SetNumber("DOWN");
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                theAudio.Play(key_sound);
                SetNumber("UP");
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                theAudio.Play(key_sound);
                if (selectedTextBox < count)
                    selectedTextBox++;
                else
                    selectedTextBox = 0;
                SetColor();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                theAudio.Play(key_sound);
                if (selectedTextBox > 0)
                    selectedTextBox--;
                else 
                    selectedTextBox = count;
                SetColor();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                theAudio.Play(key_sound);
                keyInput = false;
                StartCoroutine(OXCoroutine());
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                theAudio.Play(key_sound);
                keyInput = false;
                StartCoroutine(ExitCoroutine());
            }
        }
    }

    IEnumerator OXCoroutine()
    {
        Color color = number_Text[0].color;
        color.a = 1f;

        for (int i = count; i >= 0; i--)
        {
            number_Text[i].color = color;
            tempNumber += number_Text[i].text;
        }

        yield return new WaitForSeconds(1f);

        result = int.Parse(tempNumber);

        if (result == correctNumber)
        {
            theAudio.Play(correct_sound);
            correctFlag = true;
        }
        else
        {
            theAudio.Play(cancel_sound);
            correctFlag = false;
        }

        StartCoroutine(ExitCoroutine());
    }
    IEnumerator ExitCoroutine()
    {
        Debug.Log("우리가 낸 답 = " + result + " 정답 = " + correctNumber);
        result = 0;
        tempNumber = "";
        anim.SetBool("Appear", false);

        yield return new WaitForSeconds(0.1f);

        for(int i =0; i<=count; i++)
            panel[i].SetActive(false);

        superObject.transform.position = new Vector3(superObject.transform.position.x - (15 * count),
                                             superObject.transform.position.y,
                                             superObject.transform.position.z);
        activated = false;

    }
}
