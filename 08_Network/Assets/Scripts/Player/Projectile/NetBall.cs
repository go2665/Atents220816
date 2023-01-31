using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetBall : NetworkBehaviour
{
    public float speed = 10.0f;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        transform.Rotate(-30.0f, 0, 0);             // 위로 쏘기 위해서 회전
        rigid.velocity = transform.forward * speed; // 앞으로 날아가게 만들기
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!NetworkObject.IsSpawned)    // 이미 디스폰 된다고 되어있는 상황이면
        {   
            return;                     // 이후의 코드는 실행 안함.
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            NetPlayerDecoration deco = collision.gameObject.GetComponent<NetPlayerDecoration>();
            GameManager.Inst.Logger.Log($"공 : {deco.PlayerName} 명중");

            NetPlayer player = collision.gameObject.GetComponent<NetPlayer>();
            player.OnDie();
        }

        NetworkObject.Despawn();        // 디스폰 처리
    }
}
