using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 플레이어가 죽었을 때 화면 전체로 뜨는 패널
/// </summary>
public class GameOverPanel : MonoBehaviour
{
    /// <summary>
    /// 투명도가 낮아지는 속도
    /// </summary>
    public float alphaChangeSpeed = 2.0f;   // 1초에 2까지 변경되는 속도(0.5초면 1이 된다.)

    /// <summary>
    /// 전체 알파를 조절하기 위한 캔버스 그룹
    /// </summary>
    CanvasGroup canvasGroup;

    /// <summary>
    /// 전체 플레이 시간 표시용 텍스트
    /// </summary>
    TextMeshProUGUI totalPlayTimeText;

    /// <summary>
    /// 재시작 버튼
    /// </summary>
    Button restartButton;

    private void Awake()
    {
        // 컴포넌트 찾기
        canvasGroup = GetComponent<CanvasGroup>();
        Transform child = transform.GetChild(1);
        totalPlayTimeText = child.GetComponent<TextMeshProUGUI>();
        restartButton = GetComponentInChildren<Button>();

        // 버튼에 함수 등록
        restartButton.onClick.AddListener(OnRestartClick);
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;

        player.onDie += OnPlayerDie;    // 플레이어가 죽을 때 실행되는 델리게이트에 함수 등록
    }

    /// <summary>
    /// 플레이어가 죽었을 때 실행되는 함수
    /// </summary>
    /// <param name="totalPlayTime">플레이어가 시작부터 죽을 때까지 걸린 전체 플레이 시간</param>
    private void OnPlayerDie(float totalPlayTime)
    {
        StartCoroutine(StartAlphaChange());             // 알파값을 증가시키는 코루틴 실행

        GameManager.Inst.MapManager.UnloadAllScene();   // 모든 심리스 맵들을 언로드 시작
        totalPlayTimeText.text = $"Total Play Time\r\n< {Mathf.RoundToInt(totalPlayTime)} sec >";   // 전체 플레이 시간 출력

    }

    /// <summary>
    /// 캔버스 그룹의 알파값을 1까지 증가 시키는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartAlphaChange()
    {
        while(canvasGroup.alpha < 1.0f) // 알파가 1이 될 때까지
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed; // 초당 alphaChangeSpeed만큼 알파를 증가
            yield return null;          // 다음 프레임까지 대기
        }
    }

    /// <summary>
    /// 재시작 버튼을 눌렀을 때 실행될 함수
    /// </summary>
    private void OnRestartClick()
    {
        StartCoroutine(WaitUnload());   // 심리스맵들이 전부 언로드될 때까지 기다린 후 로딩씬을 불러오는 코루틴
    }

    /// <summary>
    /// 심리스맵들이 전부 언로드될 때까지 기다린 후 로딩씬을 불러오는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitUnload()
    {
        MapManager mapManager = GameManager.Inst.MapManager;
        while(!mapManager.IsUnloadAll)          // 전부 언로드가 끝날 때까지 기다리기(매 프레임 확인)
        {
            yield return null;
        }
        SceneManager.LoadScene("LoadingScene"); // 전부 언로드가 끝나면 로딩씬 불러오기
    }
}
