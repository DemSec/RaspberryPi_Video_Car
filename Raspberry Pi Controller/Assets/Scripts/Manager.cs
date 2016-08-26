using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Manager : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown ("Reset")) {
			Application.LoadLevel ("Main");
		}
	}
}
