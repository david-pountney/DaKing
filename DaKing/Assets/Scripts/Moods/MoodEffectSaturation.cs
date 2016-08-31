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

    public MoodEffectSaturation()
    {
    }

    public IEnumerator updateEffect(float moodPercent)
    {
        colorCorrection = ResourceManager.instance.getMainCamera().GetComponent<ColorCorrectionCurves>();

        float saturationTarget = MathsHelper.ConvertRange(effectMin, effectMax, moodPercent);
        //Debug.Log("Saturation Target = " + saturationTarget);

        bool reachedTarget = false;
        while (!reachedTarget)
        {
            colorCorrection.saturation = Mathf.SmoothDamp(colorCorrection.saturation, saturationTarget, ref updateVelocity, UpdateDuration);
            if (Mathf.Abs(updateVelocity) < 0.01f) reachedTarget = true;
            yield return new WaitForSeconds(updateDelay);
        }
    }	
}
