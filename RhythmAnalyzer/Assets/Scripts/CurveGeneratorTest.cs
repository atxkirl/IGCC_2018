using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a line by using lower audio frequencies.
/// </summary>
public class CurveGeneratorTest : MonoBehaviour
{
    public List<Transform> points;
    public int lineResolution;
    public GameObject audioController;
    public LineRenderer lineRenderer;

    private float[] spectrumData;

    private void Start()
    {
        //Get components
        lineRenderer = GetComponent<LineRenderer>();

        //Set the number of positions within the line (ie resolution of the final curve)
        lineRenderer.positionCount = lineResolution;
    }

    private void Update()
    {
            for (int i = 0; i < lineResolution; ++i)
            {
                lineRenderer.SetPosition(i, new Vector3(i * 5f, Mathf.Sin(i + Time.time), 0.0f));

            }
    }
}
