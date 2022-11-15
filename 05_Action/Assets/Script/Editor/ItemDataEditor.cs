using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

// ItemData용 커스텀 에디터를 작성한한다는 표시, 두번째 파라메터를 true로 해서 자식 클래스도 같이 적용받게 만들기
[CustomEditor(typeof(ItemData), true)]
public class ItemDataEditor : Editor
{
    // 선택된 ItemData를 저장할 변수
    ItemData itemData;

    private void OnEnable()
    {
        // target은 에디터에서 선택한 오브젝트
        // target을 ItemData로 캐스팅 시도. 성공하면 null이 아닌 참조값. 실패하면 null
        itemData = target as ItemData;
    }

    public override void OnInspectorGUI()
    {
        if(itemData != null)    // itemData가 있는지 확인
        {
            if (itemData.itemIcon != null)  // itemData에 itemIcon이 있는지 확인
            {
                Texture2D texture;
                EditorGUILayout.LabelField("Item Icon Preview");            // 제목 출력
                texture = AssetPreview.GetAssetPreview(itemData.itemIcon);  // itemData.itemIcon에서 texture가져오기
                if (texture != null)    // 텍스쳐가 있으면
                {
                    GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64)); // 64*64 크기의 영역을 잡고
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);       // 위에서 잡은 영역에 텍스처를 그린다.
                }
            }
        }
        base.OnInspectorGUI();  // 원래 인스팩터 창에서 그려지는 것들
    }
}
#endif