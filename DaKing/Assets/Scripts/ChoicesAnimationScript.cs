using UnityEngine;
using System.Collections;

/// <summary>
/// This class animates the choices options (yes/no)
/// HACK: This class assumes both the yes and no objects have the same original scale
/// </summary>
public class ChoicesAnimationScript : MonoBehaviour {

    [Tooltip("The local scale value we want the choice objects to become, change above 1f to be larger than it currently is, change lower than 1f to be smaller")]
    public float scaleToValue = 1.1f;

    //The original localScale of both objects
    private Vector3 originalScaleValue;

    void Awake()
    {
        //Store the scale value of the object so we know what value to return it to when the
        //cursor leaves the object
        originalScaleValue = transform.FindChild("yesButton").localScale;
    }

    public void OnSelected(Transform theChoice)
    {
        iTween.ScaleTo(theChoice.gameObject, iTween.Hash("scale", originalScaleValue * scaleToValue));
    }

    public void OnUnselected(Transform theChoice)
    {
        iTween.ScaleTo(theChoice.gameObject, iTween.Hash("scale", originalScaleValue));
    }
}
