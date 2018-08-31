using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapControllerInputModule : SingletonMonoBehaviour<TapControllerInputModule>
{
    [SerializeField]
    private float activeTapTimeThreshold = 1f;
    private int activeTapCount = 0;
    private float activeTapTimer = 0f;

    private int finalizedTapCount = 0;
    [SerializeField]
    private float finalizedTapWipeTimeThreshold = 0f;
    private float finalizedTapWipeTimer = 0f;

    private void Update()
    {
        if (finalizedTapCount > 0)
        {
            finalizedTapWipeTimer += Time.deltaTime;
            if (finalizedTapWipeTimer >= finalizedTapWipeTimeThreshold)
            {
                finalizedTapCount = 0;
            }
        }

        if (activeTapCount > 0)
        {
            activeTapTimer += Time.deltaTime;
            if (activeTapTimer >= activeTapTimeThreshold)
            {
                finalizedTapCount = activeTapCount;
                activeTapCount = 0;
                activeTapTimer = 0f;
            }
        }
    }

    public bool GetTap()
    {
        if (finalizedTapCount == 1) return true;
        else return false;
    }

    public bool GetDoubleTap()
    {
        if (finalizedTapCount == 2) return true;
        else return false;
    }

    public bool GetTripleTap()
    {
        if (finalizedTapCount >= 3) return true;
        else return false;
    }

    /// <summmary>
    /// Can be replaced with actual input code or used from another script
    /// </summmary>
    public void InputTap()
    {
        if (activeTapCount > 0)
        {
            activeTapTimer = 0f;
        }

        activeTapCount++;
    }
}
