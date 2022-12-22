using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public Action<Slime> onTarget;
    public Action<Slime> onUnTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Slime slime = collision.GetComponent<Slime>();
            if (slime != null)
            {
                onTarget?.Invoke(slime);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Slime slime = collision.GetComponent<Slime>();
            if (slime != null)
            {
                onUnTarget?.Invoke(slime);
            }
        }
    }
}
