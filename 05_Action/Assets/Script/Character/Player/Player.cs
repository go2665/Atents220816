using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour, IBattle, IHealth, IMana, IEquipTarget
{
    /// <summary>
    /// 무기에 붙어있는 파티클 시스템 컴포넌트
    /// </summary>
    ParticleSystem weaponPS;

    /// <summary>
    /// 무기가 붙어있을 게임오브젝트의 트랜스폼
    /// </summary>
    Transform weapon_r;

    /// <summary>
    /// 방패가 붙어있을 게임 오브젝트의 트랜스폼
    /// </summary>
    Transform weapon_l;

    /// <summary>
    /// 무기가 데미지를 주는 영역의 트리거
    /// </summary>
    Collider weaponBlade;

    Animator anim;  // 애니메이터 컴포넌트

    public float attackPower = 10.0f;      // 공격력
    public float defencePower = 3.0f;      // 방어력
    public float maxHP = 100.0f;    // 최대 HP
    float hp = 100.0f;              // 현재 HP

    public float maxMP = 100.0f;    // 최대 MP
    float mp = 100.0f;              // 현재 MP

    bool isAlive = true;            // 살았는지 죽었는지 확인용

    Inventory inven;

    public float itemPickupRange = 2.0f;

    int money = 0;

    /// <summary>
    /// 파츠별 아이템 장비 현황을 나타내는 변수(슬롯으로 저장)
    /// </summary>
    ItemSlot[] partsSlots;

    /// <summary>
    /// 락온 이펙트
    /// </summary>
    LockOnEffect lockOnEffect;

    /// <summary>
    /// 락온 범위
    /// </summary>
    float lockOnRange = 5.0f;

    // 프로퍼티 ------------------------------------------------------------------------------------
    public float AttackPower => attackPower;

    public float DefencePower => defencePower;

    public float HP
    {
        get => hp;
        set
        {
            if (isAlive && hp != value) // 살아있고 HP가 변경되었을 때만 실행
            {
                hp = value;

                if (hp < 0)
                {
                    Die();
                }

                hp = Mathf.Clamp(hp, 0.0f, maxHP);

                onHealthChange?.Invoke(hp / maxHP);
            }
        }
    }

    public float MaxHP => maxHP;
    public bool IsAlive => isAlive;
    public float MP
    {
        get => mp;
        set
        {
            if (isAlive && mp != value) // 살아있고 mp가 변경되었을 때만 실행
            {
                mp = Mathf.Clamp(value, 0.0f, maxMP);

                onManaChange?.Invoke(mp / maxMP);
            }
        }
    }

    public float MaxMP => maxMP;

    public int Money
    {
        get => money;
        set
        {
            if (money != value)
            {
                money = value;
                onMoneyChange?.Invoke(money);
            }
        }
    }

    /// <summary>
    /// 아이템 장비 현황을 확인할 수 있는 프로퍼티
    /// </summary>
    public ItemSlot[] PartsSlots => partsSlots;

    /// <summary>
    /// 락온한 대상의 트랜스폼
    /// </summary>
    public Transform LockOnTransform => lockOnEffect.transform.parent;


    // 델리게이트 ----------------------------------------------------------------------------------
    public Action<int> onMoneyChange;   // 돈이 변경되면 실행될 델리게이트

    public Action<float> onHealthChange { get; set; }

    public Action<float> onManaChange { get; set; }

    public Action onDie { get; set; }


    public Action<EquipPartType> onEquipItemClear;   // 장비 아이템이 변경되었음을 알리는 델리게이트


    // --------------------------------------------------------------------------------------------

    private void Awake()
    {
        anim = GetComponent<Animator>();

        weapon_r = GetComponentInChildren<WeaponPosition>().transform;  // 무기가 붙는 위치를 컴포넌트의 타입으로 찾기
        weapon_l = GetComponentInChildren<ShildPosition>().transform;   // 방패가 붙는 위치를 컴포넌트의 타입으로 찾기

        partsSlots = new ItemSlot[Enum.GetValues(typeof(EquipPartType)).Length];

        lockOnEffect = GetComponentInChildren<LockOnEffect>();
        LockOff();

        inven = new Inventory(this);
    }

    private void Start()
    {
        hp = maxHP;
        isAlive = true;

        GameManager.Inst.InvenUI.InitializeInventory(inven);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.up, itemPickupRange);
    }
