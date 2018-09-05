using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonMonoBehaviour<GameController>
{
    public enum AudioState
    {
        PLAYING,
        PAUSED,
        ENDED,
        NOTPLAYING
    }

    public AudioState currentAudioState;
    public GameObject audioController;
    public bool gamePaused;

    public GameObject gameoverScreen;
    public GameObject winScreen;

    public float playTime;
    public float clipTime;

    private void Start()
    {
        currentAudioState = AudioState.NOTPLAYING;
        gamePaused = false;
    }

    private void Update()
    {
        //Audio paused
        if (gamePaused)
        {
            //Stop time
            Time.timeScale = 0;
            //Pause music
            audioController.GetComponent<AudioAnalyzer>().PauseAudio();
            //Set audio state
            currentAudioState = AudioState.PAUSED;
        }
        else if(!gamePaused)
        {
            //Resume time
            Time.timeScale = 1;
            //Pause music
            audioController.GetComponent<AudioAnalyzer>().ResumeAudio();
            //Set audio state
            currentAudioState = AudioState.PLAYING;

            //Check playtime against clip time
            if(playTime >= clipTime)
            {
                currentAudioState = AudioState.ENDED;
            }
        }
        
        //Player died
        if (Player.Instance.isDead && gameoverScreen != null)
        {
            //Debug.Log("PLAYER DEAD");
            gameoverScreen.GetComponent<GameOverScreen>().ShowScreen();
            gamePaused = true;
        }
        //Player has won the game
        if (currentAudioState == AudioState.ENDED && winScreen != null)
        {
            //Debug.Log("PLAYER WINS");
            winScreen.GetComponent<WinScreen>().ShowScreen();
            gamePaused = true;
        }
    }
}
