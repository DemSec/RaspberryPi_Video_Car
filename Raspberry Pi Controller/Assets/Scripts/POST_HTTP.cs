using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Net;
using UnityStandardAssets.CrossPlatformInput;

public class POST_HTTP : MonoBehaviour {

	public void PostForm (string url, int i_Horizontal, int i_Vertical, int i_Horizontal_Alt, int i_Vertical_Alt, bool i_Runmode) {
		url = "http://" + url + "/post";

		WWWForm form = new WWWForm ();
		form.AddField ("turning", i_Horizontal);
		form.AddField ("camera_x", i_Horizontal_Alt);
		form.AddField ("camera_y", i_Vertical_Alt);
		form.AddField ("motor_r", i_Vertical);
		form.AddField ("motor_l", i_Vertical);
		if (i_Runmode) {
			form.AddField ("run_mode", 1);
		}

		WWW postRequest = new WWW (url, form);

		//Debug.Log (postRequest.responseHeaders);

		Debug.Log ("HTTP " + Time.time);
	}
}