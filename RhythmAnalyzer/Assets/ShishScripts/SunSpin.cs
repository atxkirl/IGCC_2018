using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSpin : MonoBehaviour {

    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.GetChild(0).transform.Rotate(new Vector3(0, 0, speed));
        transform.GetChild(1).transform.Rotate(new Vector3(0, 0, -speed));
    }
}
