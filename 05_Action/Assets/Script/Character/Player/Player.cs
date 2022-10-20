using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 무기에 붙어있는 파티클 시스템 컴포넌트
    /// </summary>
    ParticleSystem weaponPS;

    /// <summary>
    /// 무기가 붙어있을 게임오브젝트의 트랜스폼
    /// </summary>
    Transform weapon_r;

    private void Awake()
    {        
        weapon_r = GetComponentInChildren<WeaponPosition>().transform;  // 무기가 붙는 위치를 컴포넌트로 찾기
        weaponPS = weapon_r.GetComponentInChildren<ParticleSystem>();   // 무기에 붙어있는 파티클 시스템 가져오기
    }

    /// <summary>
    /// 무기의 이팩트를 키고 끄는 함수
    /// </summary>
    /// <param name="on">true면 무기 이팩트를 켜고, flase면 무기 이팩트를 끈다.</param>
    public void WeaponEffectSwitch(bool on)
    {
        if( weaponPS != null )
        {
            if(on)
            {
                weaponPS.Play();    // 파티클 이팩트 재생 시작
            }
            else
            {
                weaponPS.Stop();    // 파티클 이팩트 재생 중지
            }
        }
    }
}
