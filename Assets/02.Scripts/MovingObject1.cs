using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject1 : MonoBehaviour
{
    public string characterName;

    public float speed;
    public int walkCount;
    protected int currentWalkCount;

    private bool notCoroutine = false;

    protected Vector3 vector;

    public Queue<string> queue; // 선입선출 자료구조 
    public BoxCollider2D boxCollider;
    public LayerMask layerMask;
    public Animator animator;

    public void Move(string _dir, int _frequency = 5)
    {
        queue.Enqueue(_dir);
        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }
    }
    
    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        while(queue.Count != 0)
        {
            string direction = queue.Dequeue();
            vector.Set(0, 0, vector.z);

            switch (direction)
            {
                case "UP":
                    vector.y = 1f;
                    break;
                case "DOWN":
                    vector.y = -1f;
                    break;
                case "RIGHT":
                    vector.x = 1f;
                    break;
                case "LEFT":
                    vector.x = -1f;
                    break;
            }

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);
            animator.SetBool("Walking", true);

            while (currentWalkCount < walkCount)
            {
                transform.Translate(vector.x * speed, vector.y * speed, 0);
                currentWalkCount++;
                yield return new WaitForSeconds(0.01f);
            }

            currentWalkCount = 0;

            if (_frequency != 5)
            {
                animator.SetBool("Walking", false);
            }
        }
        animator.SetBool("Walking", false);
        notCoroutine = false;
    }

    protected bool CheckCollsion()
    {
        RaycastHit2D hit;

        Vector2 start = transform.position; // A 지점, 캐릭터의 현재 위치값
        Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount); 
        // B 지점, 캐릭터가 이동하고자 하는 위치값

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true;

        if (hit.transform != null)
            return true;

        return false;
    }
}