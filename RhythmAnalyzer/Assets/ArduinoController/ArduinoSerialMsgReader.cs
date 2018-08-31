using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoSerialMsgReader : Singleton<ArduinoSerialMsgReader>
{
    public delegate void OnAccelerometerMsgRecieved(string[] msg);
    public event OnAccelerometerMsgRecieved onAccelerometerMsgRecieved = delegate { }; 
    public bool tapped;

    void OnMessageArrived(string msg)
    {
        string[] msgArray = msg.Split('_');
        onAccelerometerMsgRecieved.Invoke(msgArray);
        int check = int.Parse(msgArray[msgArray.Length - 1]);

        if (check == 1)
        {
            tapped = true;
        }
        else
        {
            tapped = false;
        }
    }

    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }
}
