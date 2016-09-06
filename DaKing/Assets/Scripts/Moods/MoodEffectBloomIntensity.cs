using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class MoodEffectBloomIntensity : IMoodEffect  {

    private float updateVelocity    = 0f;
    private float updateDelay       = 0f;
    private float UpdateDuration    = 0.5f;
    private float effectMax         = 0.75f;
    private float effectMin         = 0f;

    BloomOptimized bloom;

    public MoodEffectBloomIntensity()
    {
    }

    public void SetEffect(float moodPercent)
    {
        bloom = ResourceManager.instance.getMainCamera().GetComponent<BloomOptimized>();
        float effectTarget = MathsHelper.ConvertRange(effectMin, effectMax, moodPercent);
        bloom.intensity = effectTarget;
    }

    public IEnumerator updateEffect(float moodPercent)
    {
        bloom = Camera.main.GetComponent<BloomOptimized>();

        float effectTarget = MathsHelper.ConvertRange(effectMin, effectMax, moodPercent);
        //Debug.Log("Bloom Intensity Target = " + effectTarget);

        bool reachedTarget = false;
        while (!reachedTarget)
        {
            bloom.intensity = Mathf.SmoothDamp(bloom.intensity, effectTarget, ref updateVelocity, UpdateDuration);
            if (Mathf.Abs(updateVelocity) < 0.01f) reachedTarget = true;
            yield return new WaitForSeconds(updateDelay);
        }
    }	
}
