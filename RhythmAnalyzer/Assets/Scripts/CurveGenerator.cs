using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a line by using lower audio frequencies.
/// </summary>
public class CurveGenerator : MonoBehaviour
{
    public List<Transform> points;
    public int lineResolution;
    public GameObject audioController;
    public LineRenderer lineRenderer;

    private AudioAnalyzer audioAnalyzer;
    private SpectrumAnalyzer spectrumAnalyzer;
    private float[] spectrumData;

    private void Start()
    {
        //Get components
        audioAnalyzer = audioController.GetComponent<AudioAnalyzer>();
        spectrumAnalyzer = audioController.GetComponent<AudioAnalyzer>().spectrumAnalyzer;
        lineRenderer = GetComponent<LineRenderer>();

        //Set the number of positions within the line (ie resolution of the final curve)
        lineRenderer.positionCount = lineResolution;
    }

    private void Update()
    {
        if(audioAnalyzer.mutedAudioSource.isPlaying)
        {
            //Save spectrum data
            spectrumData = new float[spectrumAnalyzer.numSamples];
            audioAnalyzer.mutedAudioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

            for (int i = 0; i < lineResolution; ++i)
            {
                float height = 0.0f;
                for (int n = 0; i < spectrumData.Length * 0.25f; ++n)
                {
                    height += spectrumData[n];
                }

                //lineRenderer.SetPosition(i, new Vector3(i * 0.5f, Mathf.Sin(i + Time.time), 0.0f));

                lineRenderer.SetPosition(i, new Vector3(i, height, 0f));
            }
        }
    }
}
