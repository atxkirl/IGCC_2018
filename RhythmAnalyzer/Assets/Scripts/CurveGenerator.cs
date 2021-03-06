﻿//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// Creates a line by using lower audio frequencies.
///// </summary>
//public class CurveGenerator : MonoBehaviour
//{
//    public GameObject audioController;
//    public Camera mainCamera;
//    public Material floorMaterial;

//    public int lineResolution;
//    public float maxValue = 1.0f;
//    public float magnificationPower = 1.0f;
//    public float waveSpeed = 1.0f;
//    public int divisionNum = 0;

//    private List<Vector3> vertexList = new List<Vector3>();
//    private List<Vector2> uxList = new List<Vector2>();
//    private List<int> indexList = new List<int>();

//    private AudioAnalyzer audioAnalyzer;
//    private SpectrumAnalyzer spectrumAnalyzer;
//    private float[] spectrumData;
//    private GameObject previousFrameGameObject = null;
//    private MeshFilter meshFilter;
//    private MeshCollider meshCollider;

//    Mesh mesh;
//    public int delayFrame;
//    int numframe;

//    int nowStartvertex = 1;

//    private void Start()
//    {
//        //Get components
//        audioController = GameObject.Find("AudioController");
//        audioAnalyzer = audioController.GetComponent<AudioAnalyzer>();
//        spectrumAnalyzer = audioController.GetComponent<AudioAnalyzer>().spectrumAnalyzer;
//        meshCollider = GetComponent<MeshCollider>();
//        meshFilter = GetComponent<MeshFilter>();


//        mesh = CreatePlaneMesh();
//        meshFilter.mesh = mesh;
//        meshCollider.sharedMesh = mesh;
//    }

//    private void Update()
//    {
//        //Get the coordinates of the edge of the screen
//        Vector3 topLeft = getScreenTopLeft();
//        Vector3 bottomRight = getScreenBottomRight();
//        float screenXlength = bottomRight.x - topLeft.x;

//        if (audioAnalyzer.mutedAudioSource.isPlaying)
//        {
//            if (numframe <= delayFrame)
//            {
//                numframe++;
//                return;
//            }
//            numframe = 0;

//            //Save spectrum data
//            //スペクトラムデータを保存
//            spectrumData = new float[spectrumAnalyzer.numSamples];
//            audioAnalyzer.mutedAudioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

//            int frameVertexNum = 0;
//            for (int i = 1; i <= lineResolution; ++i)
//            {
//                float height = 0.0f;
//                //for (int n = 0; n < spectrumData.Length * 0.25f; ++n)
//                //{
//                //    height += spectrumData[n];
//                //}
//                height = spectrumData[i] * magnificationPower;
//                if (height > maxValue)
//                {
//                    height = maxValue;
//                }

//                //lineRenderer.SetPosition(i, new Vector3(i * 0.5f, Mathf.Sin(i + Time.time), 0.0f));

//                vertexList[(nowStartvertex + (divisionNum * 2 + 2)) * i] = new Vector3(vertexList[(nowStartvertex + (divisionNum * 2 + 2)) * i].x, height, 0f);

//                frameVertexNum += 2;

//                //interpolation
//                for (int j = 1; j <= divisionNum; j++)
//                {
//                    int tmp1 = (nowStartvertex + (divisionNum * 2 + 2)) * i;
//                    int tmp2 = (nowStartvertex + (divisionNum * 2 + 2)) * (i - 1);
//                    int tmp3 = tmp1 - tmp2 + j;
//                    float y = Mathf.SmoothStep(vertexList[tmp2].y, vertexList[tmp1].y, 1.0f / divisionNum * j);
//                    vertexList[tmp3] = new Vector3(vertexList[tmp3].x, y, 0);
//                    frameVertexNum += 2;
//                }

//            }

//            mesh.SetVertices(vertexList);
//            meshCollider.sharedMesh = mesh;

//            nowStartvertex += frameVertexNum;

//            //Move Wave
//            Vector3 p = this.transform.position;
//            gameObject.transform.position = new Vector3(p.x - waveSpeed, 0, 0);

//        }
//    }

//    private Mesh CreatePlaneMesh()
//    {
//        int waveVertexNum = lineResolution + (lineResolution - 1) * divisionNum;
//        Vector3 topLeft = getScreenTopLeft();
//        Vector3 bottomRight = getScreenBottomRight();
//        float screenXlength = bottomRight.x - topLeft.x;
//        float meshLength = screenXlength / waveVertexNum;

