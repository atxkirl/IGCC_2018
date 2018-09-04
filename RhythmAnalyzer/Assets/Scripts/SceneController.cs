using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonMonoBehaviour<SceneController>
{
    public int sceneID = 0;
    public int currScene = 0;

    private bool loadScene = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int sceneToLoad)
    {
        currScene = sceneToLoad;
        StartCoroutine(Load(currScene));
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadNext());
    }

    IEnumerator Load(int sceneToLoad)
    {
        //Start Asynchronous operation to load the new scene
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!async.isDone)
            yield return null;
    }

    IEnumerator LoadNext()
    {
        //Increment sceneID
        sceneID++;
        //Start Asynchronous operation to load the new scene
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneID);

        Debug.Log("Loading Next Scene with ID of '" + sceneID + "'");

        while (!async.isDone)
            yield return null;
    }
}
