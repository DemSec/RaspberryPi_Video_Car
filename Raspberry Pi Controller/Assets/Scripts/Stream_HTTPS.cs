using UnityEngine; // Main stuff
using UnityEngine.UI; // For 'urlInput'
using UnityEngine.Networking; // For 'UnityWebRequest'
using System.Collections; // Main stuff
using System.Net; // Main Network library, most important 
using System.Net.Security; // For 'SslPolicyErrors'
using System.Security.Cryptography.X509Certificates; // For building the trusted certificates list
using UnityStandardAssets.CrossPlatformInput;

public class Stream_HTTPS : MonoBehaviour {

	public InputField urlInput; // Format: 0.0.0.0
	private string url;

	public float GetRate = 0.1f;
	private float nextGet = 0;
	private int requested = 0;

	IEnumerator GetTexture() {
		url = urlInput.text;
		Renderer renderer = GetComponent<Renderer>();

		//ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;

		// ===============
		/*HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + url + "/stream");
		request.ContentType = "image/jpg";
		request.PreAuthenticate = true;
		request.Credentials = new NetworkCredential ("Robot","@213R0b0T");
		request.Method = "GET";*/
		// ^^^
		// Combine
		// vvv
		UnityWebRequest www = UnityWebRequest.GetTexture ("http://" + url + ":8080/?action=snapshot");
		yield return www.Send();
		requested += -1;
		renderer.material.mainTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
		// ===============

		// This is to get a responce - still haven't figured it out...
		/*HttpWebResponse response = (HttpWebResponse) request.GetResponse();
		Stream dataStream = response.GetResponseStream ();
		StreamReader reader = new StreamReader (dataStream);
		string responseFromServer = reader.ReadToEnd ();

		Debug.Log ("responseFromServer=" + responseFromServer );*/

	}
		
	void Update() {
		if (requested < 2 && Time.time > nextGet) {
			StartCoroutine (GetTexture ());
			requested += 1;
			nextGet = Time.time + GetRate;
		}
		if (Time.time > nextGet + 0.5f) {
			StartCoroutine (GetTexture ());
			requested = 0;
			nextGet = Time.time + GetRate;
		}

	}

	private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors) {
		// Accept all Certificates
		return true;
	}
}
