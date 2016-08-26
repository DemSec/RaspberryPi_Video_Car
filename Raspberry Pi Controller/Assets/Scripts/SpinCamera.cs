using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class SpinCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localEulerAngles = Vector3.up * CrossPlatformInputManager.GetAxis ("Horizontal_Alt") * 45 + Vector3.right * CrossPlatformInputManager.GetAxis ("Vertical_Alt") * -30;
	}
}
