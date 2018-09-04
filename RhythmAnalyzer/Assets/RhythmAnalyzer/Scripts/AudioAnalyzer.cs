using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Numerics;
using UnityEngine;
using DSPLib;

public class AudioAnalyzer : SingletonMonoBehaviour<AudioAnalyzer>
{
    ///Audio Clip variables
    private int totalNumberOfSamples;
    private int numberOfChannels;
    private int sampleRate;
    private float clipLength;
    private float[] multiChannelSamples;

    public AudioSource unmutedAudioSource;
    public AudioSource mutedAudioSource;
    public SpectrumAnalyzer spectrumAnalyzer;

    public bool isDone;

    //TESTING VARIABLES
    public List<SpectralFluxInfo> peakInfo;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //Clear both audio sources
        mutedAudioSource.clip = null;
        unmutedAudioSource.clip = null;

        isDone = false;
    }

    private void Update()
    {
        //Get the current index of the from the audio timeline
        //int currentIndex = GetIndex(audioSource.time) / spectrumAnalyzer.numSamples;
    }

    //FUNCTIONS

    /// <summary>
    /// Get timing within the audio file from index.
    /// </summary>
    public float GetSongtime(int index)
    {
        return ((1.0f / sampleRate) * index);
    }

    /// <summary>
    /// Gets the index by converting timing from audio file.
    /// </summary>
    public int GetIndex(float currTime)
    {
        return Mathf.FloorToInt(currTime / (clipLength / totalNumberOfSamples));
    }

    public void SetAudio(AudioClip clip)
    {
        //Set audio clips
        unmutedAudioSource.clip = clip;
        mutedAudioSource.clip = clip;
        //Make sure they aren't playing
        unmutedAudioSource.Stop();
        mutedAudioSource.Stop();

        //Initalize all TESTING variables
        peakInfo = new List<SpectralFluxInfo>();

        //Initialize data from AudioSource component
        numberOfChannels = unmutedAudioSource.clip.channels;
        totalNumberOfSamples = unmutedAudioSource.clip.samples;
        clipLength = unmutedAudioSource.clip.length;
        sampleRate = unmutedAudioSource.clip.frequency;

        //Get spectral data from AudioSource component
        multiChannelSamples = new float[unmutedAudioSource.clip.samples * unmutedAudioSource.clip.channels];
        unmutedAudioSource.clip.GetData(multiChannelSamples, 0);

        //Create a thread to get all of the spectrum data
        Thread spectrumThread = new Thread(AnalyzeFullSpectrum);
        spectrumThread.Start();
    }

    private void AnalyzeFullSpectrum()
    {
        try
        {
            //We only retain the samples of combined channel over the time domain
            float[] processedSamples = new float[totalNumberOfSamples];

            int numProcessed = 0;
            float channelAverage = 0f;

            for(int i = 0; i < multiChannelSamples.Length; ++i)
            {
                channelAverage += multiChannelSamples[i];

                //Store the average of the combined channels
                if((i + 1) % numberOfChannels == 0)
                {
                    processedSamples[numProcessed] = channelAverage / numberOfChannels;
                    ++numProcessed;
                    channelAverage = 0f;
                }
            }

            Debug.Log("Channel combining done. Total number of samples processed: " + processedSamples.Length);

            //Execute Fast-Fourier Transform to return the spectrum data over time
            int spectrumSampleSize = 1024;
            int iterations = processedSamples.Length / spectrumSampleSize;

            FFT fft = new FFT();
            fft.Initialize((System.UInt32)spectrumSampleSize);

            double[] sampleChunk = new double[spectrumSampleSize];
            for (int i = 0; i < iterations; ++i)
            {
                //Grab the current 1024 chunk of audio data
                System.Array.Copy(processedSamples, i * spectrumSampleSize, sampleChunk, 0, spectrumSampleSize);

                //Apply FFT
                double[] windowCoEf = DSP.Window.Coefficients(DSP.Window.Type.Hanning, (uint)spectrumSampleSize);
                double[] scaledSampleChunk = DSP.Math.Multiply(sampleChunk, windowCoEf);
                double scaleFactor = DSP.Window.ScaleFactor.Signal(windowCoEf);

                //Perform FFT and convert output to magnitude
                Complex[] fftSpectrum = fft.Execute(scaledSampleChunk);
                double[] scaledFFTSpectrum = DSP.ConvertComplex.ToMagnitude(fftSpectrum);
                scaledFFTSpectrum = DSP.Math.Multiply(scaledFFTSpectrum, scaleFactor);

                //Convert the chunk values to a single point within audio timeline
                float currSongtime = GetSongtime(i) * spectrumSampleSize;

                //Analyze multitude data using SpectrumAnalyzer to detect peaks
                spectrumAnalyzer.AnalyzeSpectrum(System.Array.ConvertAll(scaledFFTSpectrum, x => (float)x), currSongtime);
            }

            Debug.Log("Spectrum Analysis completed, now getting all Peak data samples.");
            for (float i = 0f; i <= clipLength; i += 0.0167f)
            {
                //Get the index from the audio time
                int index = GetIndex(i) / spectrumAnalyzer.numSamples;
               
                //Only run if index returned is within range of the spectrum analyzer sample-list
                if(index < spectrumAnalyzer.spectralFluxSamples.Count)
                {
                    SpectralFluxInfo info = spectrumAnalyzer.spectralFluxSamples[index];

                    //Make sure sample is a peak
                    if (info.isPeak)
                    {
                        peakInfo.Add(info);
                    }
                }
            }

            Debug.Log("Spectrum Analysis and Peak Sampling done. Thread completed.");
            isDone = true;
        }
        catch(System.Exception ex)
        {
            //Error logging
            Debug.LogError(ex.ToString());
        }
    }
}
