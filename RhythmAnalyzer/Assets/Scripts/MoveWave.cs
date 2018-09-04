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
        Vector3 p = this.transform.position;
        gameObject.transform.position = new Vector3(p.x - waveSpeed, 0, 0);

        if (transform.position == vanishingPoint)
        {
            Destroy(this);
        }
	}
}
