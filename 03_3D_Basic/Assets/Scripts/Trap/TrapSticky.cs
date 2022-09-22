using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSticky : TrapBase
{
    public float speedDebuff = 0.5f;
    public float duration = 3.0f;

    float originalSpeed = 0.0f;
    Player player = null;

    ParticleSystem ps;

    private void Awake()
    {
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    protected override void TrapActivate(GameObject target)
    {
        if (player == null)
        {
            //Debug.Log("함정 발동");
            player = target.GetComponent<Player>();
            originalSpeed = player.moveSpeed;
            player.moveSpeed *= speedDebuff;
            ps.Play();
        }
        else
        {
            //Debug.Log("디버프 해제 초기화");
            StopAllCoroutines();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                //Debug.Log("나갔다.");
                StartCoroutine(ReleaseDebuff());
            }
        }
    }

    IEnumerator ReleaseDebuff()
    {
        yield return new WaitForSeconds(duration);
        //Debug.Log("디버프 해제");
        player.moveSpeed = originalSpeed;
        originalSpeed = 0.0f;
        player = null;
    }
}
