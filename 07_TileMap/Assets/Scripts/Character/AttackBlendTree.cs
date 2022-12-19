using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AttackBlendTree용 스크립트
/// </summary>
public class AttackBlendTree : StateMachineBehaviour
{
    Player player;

    private void Awake()
    {
        // 플레이어 미리 찾아 놓기
        player = FindObjectOfType<Player>();
        //Debug.Log($"Awake : {player}");        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log($"OnStateExit : {player}");
        player.RestoreInputDir();   // 플레이어의 이동 방향 복원
    }

}
