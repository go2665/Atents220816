using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridPainter : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject letterPrefab;

    const int gridLineCount = 11;

    private void Awake()
    {
        DrawGridLine();
        DrawGridLetter();
    }

    void DrawGridLine()
    {
        // 세로 선 그리기(세로 선을 옆으로 반복해서 그리기)
        for(int i=0;i<gridLineCount;i++) 
        { 
            GameObject line = Instantiate(linePrefab, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(i,0,1));                    // 시작점 설정
            lineRenderer.SetPosition(1, new Vector3(i,0, 1 - gridLineCount));   // 끝점 설정
        }

        // 가로 선 그리기(가로선을 아래로 반복해서 그리기)
        for (int i = 0; i < gridLineCount; i++)
        {
            GameObject line = Instantiate(linePrefab, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(-1, 0, -i));                // 시작점 설정
            lineRenderer.SetPosition(1, new Vector3(-1+gridLineCount, 0, -i));  // 끝점 설정
        }
    }

    void DrawGridLetter()
    {
        // 알파벳을 가로로 쓰기
        for (int i = 1; i < gridLineCount; i++)
        {
            GameObject letter = Instantiate(letterPrefab, transform);
            letter.transform.position = new Vector3(i - 0.5f, 0, 0.5f);     // 글자 위치 설정
            TextMeshPro text = letter.GetComponent<TextMeshPro>();
            char alphabet = (char)(64 + i);     // 찍을 글자 설정(아스키코드로 65가 'A'. )
            text.text = alphabet.ToString();
        }

        // 숫자를 세로로 쓰기
        for(int i=1;i<gridLineCount;i++)
        {
            GameObject letter = Instantiate(letterPrefab, transform);
            letter.transform.position = new Vector3(-0.5f, 0, 0.5f - i);    // 글자 위치 설정
            TextMeshPro text = letter.GetComponent<TextMeshPro>();            
            text.text = i.ToString();   // 숫자를 그대로 찍어주기
            if( i>9 )
            {
                text.fontSize= 8;       // 두자리 숫자는 크기가 넘치기 때문에 사이즈 수정
            }
        }
    }
}
