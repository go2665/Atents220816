using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1.0f;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.left, Space.World );
        //transform.Translate(speed * Time.deltaTime * new Vector3(-1,0), Space.Self);  // new로 새로 만들기 때문에 Vector3.left를 쓰는 것보다는 느리다.
    }
}
