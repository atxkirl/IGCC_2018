using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public int sceneToLoad = 2;

    private bool doOnce = false;

    private void Start()
    {
        doOnce = false;
    }

    private void Update()
    {
        //Leave Loading scene once audio analyzer is finished creating spectrum data
        if (AudioAnalyzer.Instance.isDone && !doOnce)
        {
            SceneController.Instance.LoadScene(sceneToLoad);
            doOnce = true;
        }
    }
}
