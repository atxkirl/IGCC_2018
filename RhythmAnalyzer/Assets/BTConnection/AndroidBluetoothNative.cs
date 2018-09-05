using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AndroidBluetoothNative : MonoBehaviour
{
    private bool isRunning = false;
    private AndroidJavaObject jar;

    public void Run()
    {
        jar = new AndroidJavaObject("com.Quant.BTTest.BTInterop");
        bool success = jar.Call<bool>("start");
        if (success)
        {
            isRunning = true;
            print("Bluetooth start success!");
        }
        else
        {
            isRunning = false;
            // Most likely Bluetooth not active OR Bluetooth timeout. 
            // If timeout then just try again at least one more time during the same program execution. 
            // (didn't bother to dig into changing the Bluetooth timeout time :P)
            print("Bluetooth start failed!");
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            try
            {
                // Fetch data from internal inputstream to databuffer.
                // Could also be done with a workerthread inside java module but it did not work right away so here's the previous working version.
                jar.Call("listenForData");

                // Useless function for fetching byte count. Buffer lenght has the same data.
                //int value = jar.Call<int>("availableBuffer");
                //print("Bytes in buffer: " + value);

                // Get the RAW (and in this case ASCII encoded) bytes from databuffer. Not separated or ordered in any way so manual buffering and line separation needed.
                // (meaning if constantly sending a string "Hello", 1 call may have bytes for string "He" and the next "lloHelloH" and so on.
                byte[] data = jar.Call<byte[]>("getBufferData");
                if (data.Length > 0)
                {
                    string text = System.Text.Encoding.ASCII.GetString(data);
                    print("ASCII text recieved: " + text);
                }
            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }
    }
}
