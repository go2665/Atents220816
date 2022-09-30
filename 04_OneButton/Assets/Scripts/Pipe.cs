using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float minHeight;
    public float maxHeight;
    
    Rigidbody2D rigid;

    public System.Action onScored;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void ResetRandomHeight()
    {
        //주의 : 위치를 옮기고 사용할 것
        Vector2 pos = Vector2.up * Random.Range(minHeight, maxHeight);
        rigid.MovePosition(rigid.position + pos);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            onScored?.Invoke();
        }
    }
}
