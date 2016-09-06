using UnityEngine;
using System.Collections;

public interface IMoodEffect{

    void SetEffect(float moodPercent);

    IEnumerator updateEffect(float moodPercent);

}
