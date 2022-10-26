using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float moveSpeed = 3.0f;

    Transform target;
    Vector3 offset = Vector3.zero;
    bool isTargetAlive;

    Vector3 diePosition = Vector3.zero;
    Quaternion dieRotation = Quaternion.identity;
    public float dieSpeed = 0.3f;

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        target = player.transform;     // 대상 구하기(플레이어)
        isTargetAlive = player.IsAlive;
        player.onDie += OnTargetDie;
        offset = transform.position - target.position;  // 대상과의 간격 저장해놓기
    }

    private void LateUpdate()
    {
        if (isTargetAlive)
        {
            // 카메라의 위치 -> 목표치(대상의 위치 + 간격)로 보간하며 변경. 1/moveSpeed 초에 걸쳐 목표치까지 변경
            transform.position = Vector3.Lerp(transform.position, target.position + offset, moveSpeed * Time.deltaTime);
        }
        else
        {
            float delta = dieSpeed * Time.deltaTime;
            transform.position = Vector3.Slerp(transform.position, diePosition, delta);
            transform.rotation = Quaternion.Slerp(transform.rotation, dieRotation, delta);
        }
    }

    void OnTargetDie()
    {
        isTargetAlive = false;
        diePosition = target.position + target.up * 10.0f;
        dieRotation = Quaternion.LookRotation(-target.up, -target.forward);
    }

}
