using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(999)]
[ExecuteInEditMode]
public class ShadowUnit : MonoBehaviour
{
    [Range(0, 10f)] public float BaseLength = 1f;

    private void OnEnable()
    {
        DayNightHandler.RegisterShadow(this);
    }

    private void OnDisable()
    {
        DayNightHandler.UnregisterShadow(this);
    }
}
