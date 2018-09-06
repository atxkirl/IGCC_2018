using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioVisualizer : MonoBehaviour {

    public Transform[] audioSpectrumObjects;
    [Range(1, 1000)] public float heightMultiplier;
    [Range(64, 8192)] public int numberOfSamples = 1024; //step by 2
    public FFTWindow fftWindow;
    public float lerpTime = 1;
    //public Slider sensitivitySlider;
    public GameObject SpectrumObj;
    public Slider Timing;
    public float RecordTime;
    public Text Output;

    //public GameObject[] Cube;

    private float time;
    private float Light, Dark;
    private bool Calculate;

    public static int Scale;
    public static float Height;

    static public float LightRatio;

    public static int FirstRun;
    public static bool FinishRecord;

    public static int TopFre;
    public static float TopIntensity;
    /*
	 * The intensity of the frequencies found between 0 and 44100 will be
	 * grouped into 1024 elements. So each element will contain a range of about 43.06 Hz.
	 * The average human voice spans from about 60 hz to 9k Hz
	 * we need a way to assign a range to each object that gets animated. that would be the best way to control and modify animatoins.
	*/

    void Start() {

        for (int i = 0; i < 9; i++)
        {
            Instantiate(SpectrumObj, this.transform);
        }

        audioSpectrumObjects = this.GetComponentsInChildren<Transform>();
        for (int i = 1; i < audioSpectrumObjects.Length; i++)
        {

            float x = -7 + (i - 0) * 14f / (audioSpectrumObjects.Length - 1);
            audioSpectrumObjects[i].localPosition = new Vector3(x, 1f, 0f);
        }

        //heightMultiplier = PlayerPrefsManager.GetSensitivity();

        //sensitivitySlider.onValueChanged.AddListener(delegate {
           // SensitivityValueChangedHandler(sensitivitySlider);
        //});

        time = RecordTime + 0.1f;
        Calculate = false;
        FirstRun = 0;
        FinishRecord = false;
        LightRatio = 1;
    }

    void Update()
    {
        //if (time <= RecordTime)
        //{
        //    PhanTichSong();
        //}

        //else if (time > RecordTime && Calculate == true)
        //{
        //    if (Light >= Dark)
        //    {
        //        Output.text = "Light";
        //        LightRatio = Mathf.Clamp(LightRatio + 0.2f, 0f, 1f);
        //    }
        //    if (Light < Dark)
        //    {
        //        Output.text = "Dark";
        //        LightRatio = Mathf.Clamp(LightRatio - 0.2f, 0f, 1f);
        //    }

            

        //    /*
        //    for(int i = 0; i < Cube.Length; i++)
        //    {
        //        Cube[i].GetComponent<Rigidbody>().velocity = new Vector3(0f, 10f, 0f);
        //    }
        //    */
        //    Calculate = false;

        //    if (FirstRun == 3)
        //    {
        //        FinishRecord = true;
        //    }
        //    if (FirstRun == 1)
        //    {
        //        FirstRun = 2;
        //    }
        //}

        //else
            PhanTichSong2();
    }

    private void PhanTichSong()
    {
        float sum = 0;
        float top = 0;
        // initialize our float array
        float[] spectrum = new float[numberOfSamples];

        // populate array with fequency spectrum data
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, fftWindow);


        // hiển thị âm thanh

        for (int i = 1; i < audioSpectrumObjects.Length; i++)
        {

            // apply height multiplier to intensity
            float intensity = spectrum[(i - 1)* (numberOfSamples * 2 / 3 / 8)] * heightMultiplier;

            // calculate object's scale
            float lerpY = Mathf.Lerp(audioSpectrumObjects[i].localScale.y, intensity, lerpTime);
            Vector3 newScale = new Vector3(audioSpectrumObjects[i].localScale.x, lerpY, audioSpectrumObjects[i].localScale.z);

            // appply new scale to object
            audioSpectrumObjects[i].localScale = newScale;

            if(intensity / heightMultiplier > TopIntensity)
            {
                TopIntensity = intensity / heightMultiplier;
                TopFre = i;
            }

            // Debug.Log(TopIntensity);
        }

        // tính dark - light

        for (int i = 0; i < numberOfSamples; i++)
        {
            sum = spectrum[i] + sum;

            if (spectrum[i] > top)
            {
                top = spectrum[i];
                Scale = i;
            }
        }

        Height = sum;
            
        // Debug.Log(top / (sum / (numberOfSamples)) + "  -  " + (sum * 100 / (numberOfSamples)));

            if (sum * 100 / (numberOfSamples) < 0.03)  // âm thanh quá nhỏ
        {
            Light++;
        }

        else if (top / (sum / numberOfSamples) > 30) // âm thanh to và rõ âm
        {
            Light++;
        }
        else // to và không rõ âm
            Dark++;

        time = time + Time.deltaTime;
        Timing.value = time / RecordTime;

        // Debug.Log(Light + "  -  " + Dark);

    }

    private void PhanTichSong2()
    {
        // initialize our float array
        float[] spectrum = new float[numberOfSamples];

        // populate array with fequency spectrum data
        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, fftWindow);


        // hiển thị âm thanh

        for (int i = 1; i < audioSpectrumObjects.Length; i++)
        {

            // apply height multiplier to intensity
            float intensity = spectrum[(i - 1) * (numberOfSamples * 2 / 3 / 8)] * heightMultiplier;

            // calculate object's scale
            float lerpY = Mathf.Lerp(audioSpectrumObjects[i].localScale.y, intensity, lerpTime);
            Vector3 newScale = new Vector3(audioSpectrumObjects[i].localScale.x, lerpY, audioSpectrumObjects[i].localScale.z);

            // appply new scale to object
            audioSpectrumObjects[i].localScale = newScale;

            if (intensity / heightMultiplier > TopIntensity)
            {
                TopIntensity = intensity / heightMultiplier;
                TopFre = i;
            }

            // Debug.Log(TopIntensity);
        }
    }

    public void SensitivityValueChangedHandler(Slider sensitivitySlider) {
        heightMultiplier = sensitivitySlider.value;
    }

    public void StartRecord()
    {
        time = 0;
        Calculate = true;
        Light = 0;
        Dark = 0;
        //FirstRun = true; // Ê Linh Tương Lai sau này bỏ dòng này đi, cái này để test cho đỡ mất thời gian thôi
        TopFre = 0;
        TopIntensity = 0;

        if (FirstRun == 0)
            FirstRun = 1;
    }

}
