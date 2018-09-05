using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : SingletonMonoBehaviour<CanvasController>
{
    private void Awake()
    {
#if UNITY_EDITOR
        Debug.Log("Unity Editor");
#endif

#if UNITY_STANDALONE_WIN
      Debug.Log("Stand Alone Windows");
#endif

#if UNITY_ANDROID
        Debug.Log("Android");
        Screen.orientation = ScreenOrientation.Landscape;
#endif
    }
}
