using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPreview : SingletonMonoBehaviour<MusicPreview>
{
#if UNITY_ANDROID
    public MobileImporter audioImporter;
#endif

#if UNITY_STANDALONE_WIN
    public NAudioImporter audioImporter;
#endif 

    public AudioSource audioSource;
    public bool playImmediate;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

#if UNITY_ANDROID
    audioImporter = gameObject.AddComponent<MobileImporter>();
#endif

#if UNITY_STANDALONE_WIN
        audioImporter = gameObject.AddComponent<NAudioImporter>();
#endif

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
