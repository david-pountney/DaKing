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

    public void SetEffect(float moodPercent)
    {
        Debug.Log("MoodEffectVignette(" + moodPercent + ")");
        moodPercent = 1 - moodPercent;
        vignetteAndChromaticAberration = ResourceManager.instance.getMainCamera().GetComponent<VignetteAndChromaticAberration>();
        float effectTarget = MathsHelper.ConvertRange(effectMin, effectMax, moodPercent);

        Debug.Log("Final moodPercent = " + moodPercent);
        Debug.Log("MoodEffectVignette Intensity Target = " + effectTarget);
        vignetteAndChromaticAberration.intensity = effectTarget;
    }

    public IEnumerator updateEffect(float moodPercent)
    {
        vignetteAndChromaticAberration = ResourceManager.instance.getMainCamera().GetComponent<VignetteAndChromaticAberration>();

        float effectTarget = effectMax - MathsHelper.ConvertRange(effectMin, effectMax, moodPercent);
        //Debug.Log("Intensity Target = " + effectTarget);

        bool reachedTarget = false;
        while (!reachedTarget)
        {
            vignetteAndChromaticAberration.intensity = Mathf.SmoothDamp(vignetteAndChromaticAberration.intensity, effectTarget, ref updateVelocity, UpdateDuration);

            if (Mathf.Abs(updateVelocity) < 0.01f) reachedTarget = true;
            yield return new WaitForSeconds(updateDelay);
        }
    }
}
