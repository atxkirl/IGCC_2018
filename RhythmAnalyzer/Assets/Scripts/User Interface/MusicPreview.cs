using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPreview : SingletonMonoBehaviour<MusicPreview>
{
    public AudioSource audioSource;
    public NAudioImporter audioImporter;
    public bool playImmediate;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioImporter = GetComponent<NAudioImporter>();

        audioSource.clip = null;
        playImmediate = false;
    }

    private void Update()
    {
        //If AudioSource is not playing anything then check for file to play
        if(!audioSource.isPlaying && playImmediate)
        {
            if (audioSource.clip != null)
                audioSource.Play();
            else if (audioImporter.isLoaded)
                audioSource.clip = audioImporter.audioClip;
        }
    }
}
