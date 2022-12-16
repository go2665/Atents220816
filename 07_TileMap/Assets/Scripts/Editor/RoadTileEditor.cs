using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(RoadTile))]
public class RoadTileEditor : Editor
{
    /// <summary>
    /// 선택된 roadTile을 저장할 변수
    /// </summary>
    RoadTile roadTile;

    void OnEnable()
    {
        // 선택이 되면 자동으로 활성화
        // target : 선택된 유니티 오브젝트
        roadTile = target as RoadTile;  // 캐스팅 시도해서 있으면 값을 넣는다.
    }

    /// <summary>
    /// 인스펙터창에서 필요 정보를 그리는 함수
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();  // 기본적으로 그리던 것은 계속 그림

        if(roadTile != null && roadTile.sprites != null)    // RoadTile이 있고 스프라이트도 있으면
        {
            Texture2D texture;
            EditorGUILayout.LabelField("Sprites Preview");  // 제목 넣기
            GUILayout.BeginHorizontal();                    // 수평으로 그리기 시작
            foreach(var sprite in roadTile.sprites)         // roadTile.sprites들을 하나씩 처리
            {
                texture = AssetPreview.GetAssetPreview(sprite); // 스프라이트를 텍스쳐로 바꾸고
                GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64)); // 크기 잡고
                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);       // 크기 잡은 곳에 텍스쳐 그리기
            }
            GUILayout.EndHorizontal();                      // 수평으로 그리던 것을 끝내기
        }
    }
}

#endif