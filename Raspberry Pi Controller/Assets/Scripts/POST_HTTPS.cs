using UnityEngine; // Main stuff
using System.Collections; // Main stuff
using System.Collections.Specialized; // for 'NameValueCollection'
using System.Text; // for 'StringBuilder'
using System.IO; // for 'StreamWriter'
using System.Net; // Main Network library, most important 
using System.Net.Security; // for 'SslPolicyErrors'
using System.Security.Cryptography.X509Certificates; // For building the trusted certificate list

public class POST_HTTPS : MonoBehaviour {

	// Create a function with required inputs
	public void PostForm (string url, int i_Horizontal, int i_Vertical, int i_Horizontal_Alt, int i_Vertical_Alt, bool i_Runmode) {
		// Add corresponding https, URI and maybe a port
		url = "https://" + url + "/post";

		// Apply 'TrustCertificate' (declared on the bottom)
		ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;

		// Other known are: UnityWebRequest, WWW, HttpWebRequest, WWWForm
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		request.ContentType = "application/x-www-form-urlencoded";
		request.PreAuthenticate = true;
		request.Credentials = new NetworkCredential ("Robot","password");
		request.Method = "POST";

		// Make a list of variables to POST
		NameValueCollection nvc = new NameValueCollection();
		nvc.Add ("turning", i_Horizontal.ToString());
		nvc.Add ("camera_x", i_Horizontal_Alt.ToString());
		nvc.Add ("camera_y", i_Vertical_Alt.ToString());
		nvc.Add ("motor_r", i_Vertical.ToString());
		nvc.Add ("motor_l", i_Vertical.ToString());
		if (i_Runmode) {
			nvc.Add ("run_mode", "1");
		}
		StringBuilder postVars = new StringBuilder();
		foreach(string key in nvc) // Put all nvc's in one string
			postVars.AppendFormat("{0}={1}&", key, nvc[key]); 
		postVars.Length -= 1; // clip off the remaining '&'

		// Finaly, send the request using streamWriter
		using (var streamWriter = new StreamWriter (request.GetRequestStream ())) {
			streamWriter.Write (postVars.ToString ());
			// This uses 'using' to avoid using:
			// streamWriter.Close();
		}

		// This is to get a responce - still haven't figured it out... (also causes lag)
		/*HttpWebResponse response = (HttpWebResponse) request.GetResponse();
		Stream dataStream = response.GetResponseStream ();
		StreamReader reader = new StreamReader (dataStream);
		string responseFromServer = reader.ReadToEnd ();

		Debug.Log ("responseFromServer=" + responseFromServer );*/

		// See how often I send the request
		Debug.Log ("HTTPS " + Time.time);
	}

	private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors) {
		// Accept all Certificates
		return true;
	}
}