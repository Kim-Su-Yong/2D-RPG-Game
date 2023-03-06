using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MovingObject1
{
    public int atk; // 슬라임의 공격력
    public float attackDelay; // 공격 유예 시간

    public float inter_MoveWaitTime; // 움직임 대기 시간
    private float current_interMT; // 실제로 움직임을 대기시키는 시간

    public string atkSound;

    private Vector2 playerPos; // 플레이어의 좌표값

    private int random_int; // 랜덤으로 움직이기 위한 변수값
    private string direction; 

    void Start()
    {
        queue = new Queue<string>();
        current_interMT = inter_MoveWaitTime;
    }

    private void Flip()
    {
        Vector3 flip = transform.localScale;
        if (playerPos.x > this.transform.position.x)
            flip.x = -1f;
        else
            flip.x = 1f;

        this.transform.localScale = flip;
        animator.SetTrigger("Attack");
        StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);
        AudioManager.instance.Play(atkSound);
        if (NearPlayer())
        {
            Debug.Log("슬라임이 플레이어에게 " + atk + "만큼의 데미지를 입혔습니다.");
        }
    }

    private bool NearPlayer()
    {
        playerPos = PlayerManager.instance.transform.position;

        if (Mathf.Abs(Mathf.Abs(playerPos.x) - Mathf.Abs(this.transform.position.x)) <= speed * walkCount * 1.01f) 
        {
            if (Mathf.Abs(Mathf.Abs(playerPos.y) - Mathf.Abs(this.transform.position.y)) <= speed * walkCount * 0.5f)
            {
                return true;
            }
        }

        if (Mathf.Abs(Mathf.Abs(playerPos.y) - Mathf.Abs(this.transform.position.y)) <= speed * walkCount * 1.01f)
        {
            if (Mathf.Abs(Mathf.Abs(playerPos.x) - Mathf.Abs(this.transform.position.x)) <= speed * walkCount * 0.5f)
            {
                return true;
            }
        }

        return false;
    }

    public void RandomDirection()
    {
        vector.Set(0, 0, vector.z);
        random_int = Random.Range(0, 4);

        switch (random_int)
        {
            case 0:
                vector.y = 1f;
                direction = "UP";
                break;
            case 1:
                vector.y = -1f;
                direction = "DOWN";
                break;
            case 2:
                vector.x = 1f;
                direction = "RIGHT";
                break;
            case 3:
                vector.x = -1f;
                direction = "LEFT";
                break;
        }
    }

    void Update()
    {
        current_interMT -= Time.deltaTime;

        if (current_interMT <= 0) 
        {
            current_interMT = inter_MoveWaitTime;

            if (NearPlayer())
            {
                Flip();
                return;
            }

            RandomDirection();

            if (base.CheckCollsion())
            {
                queue.Clear();
                return;
            }

            base.Move(direction);
        }
    }
}
