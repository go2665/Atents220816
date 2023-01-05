using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Test_HTTP_Request : MonoBehaviour
{
    /// <summary>
    /// http를 통해 데이터를 받아올 url
    /// </summary>
    string url = "http://go26652.dothome.co.kr/HTTP_Data/Data.txt";

    private void Start()
    {
        StartCoroutine(GetWebData());
    }

    /// <summary>
    /// http를 통해 데이터를 받아오는 코루틴(웹으로 받아오는 데이터는 언제 도착할지 알 수 없다.)
    /// </summary>
    /// <returns></returns>
    IEnumerator GetWebData()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);     // 웹에 http를 통해 데이터를 가져오는 요청을 만듬

        yield return www.SendWebRequest();                  // 만든 요청을 실제로 보내고 결과가 도착할 때까지 대기

        if( www.result != UnityWebRequest.Result.Success )  // 결과가 정상적으로 돌아왔는지 확인
        {
            Debug.Log(www.error);                           // 성공이 아니면 에러 출력
        }
        else
        {
            Debug.Log($"Result : {www.downloadHandler.text}");  // 성공이면 데이터 받아와서 처리
        }
    }
}
