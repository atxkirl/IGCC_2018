using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class ObstacleSpawner : SingletonMonoBehaviour<ObstacleSpawner>
{
    //Both must have Projectile/Projectile-derived class and must have same speed
    public GameObject sharkPrefab;
    public GameObject birdPrefab;

    public GameObject audioController;
    public GameObject playerPosition;
    public float timeOffset;
    public float minimumTimeBetweenSpawns;

    private AudioSource mutedSource;
    private AudioSource unmutedSource;
    private SpectrumAnalyzer spectrumAnalyzer;

    private bool nthSpawn;
    private bool played;
    private float elapsedTime;
    private float bounceTime;

    private void Start()
    {
        audioController = GameObject.Find("AudioController");

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
        timeOffset = (distance / sharkPrefab.GetComponent<Projectile>().speed) + (time * 0.85f);

        nthSpawn = false;
        played = false;
    }

    private void Update()
    {
        if (!mutedSource.isPlaying && !played)
        {
            //Start audio playing
            StartAudio();
            //Make sure it only plays audio once
            played = true;
        }

        //Check if song is playing
        if (mutedSource.isPlaying)
        {
            //Update elapsedTime
            elapsedTime += Time.deltaTime;

            ////Real - time checking through the entire processed list of samples
            //int index = audioController.GetComponent<AudioAnalyzer>().GetIndex(mutedSource.time) / 1024;
            //if (index < spectrumAnalyzer.spectralFluxSamples.Count)
            //{
            //    if (spectrumAnalyzer.spectralFluxSamples[index].isPeak)
            //    {
            //        nthSpawn = !nthSpawn;
            //        SpawnObstacle();
            //    }
            //}

            //Real-time checking through the list of peak samples
            foreach (SpectralFluxInfo sf in audioController.GetComponent<AudioAnalyzer>().peakInfo)
            {
                if (sf.time <= (mutedSource.time + Time.deltaTime) && sf.time >= (mutedSource.time - Time.deltaTime))
                {
                    nthSpawn = !nthSpawn;
                    SpawnObstacle();
                }
            }
        }
    }

    private void StartAudio()
    {
        //Start muted track first to spawn obstacles
        audioController.GetComponent<AudioAnalyzer>().mutedAudioSource.Play();
        //Start unmuted track second to ensure beats co-incide with player's location
        audioController.GetComponent<AudioAnalyzer>().unmutedAudioSource.PlayDelayed(timeOffset);

        //Tell GameController that game audio has started
        GameController.Instance.currentAudioState = GameController.AudioState.PLAYING;
    }

    public void SpawnObstacle()
    {
        //Only spawn obstacles every alternate peak
        if(nthSpawn && bounceTime < elapsedTime)
        {
            //Randomly select between shark (jump avoid) or bird (slide avoid)
            int random = Random.Range(0, 10);
            //Spawn shark
            if (random < 6)
            {
                GameObject shark = Instantiate(sharkPrefab, transform);
                Destroy(shark, 5f);
            }
            //Spawn birb
            else
            {
                GameObject bird = Instantiate(birdPrefab, transform);
                Destroy(bird, 5f);
            }

            //Update time
            bounceTime = elapsedTime + minimumTimeBetweenSpawns;
        }
    }
}
