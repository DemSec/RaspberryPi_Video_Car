using UnityEngine; // Main stuff
using UnityEngine.UI; // for 'Image'
using System.Collections; // Main stuff
using UnityStandardAssets.CrossPlatformInput; // For mobile on-screen virtual input

public class Control : MonoBehaviour {

	public InputField urlInput;

	public POST_HTTP postHTTP;
	public POST_HTTPS postHTTPS;

	public GameObject sslButton;
	public GameObject runButton;

	public bool SSL = true;
	public bool RUN = false;

	public float PostRate = 0.1f;
	private float nextPost = 0;

	private int i_Horizontal;
	private int i_Horizontal_Prev;
	private int i_Vertical;
	private int i_Vertical_Prev;
	private int i_Horizontal_Alt;
	private int i_Horizontal_Alt_Prev;
	private int i_Vertical_Alt;
	private int i_Vertical_Alt_Prev;
	private bool i_Runmode;

	void Start() {
		postHTTP = GetComponent<POST_HTTP> ();
		postHTTPS = GetComponent<POST_HTTPS> ();

		SSL = true;
		sslButton.GetComponent<Image> ().color = Color.green;
		RUN = false;
		runButton.GetComponent<Image> ().color = Color.red;
	}
	
	void Update() {
		if (CrossPlatformInputManager.GetButtonDown ("SSL")) {
			if (SSL) {
				sslButton.GetComponent<Image> ().color = Color.red;
				SSL = false;
			} else {
				sslButton.GetComponent<Image> ().color = Color.green;
				SSL = true;
			}
		}
		if (CrossPlatformInputManager.GetButtonDown("Run")) {
			if (RUN) {
				runButton.GetComponent<Image> ().color = Color.red;
				RUN = false;
			} else {
				runButton.GetComponent<Image> ().color = Color.green;
				RUN = true;
			}
		}

		if (CrossPlatformInputManager.GetButtonDown ("Runmode")) {
			// We want to send POST immediately on press of button
			if (SSL) {
				postHTTPS.PostForm (urlInput.text, 0, 0, 0, 0, true); 
			} else {
				postHTTP.PostForm (urlInput.text, 0, 0, 0, 0, true);
			}
		}

		i_Horizontal = Mathf.RoundToInt(CrossPlatformInputManager.GetAxis ("Horizontal") * -100);
		//i_Horizontal = Mathf.RoundToInt(Input.GetAxis ("Horizontal") * -100);
		i_Vertical = Mathf.RoundToInt(CrossPlatformInputManager.GetAxis ("Vertical") * 100);
		//i_Vertical = Mathf.RoundToInt(Input.GetAxis ("Vertical") * 100);
		i_Horizontal_Alt = Mathf.RoundToInt(CrossPlatformInputManager.GetAxis ("Horizontal_Alt") * -100);
		i_Vertical_Alt = Mathf.RoundToInt(CrossPlatformInputManager.GetAxis ("Vertical_Alt") * 100);

		if (RUN && urlInput && (i_Horizontal != i_Horizontal_Prev || i_Vertical != i_Vertical_Prev || i_Horizontal_Alt != i_Horizontal_Alt_Prev || i_Vertical_Alt != i_Vertical_Alt_Prev) && Time.time > nextPost) {
			if (SSL) {
				postHTTPS.PostForm (urlInput.text, i_Horizontal, i_Vertical, i_Horizontal_Alt, i_Vertical_Alt, false);
			} else {
				postHTTP.PostForm (urlInput.text, i_Horizontal, i_Vertical, i_Horizontal_Alt, i_Vertical_Alt, false);
			}

			i_Horizontal_Prev = i_Horizontal;
			i_Vertical_Prev = i_Vertical;
			i_Horizontal_Alt_Prev = i_Horizontal_Alt;
			i_Vertical_Alt_Prev = i_Vertical_Alt;

			nextPost = Time.time + PostRate;
		}
	}
}
