using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AccelerometerControls : MonoBehaviour {

    //public ArduinoSerialMsgReader ArduinoSerialMsgReader;
    public Text text;
    public GameObject cube;
    Vector3 initialAccl;
    int Gaxis;
    float currZ;
    float prevZ;
    float timer;
    bool tapped;
    Queue<Vector3> prev = new Queue<Vector3>();
    // Use this for initialization
    void Start () {

        Gaxis = getAxisG(initialAccl);
        currZ = initialAccl.z;
        prevZ = currZ;
        tapped = false;
        timer = 0.0f;
    }
    void Test(string[] msg)
    {

    }
	// Update is called once per frame
	void Update () {
       ArduinoSerialMsgReader.Instance.onAccelerometerMsgRecieved += Test;
        if(prev.Count >= 10)
        {
            prev.Dequeue();
        }
        if(ArduinoSerialMsgReader.Instance.tapped == true)
        {
            tapped = true;
        }
        if (tapped)
        {
            Debug.Log("Adrian");
            cube.SetActive(true);
                ArduinoSerialMsgReader.Instance.tapped = false;
                tapped = false;
                cube.SetActive(false);
        }
        else
        {
            cube.SetActive(false);
        }
        prevZ = currZ;
    }
    int getAxisG(Vector3 initialAccl)
    {
        int axis = 0;
        int difference = 0;
        if (initialAccl.x > initialAccl.y)
        {
            int tempDifference = (int)(initialAccl.x - initialAccl.y);
            if (difference < tempDifference)
            {
                difference = tempDifference;
                axis = 1;
            }
        }
        if (initialAccl.x > initialAccl.z)
        {
            int tempDifference = (int)(initialAccl.x - initialAccl.z);
            if (difference < tempDifference)
            {
                difference = tempDifference;
                axis = 1;
            }
        }
        if (initialAccl.y > initialAccl.x)
        {
            int tempDifference = (int)(initialAccl.y - initialAccl.x);
            if (difference < tempDifference)
            {
                difference = tempDifference;
                axis = 2;
            }
        }
        if (initialAccl.y > initialAccl.z)
        {
            int tempDifference = (int)(initialAccl.y - initialAccl.z);
            if (difference < tempDifference)
            {
                difference = tempDifference;
                axis = 2;
            }
        }
        if (initialAccl.z > initialAccl.x)
        {
            int tempDifference = (int)(initialAccl.z - initialAccl.x);
            if (difference < tempDifference)
            {
                difference = tempDifference;
                axis = 3;
            }
        }
        if (initialAccl.z > initialAccl.y)
        {
            int tempDifference = (int)(initialAccl.z - initialAccl.y);
            if (difference < tempDifference)
            {
                difference = tempDifference;
                axis = 3;
            }
        }
        return axis;
    }
}
