using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    Volume postProcessVolume;
    Vignette vignette;

    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<Vignette>(out vignette);
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onLifeTimeChange += OnLifeTimeChange;

        vignette.intensity.value = 0;
    }

    private void OnLifeTimeChange(float time, float maxTime)
    {
        vignette.intensity.value = 1.0f - time/maxTime;
    }
}
