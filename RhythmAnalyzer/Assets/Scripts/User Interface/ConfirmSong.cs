using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfirmSong : UIButtonBase, IPointerClickHandler
{
    public GameObject audioController;
    public string gameScene;

    protected override void Start()
    {
        base.Start();

        //Reset everytime it starts
        GetComponent<Button>().interactable = false;
        MusicPreview.Instance.audioSource.clip = null;
    }

    protected override void Update()
    {
        base.Update();

        //Change color according to whether audio has been selected
        if(MusicPreview.Instance.audioSource.clip != null)
            GetComponent<Button>().interactable = true;
        else
            GetComponent<Button>().interactable = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Audio has been selected
        if(audioController.GetComponent<AudioSource>().clip != null)
        {
            //Stops audio source
            MusicPreview.Instance.audioSource.Stop();
            MusicPreview.Instance.playImmediate = false;

            //Pass audio clip to AudioAnalyzer to start analyzing
            AudioAnalyzer.Instance.SetAudio(MusicPreview.Instance.audioSource.clip);

            //Go to loading screen
            Debug.Log("Loading game");
            SceneController.Instance.LoadNextScene();
        }
    }
}
