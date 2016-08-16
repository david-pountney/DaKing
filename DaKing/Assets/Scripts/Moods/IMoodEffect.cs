using UnityEngine;
using System.Collections;

public interface IMoodEffect{

    IEnumerator updateEffect(float moodPercent);

}
