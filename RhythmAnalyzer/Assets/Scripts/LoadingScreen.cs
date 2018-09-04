using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loaderSpinner;
    public int spinSpeed;

    private bool doOnce = false;

    private void Update()
    {
        //SPIN THE LOADERRRRRRR
        loaderSpinner.transform.Rotate(0f, 0f, -Time.deltaTime * spinSpeed);

        //Leave Loading scene once audio analyzer is finished creating spectrum data
        if (AudioAnalyzer.Instance.isDone && !doOnce)
        {
            SceneController.Instance.LoadNextScene();
            doOnce = true;
        }
    }
}
