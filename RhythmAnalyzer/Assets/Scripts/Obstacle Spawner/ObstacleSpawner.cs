using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class ObstacleSpawner : SingletonMonoBehaviour<ObstacleSpawner>
{
    public GameObject objectToSpawn;
    public GameObject audioController;
    public GameObject playerPosition;
    public float timeOffset;

    private AudioSource mutedSource;
    private AudioSource unmutedSource;
    private SpectrumAnalyzer spectrumAnalyzer;
    private EZObjectPool objectPool;

    private void Start()
    {
        audioController = GameObject.Find("AudioController");
        objectPool = EZObjectPool.CreateObjectPool(objectToSpawn, "ObstacleSpawner", 10, true, true, false);

        //Get audio sources from AudioController in scene
        mutedSource = audioController.GetComponent<AudioAnalyzer>().mutedAudioSource;
        unmutedSource = audioController.GetComponent<AudioAnalyzer>().unmutedAudioSource;
        //Get spectrum analyzer from AudioController
        spectrumAnalyzer = audioController.GetComponent<AudioAnalyzer>().spectrumAnalyzer;

        //Find time-stamp of the first peak (ie time of the first obstacle)
        float time = 0f;
        for(int i = 0; i < spectrumAnalyzer.spectralFluxSamples.Count; ++i)
        {
            if(spectrumAnalyzer.spectralFluxSamples[i].isPeak)
            {
                time = spectrumAnalyzer.spectralFluxSamples[i].time;
                break;
            }
        }

        //Calculate distance between player and spawner to calculate offset
        float distance = Vector3.Distance(transform.position, playerPosition.transform.position);
        //Now calculate time offset based on projectile speed and distance between
        //The time of the first beat must be slightly shorter because of lag
        timeOffset = (distance / objectToSpawn.GetComponent<Projectile>().speed) + (time * 0.9f);
    }

    private void Update()
    {
        //TESTING WITH ENTER KEY
        if (Input.GetKeyDown(KeyCode.Return) && !mutedSource.isPlaying)
        {
            //Start audio playing
            StartAudio();
        }

        //Check if song is playing
        if (mutedSource.isPlaying)
        {
            //Real-time checking through the entire processed list of samples
            int index = audioController.GetComponent<AudioAnalyzer>().GetIndex(mutedSource.time) / 1024;
            if (spectrumAnalyzer.spectralFluxSamples[index].isPeak)
            {
                SpawnObstacle();
            }
        }
    }

    private void StartAudio()
    {
        //Start muted track first to spawn obstacles
        audioController.GetComponent<AudioAnalyzer>().mutedAudioSource.Play();
        //Start unmuted track second to ensure beats co-incide with player's location
        audioController.GetComponent<AudioAnalyzer>().unmutedAudioSource.PlayDelayed(timeOffset);
    }

    public void SpawnObstacle()
    {
        GameObject spawnedObject;
        objectPool.TryGetNextObject(transform.position, transform.rotation, out spawnedObject);
    }
}