//        var mesh = new Mesh();

//        vertexList.Add(new Vector3(topLeft.x, -4, 0));
//        vertexList.Add(new Vector3(topLeft.x, 0, 0));


//        Vector3[] vertmp = new Vector3[4];
//        int tmp = 0;
//        int frameNum = (int)audioAnalyzer.mutedAudioSource.clip.length;
//        for (int h = 1; h <= frameNum; h++)
//        {
//            for (int i = 1; i <= waveVertexNum; i++)
//            {
//                vertmp[0] = new Vector3(topLeft.x + (meshLength * i) + (h - 1) * screenXlength, 0, 0);
//                vertmp[1] = new Vector3(topLeft.x + (meshLength * i) + (h - 1) * screenXlength, -4, 0);

//                vertexList.AddRange(new[] { vertmp[0], vertmp[1] });

//                if (i % 2 == 0)
//                {
//                    indexList.AddRange(new[] { 0 + tmp, 3 + tmp, 1 + tmp, 0 + tmp, 2 + tmp, 3 + tmp });
//                    indexList.AddRange(new[] { 0 + tmp + 2, 3 + tmp + 2, 1 + tmp + 2, 0 + tmp + 2, 2 + tmp + 2, 3 + tmp + 2 });

//                    tmp += 4;
//                }
//            }
//            vertexList.Add(new Vector3(bottomRight.x + (h - 1) * screenXlength, 0, 0));
//            vertexList.Add(new Vector3(bottomRight.x + (h - 1) * screenXlength, -4, 0));

//            indexList.AddRange(new[] { 0 + tmp, 1 + tmp, 2 + tmp, 2 + tmp, 1 + tmp, 3 + tmp });
//            indexList.AddRange(new[] { 0 + tmp + 2, 1 + tmp + 2, 2 + tmp + 2, 2 + tmp + 2, 1 + tmp + 2, 3 + tmp + 2 });

//        }
//        //meshに頂点群をセット
//        mesh.SetVertices(vertexList);
//        //メッシュにどの頂点の順番で面を作るかセット
//        mesh.SetIndices(indexList.ToArray(), MeshTopology.Triangles, 0);
//        return mesh;
//    }

//    private Vector3 getScreenTopLeft()
//    {
//        // 画面の左上を取得
//        Vector3 topLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);
//        // 上下反転させる
//        topLeft.Scale(new Vector3(1f, -1f, 1f));
//        return topLeft;
//    }

//    private Vector3 getScreenBottomRight()
//    {
//        // 画面の右下を取得
//        Vector3 bottomRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
//        // 上下反転させる
//        bottomRight.Scale(new Vector3(1f, -1f, 1f));
//        return bottomRight;
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Creates a line by using lower audio frequencies.
/// </summary>
public class CurveGenerator : MonoBehaviour
{
    public GameObject audioController;
    public Camera mainCamera;
    public Material floorMaterial;

    public GameObject[] floorDecoPrefabs;
    public float floorDecoVerticalLevelGuide = 0f;
    public int floorDecoDensity = 4;

    public int lineResolution;
    public float waveTop = 0.0f;
    public float waveBottom = -4.0f;
    public float maxValue = 1.0f;
    public float magnificationPower = 1.0f;
    public float waveSpeed = 1.0f;
    public int divisionNum = 0;
    public float perlinNoiseScale = 1.0f;

    private List<Vector3> vertexList = new List<Vector3>();
    private List<Vector2> uxList = new List<Vector2>();
    private List<int> indexList = new List<int>();

    private AudioAnalyzer audioAnalyzer;
    private SpectrumAnalyzer spectrumAnalyzer;
    private float[] spectrumData;
    private GameObject previousFrameGameObject = null;

    public int delayFrame;
    int numframe;
    float time = 0;


