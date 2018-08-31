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
    public Camera mainCamera;
    public float maxValue = 0;
    public float magnificationPower = 0.0f;

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
        //Add start and end points
        lineRenderer.positionCount = lineResolution + 2;
    }

    private void Update()
    {
        //Get the coordinates of the edge of the screen
        Vector3 topLeft = getScreenTopLeft();
        Vector3 bottomRight = getScreenBottomRight();

        if(audioAnalyzer.unmutedAudioSource.isPlaying)
        {
            //Save spectrum data
            //スペクトラムデータを保存
            spectrumData = new float[spectrumAnalyzer.numSamples];
            audioAnalyzer.unmutedAudioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

            lineRenderer.SetPosition(0, new Vector3(topLeft.x, 0, 0));
            for (int i = 0; i < lineResolution; ++i)
            {
                float height = 0.0f;
                //for (int n = 0; n < spectrumData.Length * 0.25f; ++n)
                //{
                //    height += spectrumData[n];
                //}
                height = spectrumData[i] * magnificationPower;
                if (height > maxValue)
                {
                    height = maxValue;
                }

                //lineRenderer.SetPosition(i, new Vector3(i * 0.5f, Mathf.Sin(i + Time.time), 0.0f));

                float x = topLeft.x + ((bottomRight.x - topLeft.x) / lineResolution) * i;
                lineRenderer.SetPosition(i + 1, new Vector3(x, height, 0f));
            }
            lineRenderer.SetPosition(lineResolution + 1, new Vector3(bottomRight.x, 0, 0));
        }
    }

    private Vector3 getScreenTopLeft()
    {
        // 画面の左上を取得
        Vector3 topLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);
        // 上下反転させる
        topLeft.Scale(new Vector3(1f, -1f, 1f));
        return topLeft;
    }

    private Vector3 getScreenBottomRight()
    {
        // 画面の右下を取得
        Vector3 bottomRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        // 上下反転させる
        bottomRight.Scale(new Vector3(1f, -1f, 1f));
        return bottomRight;
    }
}
