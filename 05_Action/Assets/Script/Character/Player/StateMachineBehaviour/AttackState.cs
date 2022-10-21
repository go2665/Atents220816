using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    // 이 상태머신에 들어왔을 때(Entry했을 때) 실행
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        GameManager.Inst.Player.WeaponEffectSwitch(true);   // 무기 이팩트 켜기
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    // 이 상태머신이 Exit로 갈 때 실행
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        GameManager.Inst.Player.WeaponEffectSwitch(false);  // 무기 이팩트 끄기
        animator.SetInteger("ComboState", 0);   // 콤보 상태 리셋
        animator.ResetTrigger("Attack");        // 어택 트리거도 일단 초기화
    }
}
