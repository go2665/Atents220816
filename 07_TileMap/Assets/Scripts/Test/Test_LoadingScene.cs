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
    public string nextSceneName = "Test_Seemless_00";
    public float loadingBarSpeed = 1.0f;

    Slider slider;
    TextMeshProUGUI loadingText;

    AsyncOperation async;

    PlayerInputActions inputActions;

    IEnumerator loadingTextCoroutine;

    bool loadingComplete = false;
    float loadRatio = 0.0f;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.AnyKey.performed += Press;
        inputActions.UI.Click.performed += Press;
    }

    private void OnDisable()
    {
        inputActions.UI.Click.performed -= Press;
        inputActions.UI.AnyKey.performed -= Press;
        inputActions.UI.Disable();
    }
    
    private void Start()
    {
        slider = FindObjectOfType<Slider>();
        loadingText = FindObjectOfType<TextMeshProUGUI>();

        loadingTextCoroutine = LoadingTextProgress();
        StartCoroutine(loadingTextCoroutine);
        StartCoroutine(LoadScene());
    }

    private void Update()
    {
        if(slider.value < loadRatio)
        {
            slider.value += (Time.deltaTime * loadingBarSpeed); // 1초에 loadingBarSpeed만큼 증가
        }
    }

    private void Press(InputAction.CallbackContext _)
    {
        if(loadingComplete)
        {
            async.allowSceneActivation = true;
        }
    }

    IEnumerator LoadScene()
    {
        slider.value = 0.0f;
        loadRatio = 0.0f;
        async = SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;

        while (loadRatio < 1.0f)
        {
            loadRatio = async.progress + 0.1f;
            yield return null;
        }

        yield return new WaitForSeconds(1 / loadingBarSpeed);

        Debug.Log("Loading Complete");
        StopCoroutine(loadingTextCoroutine);
        loadingComplete = true;

        loadingText.text = "Loading\nComplete.";
    }

    IEnumerator LoadingTextProgress()
    {
        yield return null;
    }
}
