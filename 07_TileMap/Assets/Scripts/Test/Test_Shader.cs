using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Shader : TestBase
{
    public GameObject phaseSlime;
    public GameObject phaseReverseSlime;
    public GameObject dessolveSlime;
    public GameObject allSlime;

    public float phaseDuration = 2.0f;
    public float dessolveDuration = 2.0f;
    public float allDuration = 1.5f;

    protected override void Test1(InputAction.CallbackContext _)
    {
        StartCoroutine(StartPhase(phaseSlime));
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        StartCoroutine(StartPhase(phaseReverseSlime));
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        StartCoroutine(StartDessolve(dessolveSlime));
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        StartCoroutine(StartAllTest(allSlime));
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

    IEnumerator StartDessolve(GameObject target)
    {
        Renderer renderer = target.GetComponent<SpriteRenderer>();
        Material material = renderer.material;
        material.SetFloat("_Fade", 1.0f);

        float timeElipsed = 0.0f;
        float dessolveDurationNormalize = 1 / dessolveDuration;

        while (timeElipsed < dessolveDuration)
        {
            timeElipsed += Time.deltaTime;

            material.SetFloat("_Fade", 1 - timeElipsed * dessolveDurationNormalize);

            yield return null;
        }
    }

    IEnumerator StartAllTest(GameObject target)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        Material material = renderer.material;

        material.SetFloat("_Phase_Split", 0.0f);
        material.SetFloat("_Dissolve_Fade", 1.0f);

        float timeElipsed = 0.0f;
        float allDurationNormalize = 1 / allDuration;

        while (timeElipsed < dessolveDuration)
        {
            timeElipsed += Time.deltaTime;

            material.SetFloat("_Phase_Split", timeElipsed * allDurationNormalize);
            material.SetFloat("_Dissolve_Fade", 1 - timeElipsed * allDurationNormalize);

            yield return null;
        }
    }
}
