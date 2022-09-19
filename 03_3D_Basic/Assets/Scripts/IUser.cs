using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IUser
{
    Action onObjectUse { get; set; }    // Action타입의 프로퍼티
}
