using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor : MonoBehaviour {

	public int hi;
	// Use this for initialization
	void Start () {
		hi = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//if (transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x <= this.GetComponent<MeshFilter>().mesh.bounds.size.x / 2
		//	&& transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x >= -this.GetComponent<MeshFilter>().mesh.bounds.size.x / 2)
		//{
		//	float length = this.GetComponent<MeshFilter>().mesh.bounds.size.x;
		//	List<Vector3> points = new List<Vector3>();
		//	this.GetComponent<MeshFilter>().mesh.GetVertices(points);
		//	float some = length - ((this.transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x)) / (length / points.Count);
		//	for (int i = hi; i < points.Count; i++)
		//	{
		//		float X = (this.transform.position.x - (length / 2)) + ((length / points.Count) * i);
		//		Debug.Log(X);
		//		if (X >= GameObject.FindGameObjectWithTag("Player").transform.position.x - 0.2
		//			&& X <= GameObject.FindGameObjectWithTag("Player").transform.position.x + 0.2 && points[i].y > 0)
		//		{
		//			Debug.Log(points[i].y + 2);
		//			GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().initialHeight = points[i].y + 2;// + (GameObject.FindGameObjectsWithTag("Floor")[i].GetComponent<MeshFilter>().mesh.bounds.size.y/ 2);
		//			hi = i;
		//			break;

		//		}
		//	}
		//}
	}
}
