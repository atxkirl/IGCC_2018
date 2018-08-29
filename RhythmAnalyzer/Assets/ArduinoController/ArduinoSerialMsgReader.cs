using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoSerialMsgReader : MonoBehaviour
{
    public delegate void OnAccelerometerMsgRecieved(string[] msg);
    public event OnAccelerometerMsgRecieved onAccelerometerMsgRecieved = delegate { };

    void OnMessageArrived(string msg)
    {
        string[] msgArray = msg.Split('_');
        onAccelerometerMsgRecieved.Invoke(msgArray);
        for (int i = 0; i < 2; i++)
        {
            print(msgArray[i]);
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
