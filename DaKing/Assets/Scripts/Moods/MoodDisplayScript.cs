using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class MoodDisplayScript : MonoBehaviour {

    private IMoodEffect[] arrMoodEffects = new IMoodEffect[4];
    
    void Awake()
    {
        this.init();
    }

    private void init()
    {
        arrMoodEffects[0] = new MoodEffectSaturation();
        arrMoodEffects[1] = new MoodEffectVignette();
        arrMoodEffects[2] = new MoodEffectBloomIntensity();
        arrMoodEffects[3] = new MoodEffectBloomBlurSize();
        //Debug.Log("Instantiated");
    }

    public void handleMood(int newMood)
    {
        float moodPercent = newMood / 100f;
        //Debug.Log("moodPercent = "+moodPercent);
        for (int i = 0; i < arrMoodEffects.Length; ++i)
        {
            arrMoodEffects[i].SetEffect(moodPercent);
            StartCoroutine(arrMoodEffects[i].updateEffect(moodPercent));
        }
    }
}
