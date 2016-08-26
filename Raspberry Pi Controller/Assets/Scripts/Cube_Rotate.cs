using UnityEngine;
using System.Collections;

public class Cube_Rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.eulerAngles = transform.eulerAngles + Vector3.up;
	}
}
