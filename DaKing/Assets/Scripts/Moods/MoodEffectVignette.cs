using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class MoodEffectVignette: IMoodEffect
{

    private float updateVelocity    = 0f;
    private float updateDelay       = 0f;
    private float UpdateDuration    = 0.5f;
    private float effectMax         = 0.5f;
    private float effectMin         = 0.0f;

    VignetteAndChromaticAberration vignetteAndChromaticAberration;
    public MoodEffectVignette()
    {
    }

    public IEnumerator updateEffect(float moodPercent)
    {
        vignetteAndChromaticAberration = ResourceManager.instance.getMainCamera().GetComponent<VignetteAndChromaticAberration>();

        float effectTarget = effectMax - MathsHelper.ConvertRange(effectMin, effectMax, moodPercent);
        Debug.Log("Intensity Target = " + effectTarget);

        bool reachedTarget = false;
        while (!reachedTarget)
        {
            vignetteAndChromaticAberration.intensity = Mathf.SmoothDamp(vignetteAndChromaticAberration.intensity, effectTarget, ref updateVelocity, UpdateDuration);

            if (Mathf.Abs(updateVelocity) < 0.01f) reachedTarget = true;
            yield return new WaitForSeconds(updateDelay);
        }
    }
}
