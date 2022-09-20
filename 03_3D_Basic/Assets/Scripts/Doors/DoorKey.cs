using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public Door targetDoor;

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            if (targetDoor != null)
            {
                targetDoor.Open();
            }
            Destroy(this.gameObject);
        }
    }
}
