using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject hitEffect;
    Player player;

    private void Start()
    {
        player = GameManager.Inst.Player;
        //player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("적을 공격했다.");
            IBattle target = other.GetComponent<IBattle>();
            if (target != null)
            {
                player.Attack(target);

                Vector3 impactPoint = transform.position + transform.up;    // 칼 위치 + 칼의 위쪽방향만큼 1만큼 이동
                Vector3 effectPoint = other.ClosestPoint(impactPoint);      // impactPoint에서 칼에 부딪친 컬라이더 위치에 가장 가까운 위치

                Instantiate(hitEffect, effectPoint, Quaternion.identity);   // 이펙트를 대충 부딪친 지점에 생성

                //Time.timeScale = 0.01f;
            }
        }
    }
}