#endif

    /// <summary>
    /// 무기의 이팩트를 키고 끄는 함수
    /// </summary>
    /// <param name="on">true면 무기 이팩트를 켜고, flase면 무기 이팩트를 끈다.</param>
    public void WeaponEffectSwitch(bool on)
    {
        if (weaponPS != null)
        {
            if (on)
            {
                weaponPS.Play();    // 파티클 이팩트 재생 시작
            }
            else
            {
                weaponPS.Stop();    // 파티클 이팩트 재생 중지
            }
        }
    }

    /// <summary>
    /// 무기가 공격 행동을 할 때 무기의 트리거 켜는 함수
    /// </summary>
    public void WeaponBladeEnable()
    {
        if (weaponBlade != null)
        {
            weaponBlade.enabled = true;
        }
    }

    /// <summary>
    /// 무기가 공격 행동이 끝날 때 무기의 트리거를 끄는 함수
    /// </summary>
    public void WeaponBladeDisable()
    {
        if (weaponBlade != null)
        {
            weaponBlade.enabled = false;
        }
    }

    /// <summary>
    /// 무기와 방패를 표시하거나 표시하지 않는 함수
    /// </summary>
    /// <param name="isShow">ture면 표시하고, false면 표시하지 않는다.</param>
    public void ShowWeaponAndSheild(bool isShow)
    {
        weapon_r.gameObject.SetActive(isShow);
        weapon_l.gameObject.SetActive(isShow);
    }

    /// <summary>
    /// 공격용 함수
    /// </summary>
    /// <param name="target">공격할 대상</param>
    public void Attack(IBattle target)
    {
        target?.Defence(AttackPower);
    }

    /// <summary>
    /// 방어용 함수
    /// </summary>
    /// <param name="damage">현재 입은 데미지</param>
    public void Defence(float damage)
    {
        if (isAlive)                // 살아있을 때만 데미지 입음.
        {
            anim.SetTrigger("Hit"); // 피격 애니메이션 재생            
            HP -= (damage - DefencePower);  // 기본 공식 : 실제 입는 데미지 = 적 공격 데미지 - 방어력
        }
    }

    /// <summary>
    /// 죽었을 때 실행될 함수
    /// </summary>
    public void Die()
    {
        isAlive = false;
        ShowWeaponAndSheild(true);
        anim.SetLayerWeight(1, 0.0f);       // 애니메이션 레이어 가중치 제거
        anim.SetBool("IsAlive", isAlive);   // 죽었다고 표시해서 사망 애니메이션 재생
        onDie?.Invoke();
    }

    /// <summary>
    /// 플레이어 주변의 아이템을 획득하는 함수
    /// </summary>
    public void ItemPickup()
    {
        Collider[] items = Physics.OverlapSphere(transform.position, itemPickupRange, LayerMask.GetMask("Item"));

        foreach (var itemCollider in items)
        {
            Item item = itemCollider.gameObject.GetComponent<Item>();

            // 즉시 사용해야 하는 아이템인지 확인
            IConsumable consumable = item.data as IConsumable;
            if (consumable != null)
            {
                // 즉시 사용되는 아이템
                consumable.Consume(this.gameObject);    // 즉시 사용
                Destroy(itemCollider.gameObject);       // 삭제
            }
            else
            {
                // 인벤토리에 들어갈 아이템
                if (inven.AddItem(item.data))           // 인벤토리에 추가하고, 추가가 성공하면
                {
                    Destroy(itemCollider.gameObject);   // 아이템 오브젝트 삭제하기
                }
            }
        }
    }

    /// <summary>
    /// 마나 회복용 함수
    /// </summary>
    /// <param name="totalRegen">전체 회복량</param>
    /// <param name="duration">전체 회복하는데 걸리는 시간</param>
    public void ManaRegenerate(float totalRegen, float duration)
    {
        StartCoroutine(ManaRegeneration(totalRegen, duration));
        //StartCoroutine(ManaRegeneration_Tick(totalRegen, duration));
    }

    /// <summary>
    /// 마나 회복용 코루틴
    /// </summary>
    /// <param name="totalRegen">전체 회복량</param>
    /// <param name="duration">전체 회복하는데 걸리는 시간</param>
    /// <returns></returns>
    IEnumerator ManaRegeneration(float totalRegen, float duration)
    {
        //float timeStart = Time.realtimeSinceStartup;    // 시작 시간 기록
        //Debug.Log($"Regen Start");
        float regenPerSec = totalRegen / duration;      // 초당 회복량 계산
        float timeElapsed = 0.0f;                       // 진행 시간 기록용
        while (timeElapsed < duration)                   // 진행시간이 duration을 지날 때까지 반복
        {
            timeElapsed += Time.deltaTime;              // 진행시간 누적시키기
            MP += Time.deltaTime * regenPerSec;         // MP를 1초에 초당 회복량만큼 증가
            yield return null;                          // 다음 프레임 시작까지 대기
        }
        //Debug.Log($"Regen End : ({Time.realtimeSinceStartup - timeStart})");    // 전체 걸린 시간 측정용
    }

    /// <summary>
    /// 틱마다 마나가 회복되는 코루틴
    /// </summary>
    /// <param name="totalRegen">전체 회복량</param>
    /// <param name="duration">전체 회복하는데 걸리는 시간</param>
    /// <returns></returns>
    IEnumerator ManaRegeneration_Tick(float totalRegen, float duration)
    {
        float tick = 1.0f;                                  // 1번 회복하는 시간 간격(1초에 한번씩 회복이 발생한다)
        int regenCount = Mathf.FloorToInt(duration / tick); // 전체 회복 횟수
        float regenPerTick = totalRegen / regenCount;       // 한 틱당 회복량
        for (int i = 0; i < regenCount; i++)                       // 전체 반복 횟수만큼 for 진행
        {
            MP += regenPerTick;                             // 한 틱당 회복량을 추가
            //Debug.Log($"Regen : {regenPerTick}");
            yield return new WaitForSeconds(tick);          // 다음 틱까지 대기
        }
    }

    /// <summary>
    /// 아이템을 장비하는 함수
    /// </summary>
    /// <param name="part">아이템 장비할 부위</param>
    /// <param name="itemSlot">장비할 아이템이 들어있는 슬롯</param>
    public void EquipItem(EquipPartType part, ItemSlot itemSlot)
    {
        Transform partTransform = GetPartTransform(part);   // 아이템이 장착될 부모 트랜드폼 가져오기

        ItemData_EquipItem equipItem = itemSlot.ItemData as ItemData_EquipItem;
        if (equipItem != null)  // ItemData_EquipItem일때만 처리
        {
            Instantiate(equipItem.equipPrefab, partTransform);  // 아이템을 생성해서 partTransform의 자식으로 붙임
            partsSlots[(int)part] = itemSlot;                   // 아이템이 장비되었다고 표시        

            if (part == EquipPartType.Weapon)
            {
                weaponPS = weapon_r.GetComponentInChildren<ParticleSystem>();   // 무기에 붙어있는 파티클 시스템 가져오기
                weaponBlade = weapon_r.GetComponentInChildren<Collider>();      // 무기의 충돌 영역 가져오기      
            }
        }
    }

    /// <summary>
    /// 아이템을 장비해제하는 함수
    /// </summary>
    /// <param name="part">아이템을 장비해제할 부위</param>
    public void UnEquipItem(EquipPartType part)
    {
        Transform partTransform = GetPartTransform(part);   // 아이템이 장착 해제될 부모 트랜드폼 가져오기
        while (partTransform.childCount > 0)
        {
            Transform child = partTransform.GetChild(0);    // 자식들 모두 제거
            child.parent = null;
            Destroy(child.gameObject);
        }
        partsSlots[(int)part] = null;                       // 아이템 장비가 해제되었다고 표시
        onEquipItemClear?.Invoke(part);                     // 아이템이 해제된 장비를 알림
    }

    /// <summary>
    /// 아이템이 장비될 트랜스폼을 돌려주는 함수
    /// </summary>
    /// <param name="part">확인할 부위</param>
    /// <returns>확인할 부위에 아이템이 붙을 부모 트랜스폼</returns>
    private Transform GetPartTransform(EquipPartType part)
    {
        Transform result = null;
        switch (part)
        {
            case EquipPartType.Weapon:
                result = weapon_r;
                break;
            case EquipPartType.Shield:
                result = weapon_l;
                break;
        }
        return result;
    }

    public void LockOnToggle()
    {
        LockOn();
    }

    void LockOn()
    {
        // lockOnRange 거리 안에 있는 Enemy오브젝트 찾기
        Collider[] enemies = Physics.OverlapSphere(transform.position, lockOnRange, LayerMask.GetMask("AttackTarget"));
        if(enemies.Length > 0)
        {
            // 찾은 적 중 가장 가까이에 있는 적 찾기
            Transform nearest = null;
            float nearestDistance = float.MaxValue;
            foreach(var enemy in enemies)
            {
                Vector3 dir = enemy.transform.position - transform.position;
                float distanceSqr = dir.sqrMagnitude;   // root 연산은 느리기 때문에 sqrMagnitude 사용
                if (distanceSqr < nearestDistance)
                {
                    nearestDistance = distanceSqr;
                    nearest = enemy.transform;
                }
            }

            // 가장 가까이에 있는 적에게 LockOnEffect 붙이기            
            lockOnEffect.SetLockOnTarget(nearest);      // 부모지정과 위치 변경            
        }
        else
        {
            LockOff();  // 주변에 적이 없는데 락온을 시도하면 락온 해제
        }
    }

    void LockOff()
    {
        lockOnEffect.SetLockOnTarget(null);
    }

    /// <summary>
    /// 테스트용 함수. 아이템 추가하기
    /// </summary>
    /// <param name="itemData">추가할 아이템 종류</param>
    public void Test_AddItem(ItemData itemData)
    {
        inven.AddItem(itemData);
    }

    /// <summary>
    /// 테스트용 함수. 아이템 사용하기(장비도 가능)
    /// </summary>
    /// <param name="index">아이템을 사용할 슬롯</param>
    public void Test_UseItem(uint index)
    {
        inven[index].UseSlotItem(this.gameObject);
    }
}
