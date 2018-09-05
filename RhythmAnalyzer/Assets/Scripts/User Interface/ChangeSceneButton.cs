using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    public void ChangeToScene(int sceneID)
    {
        GameController.Instance.gamePaused = false;
        GameController.Instance.audioController.GetComponent<AudioAnalyzer>().StopAudio();
        SceneController.Instance.LoadScene(sceneID);
    }
}
