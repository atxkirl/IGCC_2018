using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing all the spectral flux information.
/// </summary>
[System.Serializable]
public class SpectralFluxInfo
{
    public float time;
    public float spectralFlux;
    public float threshold;
    public float prunedSpectralFlux;
    public bool isPeak;
}

public class SpectrumAnalyzer : MonoBehaviour
{
    //Number of samples
    public int numSamples = 1024;

    //Sensitivity Multiplier to check if a rectified spectral flux sample is a peak
    //By default if the sample is 1.5x larger than the average then it is counted as a peak
    private float thresholdMultiplier = 1.25f;

    //Number of samples to average against to detect peak samples
    //By default we need a minimum of 50 samples before we can detect a peak
    private int thresholdSamplesNeeded = 50;

    //List of audio samples
    public List<SpectralFluxInfo> spectralFluxSamples;

    //Spectrum data
    private float[] currSpectrum;
    private float[] prevSpectrum;

    //Index for processing audio samples
    private int indexToProcess;

    //CONSTRUCTORS
    public SpectrumAnalyzer()
    {
        //Initialize list of samples
        spectralFluxSamples = new List<SpectralFluxInfo>();

        //Start processing at the middle of the first window
        indexToProcess = (int)(thresholdSamplesNeeded * 0.5f);

        currSpectrum = new float[numSamples];
        prevSpectrum = new float[numSamples];
    }

    //GETTERS
    public float[] GetCurrSpectrum() { return currSpectrum; }

    //SETTERS
    public void SetCurrSpectrum(float[] _currSpectrum)
    {
        //Sets the current spectrum as the previous spectrum
        currSpectrum.CopyTo(prevSpectrum, 0);
        //Sets the input as the current spectrum
        _currSpectrum.CopyTo(currSpectrum, 0);
    }

    //PRIVATE FUNCTIONS

    /// <summary>
    /// Returns the sum of all the spectral flux up to the sample size (Default: 1024).
    /// </summary>
    private float RectifySpectralFlux()
    {
        //Get the sum of all the spectral flux within the range
        float sum = 0f;
        for (int i = 0; i < numSamples; ++i)
        {
            sum += Mathf.Max(0f, currSpectrum[i] - prevSpectrum[i]);
        }

        return sum;
    }

    /// <summary>
    /// Returns the positive value of the sample at index if it is larger than the minimum threshold.
    /// </summary>
    private float PruneSpectralFlux(int index)
    {
        return Mathf.Max(0f, spectralFluxSamples[index].spectralFlux - spectralFluxSamples[index].threshold);
    }

    /// <summary>
    /// Returns true if audio sample at index is greater than its neighbours (index + 1, index - 1).
    /// </summary>
    private bool IsPeak(int index)
    {
        //Check against the previous sample and the next sample
        return (spectralFluxSamples[index].prunedSpectralFlux > spectralFluxSamples[index + 1].prunedSpectralFlux && 
                spectralFluxSamples[index].prunedSpectralFlux > spectralFluxSamples[index - 1].prunedSpectralFlux);
    }

    /// <summary>
    /// Logs all spectrumFlux data for the given index to the Unity Debug Console.
    /// </summary>
    private void LogSample(int index)
    {
        int startIndex = Mathf.Max(0, index - thresholdSamplesNeeded / 2);
        int endIndex = Mathf.Min(spectralFluxSamples.Count - 1, index + thresholdSamplesNeeded / 2);
        Debug.Log(string.Format(
            "Peak detected at song time {0} with pruned flux of {1} ({2} over thresh of {3}).\n" +
            "Thresh calculated on time window of {4}-{5} ({6} seconds) containing {7} samples.",
            spectralFluxSamples[index].time,
            spectralFluxSamples[index].prunedSpectralFlux,
            spectralFluxSamples[index].spectralFlux,
            spectralFluxSamples[index].threshold,
            spectralFluxSamples[startIndex].time,
            spectralFluxSamples[endIndex].time,
            spectralFluxSamples[endIndex].time - spectralFluxSamples[startIndex].time,
            endIndex - startIndex
        ));
    }

    /// <summary>
    /// Returns the average spectral flux within a range multiplied by the threshold multiplier (Default: 1.5x).
    /// </summary>
    private float GetFluxThreshold(int index)
    {
        //Create range to sample to calculate the average
        int startIndex = (int)Mathf.Max(0, index - thresholdSamplesNeeded * 0.5f);
        int endIndex = (int)Mathf.Min(spectralFluxSamples.Count - 1, index + thresholdSamplesNeeded * 0.5f);

        //Get the sum of all the spectral flux within the range
        float sum = 0f;
        for (int i = startIndex; i < endIndex; ++i)
        {
            sum += spectralFluxSamples[i].spectralFlux;
        }

        //Calculate average and multiply it against the threshold sensitivity
        float average = sum / (endIndex - startIndex);
        return average * thresholdMultiplier;
    }

    //PUBLIC FUNCTIONS

    /// <summary>
    /// Analyzes spectrum data to create an array of spectral flux samples.
    /// </summary>
    public void AnalyzeSpectrum(float[] spectrumData, float time)
    {
        //Set the current spectrum
        SetCurrSpectrum(spectrumData);

        //Get the current spectral flux
        SpectralFluxInfo currInfo = new SpectralFluxInfo();
        currInfo.time = time;
        currInfo.spectralFlux = RectifySpectralFlux();
        //Add the current spectral flux to samples
        spectralFluxSamples.Add(currInfo);

        //Check if there are enough samples to detect peaks
        if(spectralFluxSamples.Count >= thresholdSamplesNeeded)
        {
            //Get the threshold at the index
            spectralFluxSamples[indexToProcess].threshold = GetFluxThreshold(indexToProcess);

            //Get the amplitude amounr above threshold to allow for peak filtering
            spectralFluxSamples[indexToProcess].prunedSpectralFlux = PruneSpectralFlux(indexToProcess);

            //Now process prior sample
            int indexToDetect = indexToProcess - 1;

            if(IsPeak(indexToDetect))
            {
                spectralFluxSamples[indexToDetect].isPeak = true;
            }
            ++indexToProcess;
        }
        else
        {
            //Debug.Log(string.Format("Not enough samples to detect peaks. Current spectral flux sample size of {0} growing to {1}", spectralFluxSamples.Count, thresholdSamplesNeeded));
        }
    }

    public void ResetAnalyzer()
    {
        //Initialize list of samples
        spectralFluxSamples = new List<SpectralFluxInfo>();

        //Start processing at the middle of the first window
        indexToProcess = (int)(thresholdSamplesNeeded * 0.5f);

        currSpectrum = new float[numSamples];
        prevSpectrum = new float[numSamples];
    }
}
