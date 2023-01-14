using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public Color warningColor;       // 경고용 색상. 
    public Color criticalColor;      // 심각한 강조용 색상.

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
        // [] 사이에 있는 글자는 critical 색상으로 보여주기
        // {} 사이에 있는 글자는 warning 색상으로 보여주기

        // 입력 예시
        // logStr = "[위험]합니다. {경고}입니다."  => 위험은 빨간색, 경고는 노란색으로 보여야 한다.
        // 출력 예시
        // logStr = "<#ff0000>위험</color>합니다. <#ffff00>경고</color>입니다.";
        // Color를 RGB코드로 바꾸기
        //string warnning = ColorUtility.ToHtmlStringRGB(warningColor);   //warnning = "ffff00";

        //string test = string.Format("{0} 입력됨", logStr);
        //int index = logStr.IndexOf("[");                // 문자열 안에 특정 문자열이 있는 위치 찾기
        //test = $"{logStr}에서 [는 {index}번째에 있다.";
        //string[] split = logStr.Split('[', ']');
        //test = split[0] + "<#ff0000>" + split[1] + "</color>" + split[2];
        //test = logStr.Replace("[", "<#ff0000>");
        //test = test.Replace("]", "</color>");
        //logStr = test;
                
        logStr = Emphasize(logStr, '[', ']', criticalColor);    // 괄호 내부를 강조
        logStr = Emphasize(logStr, '{', '}', warningColor);

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

    string Emphasize(string source, char open, char close, Color color)
    {
        string temp = source;
        if (temp.IndexOfAny(new char[] { open, close }) != -1) // 이 문자열에 괄호([],{})가 포함 되어있는지 확인
        {
            // 추가 처리 필요
            string[] split = temp.Split(open, close);
            string colorText = ColorUtility.ToHtmlStringRGB(color);
            temp = $"{split[0]}<#{colorText}>{split[1]}</color>{split[2]}";
        }

        return temp;
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
