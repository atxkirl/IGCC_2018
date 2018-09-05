using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWave : MonoBehaviour
{
    public float waveSpeed = 1.0f;
    public Vector3 vanishingPoint;


    void Start()
    {
	}

    void Update()
    {
        this.transform.position = new Vector3(transform.position.x - waveSpeed, 0, 0);

        if(transform.position.x <= vanishingPoint.x)
        {
            Destroy(this.gameObject);
        }
    }
}
