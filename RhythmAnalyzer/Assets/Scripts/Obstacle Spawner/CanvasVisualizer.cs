using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasVisualizer : MonoBehaviour
{
    public GameObject note;
    public List<Transform> spawnPoints;
    public float elapsedTime;
    public float bounceTime;
    public bool randTimeSet;
    public int randIndex;
    public GameObject audioController;

    private void Start()
    {
        elapsedTime = 0f;
        bounceTime = 0f;
        randTimeSet = false;

        spawnPoints = new List<Transform>();
        foreach(Transform child in this.transform)
        {
            spawnPoints.Add(child);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && !audioController.GetComponent<AudioSource>().isPlaying)
        {
            //Start audio playing
            audioController.GetComponent<AudioSource>().Play();
        }

        if(audioController.GetComponent<AudioSource>().isPlaying)
        {
            //Real-time checking through the entire processed list of samples
            //int index = audioController.GetComponent<AudioAnalyzer>().GetIndex(audioController.GetComponent<AudioSource>().time) / 1024;
            //if (audioController.GetComponent<AudioAnalyzer>().spectrumAnalyzer.spectralFluxSamples[index].isPeak)
            //{
            //    int spawnIndex = Random.Range(0, spawnPoints.Count);
            //    GameObject obj = Instantiate(note, spawnPoints[spawnIndex]);
            //    Destroy(obj, 0.5f);
            //}

            //Real-time checking through the list of peak samples
            foreach (SpectralFluxInfo sf in audioController.GetComponent<AudioAnalyzer>().peakInfo)
            {
                if (sf.time <= audioController.GetComponent<AudioSource>().time + 0.0167f && sf.time >= audioController.GetComponent<AudioSource>().time - 0.0167f)
                {
                    int spawnIndex = Random.Range(0, spawnPoints.Count);
                    GameObject obj = Instantiate(note, spawnPoints[spawnIndex]);
                    Destroy(obj, 0.5f);
                }
            }
        }
    }
}
