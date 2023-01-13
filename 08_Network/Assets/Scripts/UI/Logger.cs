using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    TextMeshProUGUI log;

    private void Awake()
    {
        log = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Log(string logStr)
    {
        log.text += logStr;
    }
}
