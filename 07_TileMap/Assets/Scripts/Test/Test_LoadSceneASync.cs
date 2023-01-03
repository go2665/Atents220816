using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_LoadSceneASync : TestBase
{
    AsyncOperation async;
    protected override void Test1(InputAction.CallbackContext _)
    {
        //SceneManager.LoadScene(1,LoadSceneMode.Additive);
        async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        async.completed += Async_completed;
    }

    private void Async_completed(AsyncOperation obj)
    {
        Debug.Log("로딩 완료");
        async = null;
    }

    private void Update()
    {
        if(async != null)
        {
            Debug.Log($"Progress : {async.progress}");
        }
    }
}
