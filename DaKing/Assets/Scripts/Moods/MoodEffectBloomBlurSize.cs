using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class MoodEffectBloomBlurSize : IMoodEffect  {

    private float updateVelocity    = 0f;
    private float updateDelay       = 0f;
    private float UpdateDuration    = 0.5f;
    private float effectMax         = 1.5f;
    private float effectMin         = 0f;

    BloomOptimized bloom;

    public MoodEffectBloomBlurSize()
    {
    }

    public IEnumerator updateEffect(float moodPercent)
    {
        bloom = ResourceManager.instance.getMainCamera().GetComponent<BloomOptimized>();

        float effectTarget = MathsHelper.ConvertRange(effectMin, effectMax, moodPercent);
        Debug.Log("Bloom Size Target = " + effectTarget);

        bool reachedTarget = false;
        while (!reachedTarget)
        {
            bloom.blurSize = Mathf.SmoothDamp(bloom.blurSize, effectTarget, ref updateVelocity, UpdateDuration);
            if (Mathf.Abs(updateVelocity) < 0.01f) reachedTarget = true;
            yield return new WaitForSeconds(updateDelay);
        }
    }	
}
