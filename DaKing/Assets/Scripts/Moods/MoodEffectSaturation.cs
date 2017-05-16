using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class MoodEffectSaturation : IMoodEffect  {

    private float updateVelocity    = 0f;
    private float updateDelay       = 0f;
    private float UpdateDuration    = 0.5f;
    private float effectMax         = 1f;
    private float effectMin         = 0f;  

    ColorCorrectionCurves colorCorrection;

    public void SetEffect(float moodPercent)
    {
        colorCorrection = Camera.main.GetComponent<ColorCorrectionCurves>();
        float effectTarget = MathsHelper.ConvertRange(effectMin, effectMax, moodPercent);
        colorCorrection.saturation = effectTarget;
    }

    public IEnumerator updateEffect(float moodPercent)
    {
        colorCorrection = Camera.main.GetComponent<ColorCorrectionCurves>();

        float saturationTarget = MathsHelper.ConvertRange(effectMin, effectMax, moodPercent);

        bool reachedTarget = false;
        while (!reachedTarget)
        {
            colorCorrection.saturation = Mathf.SmoothDamp(colorCorrection.saturation, saturationTarget, ref updateVelocity, UpdateDuration);
            if (Mathf.Abs(updateVelocity) < 0.01f) reachedTarget = true;
            yield return new WaitForSeconds(updateDelay);
        }
    }	
}
