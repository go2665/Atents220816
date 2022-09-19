using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDoor : Door, IUseableObject
{
    public void Use()
    {
        Debug.Log("Use");        
    }
}
