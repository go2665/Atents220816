using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public Action onGoalIn;
    public string nextSceanName;

    ParticleSystem[] goalInEffects;

    private void Awake()
    {
        Transform effectsParent = transform.GetChild(2);
        goalInEffects = effectsParent.GetComponentsInChildren<ParticleSystem>();    // 골인할 때 터트릴 파티클 시스템 찾기
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player")) // 트리거 안에 플레이어가 들어왔을 때
        {
            PlayGoalInEffect();         // 골인 이팩트 터트리기
            
            onGoalIn?.Invoke();            
        }
    }

    void PlayGoalInEffect()
    {
        foreach( var effect in goalInEffects )  // 찾아놓은 골인 파티클 시스템을 전부 실행하기
        {
            effect.Play();
        }
    }

    IEnumerator Wait3Second()
    {
        yield return new WaitForSeconds(3.0f);
        GoNextStage();
    }

    void GoNextStage()
    {
        SceneManager.LoadScene(nextSceanName);
    }
}
