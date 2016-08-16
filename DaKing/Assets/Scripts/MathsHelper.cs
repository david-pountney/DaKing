using UnityEngine;
using System.Collections;

public class MathsHelper {

    public static float ConvertRange(
        float newStart, float newEnd, // desired range
        float val) // value to convert
    {
        return MathsHelper.ConvertRange(0, 1, newStart, newEnd, val);
    }

    public static float ConvertRange(
        float originalStart, float originalEnd, // original range
        float newStart, float newEnd, // desired range
        float val) // value to convert
    {
        float scale = (newEnd - newStart) / (originalEnd - originalStart);
        return newStart + ((val - originalStart) * scale);
    }

}
