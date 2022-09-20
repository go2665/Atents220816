using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    protected Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            //Debug.Log("문이 열려야 한다.");
            Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("문이 닫혀야 한다.");
            Close();
        }
    }

    public virtual void Open()
    {
        anim.SetBool("IsOpen", true);
    }

    public virtual void Close()
    {
        anim.SetBool("IsOpen", false);
    }
}
