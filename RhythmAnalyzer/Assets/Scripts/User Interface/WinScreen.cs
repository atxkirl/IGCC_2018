using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    private void Start()
    {
        //deactivate child
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ShowScreen()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
