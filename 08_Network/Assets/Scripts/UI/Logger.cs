using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    /// <summary>
    /// 한번에 출력되는 최대 줄 수
    /// </summary>
    public int maxLineCount = 20;

    /// <summary>
    /// 로그창에 출력될 모든 문자열들.
    /// </summary>
    List<string> logLines;

    /// <summary>
    /// 문자열을 합치기 위한 StringBuilder의 인스턴스
    /// </summary>
    StringBuilder builder;

    /// <summary>
    /// 글자 출력용 UI
    /// </summary>
    TextMeshProUGUI log;

    private void Awake()
    {
        log = GetComponentInChildren<TextMeshProUGUI>();    // 컴포넌트 찾아오기

        logLines = new List<string>(maxLineCount + 5);      // 만약을 대비해서 5개의 여유분 추가
        builder = new StringBuilder(logLines.Capacity);     // 아무리 커져도 logLines 크기를 넘어서지 않기 때문에 logLines.Capacity 크기로 설정
    }

    private void Start()
    {
        Clear();    // 시작할 때 모두 비우기
    }

    /// <summary>
    /// 로거에 문장을 추가하는 함수
    /// </summary>
    /// <param name="logStr">추가할 문장</param>
    public void Log(string logStr)
    {
        logLines.Add(logStr);               // 리스트에 문장 추가하고
        if(logLines.Count > maxLineCount)   // 최대 줄 수를 넘어서면
        {
            logLines.RemoveAt(0);           // 첫번째 줄 삭제하기
        }

        builder.Clear();                    // 빌더 클리어
        foreach(var line in logLines)       // 빌더에 리스트에 들어있는 모든 문장 추가
        {
            builder.AppendLine(line);
        }

        log.text = builder.ToString();      // 빌더에 있는 내용을 하나의 문자열로 합치기
    }

    /// <summary>
    /// 데이터 클리어용 함수
    /// </summary>
    public void Clear()
    {
        log.text = "";
        logLines.Clear();
        builder.Clear();
    }
}
