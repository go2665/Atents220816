using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class Test_LoadingScene : MonoBehaviour
{
    /// <summary>
    /// 불러올 씬의 이름(씬은 build setting에 등록되어 있어야 함)
    /// </summary>
    public string nextSceneName = "Test_Seemless_00";

    /// <summary>
    /// 로딩바가 증가하는 속도(1초에 증가하는 양)
    /// </summary>
    public float loadingBarSpeed = 1.0f;

    /// <summary>
    /// 로딩 상태 표시용 슬라이더
    /// </summary>
    Slider slider;

    /// <summary>
    /// 로딩 상태 표시용 텍스트
    /// </summary>
    TextMeshProUGUI loadingText;

    /// <summary>
    /// 비동기 로딩 상태를 확인하기 위한 클래스의 인스턴스
    /// </summary>
    AsyncOperation async;

    /// <summary>
    /// 입력 처리용 인풋 액션
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 로딩 중 loadingText의 변화를 만들기 위한 코루틴
    /// </summary>
    IEnumerator loadingTextCoroutine;

    /// <summary>
    /// 로딩이 완료되었는지 표시하는 변수
    /// </summary>
    bool loadingComplete = false;

    /// <summary>
    /// 로딩 바의 value가 목표로 하는 값
    /// </summary>
    float loadRatio = 0.0f;

    private void Awake()
    {
        inputActions = new PlayerInputActions();    // 인풋 액션 만들기
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();                   // 필요한 액션맵 활성화
        inputActions.UI.AnyKey.performed += Press;  // 액션별 필요한 함수 연결
        inputActions.UI.Click.performed += Press;
    }

    private void OnDisable()
    {
        inputActions.UI.Click.performed -= Press;   // 액션별 함수 연결 해제
        inputActions.UI.AnyKey.performed -= Press;
        inputActions.UI.Disable();                  // 액션맵 비활성화
    }
    
    private void Start()
    {
        slider = FindObjectOfType<Slider>();                // 컴포넌트 찾기
        loadingText = FindObjectOfType<TextMeshProUGUI>();

        loadingTextCoroutine = LoadingTextProgress();       // 코루틴 저장해 놓기(나중에 stop을 시켜야하기 때문에 저장)
        StartCoroutine(loadingTextCoroutine);               // 로딩 텍스트 변경용 코루틴 실행
        StartCoroutine(LoadScene());                        // 비동기 로딩용 코루틴 실행
    }

    private void Update()
    {
        if(slider.value < loadRatio)                            // loadRatio가 슬라이더에서 표시하는 것보다 더 크면
        {
            slider.value += (Time.deltaTime * loadingBarSpeed); // 1초에 loadingBarSpeed만큼 증가
        }
        else
        {
            slider.value = loadRatio;                           // 넘치면 슬라이더 값은 loadRatio로 설정
        }
    }

    /// <summary>
    /// 키보드가 아무키나 눌려지거나 마우스 좌/우 버튼이 눌러졌을 때 실행
    /// </summary>
    /// <param name="_"></param>
    private void Press(InputAction.CallbackContext _)
    {
        if(loadingComplete)                     // 입력이 들어왔는데 로딩이 완료 되었으면
        {
            async.allowSceneActivation = true;  // 로드한 씬을 활성화하도록 설정
        }
    }

    /// <summary>
    /// 비동기로 씬으로 로딩하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadScene()
    {
        slider.value = 0.0f;    // 초기화
        loadRatio = 0.0f;
        async = SceneManager.LoadSceneAsync(nextSceneName);     // 비동기 로딩 시작
        async.allowSceneActivation = false;                     // 로드 준비만 해놓도록 설정

        while (loadRatio < 1.0f)
        {
            loadRatio = async.progress + 0.1f;  // 비동기 로딩 진행 상황에 따라 loadRatio 증가(loadRatio가 1이 될 때까지 반복)
            yield return null;                  // 다음 프레임까지 대기
        }        

        // 이곳에 도착했을 때 slider는 아직 끝까지 안왔기 때문에 slider가 1이 될 때까지 대기
        yield return new WaitForSeconds((loadRatio - slider.value) / loadingBarSpeed);

        Debug.Log("Loading Complete");
        StopCoroutine(loadingTextCoroutine);        // loadingText를 변경하는 코루틴을 중지
        loadingComplete = true;                     // 로딩이 끝났다고 표시

        loadingText.text = "Loading\nComplete.";    // 로딩이 끝났다고 텍스트도 표시
    }

    /// <summary>
    /// loadingText 변경용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadingTextProgress()
    {
        float waitTime = 0.2f;  // 글자가 변경되는 간격
        WaitForSeconds wait = new WaitForSeconds(waitTime); // 가비지가 계속 생성되는 것을 방지하기 위해 미리 만들기
        int index = 0;          // texts에서 몇번째를 보여줄 것인지 결정
        // 출력할 글자를 미리 배열로 만들어 놓기
        string[] texts = { "Loading", "Loading .", "Loading . .", "Loading . . .", "Loading . . . .", "Loading . . . . ." };

        while (true)
        {
            loadingText.text = texts[index];    // loadingText 변경

            index++;                            // index 증가
            index %= texts.Length;              // 배열 크기 안쪽만 값이 설정되도록 처리

            yield return wait;                  // waitTime초만큼 대기
        }
    }
}
