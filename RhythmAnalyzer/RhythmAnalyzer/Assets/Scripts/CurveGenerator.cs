using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a line by using lower audio frequencies.
/// </summary>
public class CurveGenerator : MonoBehaviour
{
    public GameObject audioController;
    public Camera mainCamera;

    public int lineResolution;
    public float maxValue = 0.0f;
    public float magnificationPower = 0.0f;
    public int divisionNum = 0;

    private List<Vector3> vertexList = new List<Vector3>();
    private List<Vector2> uxList = new List<Vector2>();
    private List<int> indexList = new List<int>();

    private AudioAnalyzer audioAnalyzer;
    private SpectrumAnalyzer spectrumAnalyzer;
    private float[] spectrumData;
    private Mesh mesh;
    private MeshCollider meshCollider;

    [SerializeField]
    private MeshFilter meshFilter;

    private void Start()
    {
        //Get components
        audioAnalyzer = audioController.GetComponent<AudioAnalyzer>();
        spectrumAnalyzer = audioController.GetComponent<AudioAnalyzer>().spectrumAnalyzer;
        meshCollider = GetComponent<MeshCollider>();

        //Set the number of positions within the line (ie resolution of the final curve)
        //Add start and end points

        //CreateMesh
        mesh = CreatePlaneMesh();
        meshFilter.mesh = mesh;

        //CreateMeshCollider
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void Update()
    {
        //Get the coordinates of the edge of the screen
        Vector3 topLeft = getScreenTopLeft();
        Vector3 bottomRight = getScreenBottomRight();



        if (audioAnalyzer.unmutedAudioSource.isPlaying)
        {
            //Save spectrum data
            //スペクトラムデータを保存
            spectrumData = new float[spectrumAnalyzer.numSamples];
            audioAnalyzer.unmutedAudioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

            for (int i = 1; i < lineResolution - 1; ++i)
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

                vertexList[1+((divisionNum + 1) * 2) * i] = new Vector3(vertexList[1+((divisionNum + 1) * 2) * i].x, height, 0f);
            }
            //UpdateMesh
            mesh.SetVertices(vertexList);
            meshCollider.sharedMesh = mesh;
        }
    }

    private Mesh CreatePlaneMesh()
    {
        int waveVertexNum = lineResolution + (lineResolution) * divisionNum;
        int meshNum = waveVertexNum;
        Vector3 topLeft = getScreenTopLeft();
        Vector3 bottomRight = getScreenBottomRight();
        float screenXlength = bottomRight.x - topLeft.x;
        float meshLength = screenXlength / waveVertexNum;

        var mesh = new Mesh();

        vertexList.Add(new Vector3(topLeft.x, -4, 0));
        vertexList.Add(new Vector3(topLeft.x, 0, 0));


        Vector3[] vertmp = new Vector3[4];
        int tmp = 0;
        for (int i = 1; i < meshNum; i++)
        {
            vertmp[0] = new Vector3(topLeft.x + (meshLength * i), -4, 0);
            vertmp[1] = new Vector3(topLeft.x + (meshLength * i), 0, 0);

            vertexList.AddRange(new[] {vertmp[0], vertmp[1] });

            if(i % 2 == 0)
            {
                indexList.AddRange(new[] { 0 + tmp, 1 + tmp, 2 + tmp, 2 + tmp, 1 + tmp, 3 + tmp });
                indexList.AddRange(new[] { 0 + tmp + 2, 1 + tmp + 2, 2 + tmp + 2, 2 + tmp + 2, 1 + tmp + 2, 3 + tmp + 2 });

                tmp += 4;
            }
        }
        vertexList.Add(new Vector3(bottomRight.x, -4, 0));
        vertexList.Add(new Vector3(bottomRight.x, 0, 0));
        indexList.AddRange(new[] { 0 + tmp, 1 + tmp, 2 + tmp, 2 + tmp, 1 + tmp, 3 + tmp });
        indexList.AddRange(new[] { 0 + tmp + 2, 1 + tmp + 2, 2 + tmp + 2, 2 + tmp + 2, 1 + tmp + 2, 3 + tmp + 2 });

        //meshに頂点群をセット
        mesh.SetVertices(vertexList);
        //メッシュにどの頂点の順番で面を作るかセット
        mesh.SetIndices(indexList.ToArray(), MeshTopology.Triangles, 0);
        return mesh;
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
