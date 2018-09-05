using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject gameoverScreen;

    private void Start()
    {
        GameController.Instance.winScreen = this.winScreen;
        GameController.Instance.gameoverScreen = this.gameoverScreen;

        GameController.Instance.gamePaused = false;
        GameController.Instance.playTime = 0f;
        GameController.Instance.clipTime = GameController.Instance.audioController.GetComponent<AudioAnalyzer>().mutedAudioSource.clip.length + ObstacleSpawner.Instance.timeOffset;
    }

    private void Update()
    {
        if(!GameController.Instance.gamePaused && GameController.Instance.audioController.GetComponent<AudioAnalyzer>().unmutedAudioSource.isPlaying)
        {
            GameController.Instance.playTime += Time.deltaTime;
        }
    }
}
