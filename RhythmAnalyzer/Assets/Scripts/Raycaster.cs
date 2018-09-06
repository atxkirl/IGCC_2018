using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : SingletonMonoBehaviour<Raycaster>
{
    private void Update()
    {
        Debug.Log("Raycast");

        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);
            Debug.DrawLine(transform.position, hit.point, Color.cyan);
        }
    }
}
