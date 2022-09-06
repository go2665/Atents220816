using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Stars : Background
{
    SpriteRenderer[] spriteRenderers;

    protected override void Awake()
    {
        base.Awake();   // Background.Awake 실행

        // 스프라이트 랜더러 미리 찾아놓기
        spriteRenderers = new SpriteRenderer[transform.childCount];
        for(int i=0; i<transform.childCount; i++)
        {
            spriteRenderers[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
        }
    }

    protected override void MoveRightEnd(int index)
    {
        base.MoveRightEnd(index);   // 그냥 오른쪽 끝으로 옮기기

        // 랜덤하게 플립시키기
        int rand = Random.Range(0, 4);
        spriteRenderers[index].flipX = ((rand & 0b_01) != 0);
        spriteRenderers[index].flipY = ((rand & 0b_10) != 0);
    }
}
