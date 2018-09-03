using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MusicButton : UIButtonBase, IPointerClickHandler
{
    public GameObject audioController;

    protected override void Start()
    {
        base.Start();
        audioController.GetComponent<AudioSource>().clip = null;
    }

    protected override void Update()
    {
        base.Update();

        if(!audioController.GetComponent<AudioSource>().isPlaying)
        {
            if(audioController.GetComponent<AudioSource>().clip == null && audioController.GetComponent<NAudioImporter>().isLoaded)
            {
                audioController.GetComponent<AudioSource>().clip = audioController.GetComponent<NAudioImporter>().audioClip;
            }
            if(audioController.GetComponent<AudioSource>().clip != null)
            {
                audioController.GetComponent<AudioSource>().Play();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Stop any preview audio and reset audio clip
        if (audioController.GetComponent<AudioSource>().isPlaying)
        {
            audioController.GetComponent<AudioSource>().Stop();
            audioController.GetComponent<AudioSource>().clip = null;
        }

        //Import new audio
        audioController.GetComponent<NAudioImporter>().Import(buttonDescription);
    }
}
