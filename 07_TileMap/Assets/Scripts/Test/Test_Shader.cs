using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Shader : TestBase
{
    public GameObject phaseSlime;
    public GameObject phaseReverseSlime;
    public float phaseDuration = 2.0f;

    protected override void Test1(InputAction.CallbackContext _)
    {
        StartCoroutine(StartPhase(phaseSlime));
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        StartCoroutine(StartPhase(phaseReverseSlime));
    }

    IEnumerator StartPhase(GameObject target)
    {
        Renderer renderer = target.GetComponent<SpriteRenderer>();
        Material material = renderer.material;
        material.SetFloat("_Thickness", 0.1f);
        material.SetFloat("_Split", 0.0f);

        float timeElipsed = 0.0f;
        float phaseDurationNormalize = 1 / phaseDuration;

        while (timeElipsed < phaseDuration)
        {
            timeElipsed += Time.deltaTime;

            material.SetFloat("_Split", timeElipsed * phaseDurationNormalize);

            yield return null;
        }

        material.SetFloat("_Thickness", 0.0f);
    }
}