    private void Start()
    {
        //Get components
        audioController = GameObject.Find("AudioController");
        audioAnalyzer = audioController.GetComponent<AudioAnalyzer>();
        spectrumAnalyzer = audioController.GetComponent<AudioAnalyzer>().spectrumAnalyzer;

        Vector3 topLeft = getScreenTopLeft();
        Vector3 bottomRight = getScreenBottomRight();
        float screenXlength = bottomRight.x - topLeft.x;

        //Generate new wave
        GameObject floor = new GameObject("floor");
        floor.AddComponent<MeshRenderer>();
        floor.AddComponent<MeshFilter>();
        floor.AddComponent<MeshCollider>();
		floor.AddComponent<floor>();
		floor.GetComponent<MeshFilter>().mesh = CreatePlaneMesh();
        floor.GetComponent<MeshFilter>().mesh.SetVertices(vertexList);
        floor.GetComponent<MeshRenderer>().material = floorMaterial;
        floor.GetComponent<MeshCollider>().sharedMesh = floor.GetComponent<MeshFilter>().mesh;

        floor.transform.position = new Vector3(topLeft.x + screenXlength / 2, 0, 0);
        previousFrameGameObject = floor;
    }

    private void Update()
    {
        //Get the coordinates of the edge of the screen
        Vector3 topLeft = getScreenTopLeft();
        Vector3 bottomRight = getScreenBottomRight();
        float screenXlength = bottomRight.x - topLeft.x;

        if (audioAnalyzer.unmutedAudioSource.isPlaying)
        {
            if (numframe < delayFrame)
            {
                numframe++;
                return;
            }
            numframe = 0;

            //Save spectrum data
            //スペクトラムデータを保存
            spectrumData = new float[spectrumAnalyzer.numSamples];
            audioAnalyzer.unmutedAudioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

            for (int i = 0; i < lineResolution + 1; ++i)
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

                if (i > 0)
                {
                    int tmp1 = 1 + ((divisionNum + 1) * 2) * i;
                    int tmp2 = 1 + ((divisionNum + 1) * 2) * (i - 1);
                    int tmp3;
                    float xSample = 100;
                    float ySample = height / perlinNoiseScale + time; time += Time.deltaTime;

                    height = ((Noise.PerlinNoise(Mathf.Abs(ySample))) * magnificationPower);

                    //if(height < 0)
                    //{
                    //    height = 0;
                    //}

                    vertexList[tmp1] = new Vector3(vertexList[tmp1].x, Mathf.Abs(height) + waveTop, 0f);

                    //interpolation
                    for (int j = 1; j <= divisionNum; j++)
                    {
                        tmp3 = tmp2 + j * 2;
                        float y = Mathf.SmoothStep(vertexList[tmp2].y, vertexList[tmp1].y, 1.0f / divisionNum * j);
                        vertexList[tmp3] = new Vector3(vertexList[tmp3].x, y, 0);
                    }
                }
                else
                {
                    vertexList[0] = vertexList[vertexList.Count - 1];
                    //vertexList[1 + ((divisionNum + 1) * 2) * i] = new Vector3(vertexList[1 + ((divisionNum + 1) * 2) * i].x, vertexList[vertexList.Count].y, 0f);
                }
            }

            //Generate new wave
            GameObject floor = new GameObject("floor");
			floor.tag = "Floor";
            floor.AddComponent<MeshRenderer>();
            floor.AddComponent<MeshFilter>();
			floor.AddComponent<floor>();
			floor.AddComponent<MeshCollider>();
			floor.AddComponent<MoveWave>();
            floor.GetComponent<MoveWave>().waveSpeed = waveSpeed;
            floor.GetComponent<MoveWave>().vanishingPoint = new Vector3(topLeft.x - screenXlength / 2, 0, 0);
            floor.GetComponent<MeshFilter>().mesh = CreatePlaneMesh();
            floor.GetComponent<MeshFilter>().mesh.SetVertices(vertexList);
            floor.GetComponent<MeshRenderer>().material = floorMaterial;
			//floor.GetComponent<MeshCollider>().convex = true;
			floor.GetComponent<MeshCollider>().sharedMesh = floor.GetComponent<MeshFilter>().mesh;

			if (previousFrameGameObject == null)
            {
                floor.transform.position = new Vector3(topLeft.x + screenXlength / 2, 0, 0);
            }
            else
            {
                Vector3 p = previousFrameGameObject.transform.position;
                floor.transform.position = new Vector3(p.x + screenXlength - waveSpeed * 2, 0, 0);
            }

            CreateAndAddFloorDecos(floor.transform, floorDecoVerticalLevelGuide, floor.GetComponent<MeshCollider>().bounds.size.x);
            previousFrameGameObject = floor;
        }
    }

    private void CreateAndAddFloorDecos(Transform floor, float floorDecoVerticalLevel, float floorHorizontalWidth)
    {
        for (int p = 0; p < floorDecoDensity; p++)
        {
            GameObject deco = Instantiate(floorDecoPrefabs[Random.Range(0, floorDecoPrefabs.Length)]);
            deco.transform.SetParent(floor);
            float decoXPos = (floorHorizontalWidth / (floorDecoDensity + 1)) / 2 + (floorHorizontalWidth / floorDecoDensity * p);
            deco.transform.localPosition = new Vector3(decoXPos, floorDecoVerticalLevel, 0f);
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

        vertexList.Add(new Vector3(topLeft.x, waveBottom, 0));
        vertexList.Add(new Vector3(topLeft.x, waveTop, 0));


        Vector3[] vertmp = new Vector3[4];
        int tmp = 0;

        for (int i = 1; i < meshNum; i++)
        {
            vertmp[0] = new Vector3(topLeft.x + (meshLength * i), waveBottom, 0);
            vertmp[1] = new Vector3(topLeft.x + (meshLength * i), waveTop, 0);

            vertexList.AddRange(new[] { vertmp[0], vertmp[1] });

            if (i % 2 == 0)
            {
                indexList.AddRange(new[] { 0 + tmp, 1 + tmp, 2 + tmp, 2 + tmp, 1 + tmp, 3 + tmp });
                indexList.AddRange(new[] { 0 + tmp + 2, 1 + tmp + 2, 2 + tmp + 2, 2 + tmp + 2, 1 + tmp + 2, 3 + tmp + 2 });

                tmp += 4;
            }
        }
        vertexList.Add(new Vector3(bottomRight.x, waveBottom, 0));
        vertexList.Add(new Vector3(bottomRight.x, waveTop, 0));
        indexList.AddRange(new[] { 0 + tmp, 1 + tmp, 2 + tmp, 2 + tmp, 1 + tmp, 3 + tmp });
        indexList.AddRange(new[] { 0 + tmp + 2, 1 + tmp + 2, 2 + tmp + 2, 2 + tmp + 2, 1 + tmp + 2, 3 + tmp + 2 });

        //meshに頂点群をセット
        mesh.SetVertices(vertexList);
        //メッシュにどの頂点の順番で面を作るかセット
        mesh.SetIndices(indexList.ToArray(), MeshTopology.Triangles, 0);
        return mesh;
    }

    private Mesh CreateMesh(Vector2 [] polygons)
    {
        // convert polygon to triangles
        Triangulator triangulator = new Triangulator(polygons);
        int[] tris = triangulator.Triangulate();
        Mesh m = new Mesh();
        Vector3[] vertices = new Vector3[polygons.Length * 2];

        for (int i = 0; i < polygons.Length; i++)
        {
            vertices[i].x = polygons[i].x;
            vertices[i].y = polygons[i].y;
            vertices[i].z = -10; // front vertex
            vertices[i + polygons.Length].x = polygons[i].x;
            vertices[i + polygons.Length].y = polygons[i].y;
            vertices[i + polygons.Length].z = 10;  // back vertex    
        }
        int[] triangles = new int[tris.Length * 2 + polygons.Length * 6];
        int count_tris = 0;
        for (int i = 0; i < tris.Length; i += 3)
        {
            triangles[i] = tris[i];
            triangles[i + 1] = tris[i + 1];
            triangles[i + 2] = tris[i + 2];
        } // front vertices
        count_tris += tris.Length;
        for (int i = 0; i < tris.Length; i += 3)
        {
            triangles[count_tris + i] = tris[i + 2] + polygons.Length;
            triangles[count_tris + i + 1] = tris[i + 1] + polygons.Length;
            triangles[count_tris + i + 2] = tris[i] + polygons.Length;
        } // back vertices
        count_tris += tris.Length;
        for (int i = 0; i < polygons.Length; i++)
        {
            // triangles around the perimeter of the object
            int n = (i + 1) % polygons.Length;
            triangles[count_tris] = i;
            triangles[count_tris + 1] = i + polygons.Length;
            triangles[count_tris + 2] = n;
            triangles[count_tris + 3] = n;
            triangles[count_tris + 4] = n + polygons.Length;
            triangles[count_tris + 5] = i + polygons.Length;
            count_tris += 6;
        }
        m.vertices = vertices;
        m.triangles = triangles;
        m.RecalculateNormals();
        m.RecalculateBounds();
        MeshUtility.Optimize(m);
        return m;
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

    //private float CalculateHeight(int x, int y)
    //{
        
    //}
}

