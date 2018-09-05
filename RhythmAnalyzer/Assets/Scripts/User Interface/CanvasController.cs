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

        //Check if game is being launched for the first time
        if(!PlayerPrefs.HasKey("FirstLaunch"))
        {
            //Does not have key, therefore is first launch

            //Clear all Music url data (just in case)
            PlayerPrefs.DeleteAll();
            //Set the key
            PlayerPrefs.SetInt("FirstLaunch", 0);
        }
    }
}
