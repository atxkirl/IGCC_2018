using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfBoard : MonoBehaviour {

    public Animator animator;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(animator.GetBool("Jump") || animator.GetBool("Fall"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        if(!animator.GetBool("Jump") && !animator.GetBool("Jump"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
	}
}
