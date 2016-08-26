using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityStandardAssets.CrossPlatformInput;

public class POST_Form2 : MonoBehaviour {

	void Update() {
		if (CrossPlatformInputManager.GetButtonDown("Connect")){
			PostForm ("https://192.168.1.2/post");
		}
	}

	void PostForm (string url) {
		ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;

		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		request.ContentType = "application/x-www-form-urlencoded";
		request.Method = "POST";

		NameValueCollection nvc = new NameValueCollection();
		nvc.Add("variable1", "123");
		nvc.Add("variable2", "this");
		StringBuilder postVars = new StringBuilder();
		foreach(string key in nvc)
			postVars.AppendFormat("{0}={1}&", key, nvc[key]);
		postVars.Length -= 1; // clip off the remaining &

		//This
		using (var streamWriter = new StreamWriter(request.GetRequestStream()))
			streamWriter.Write(postVars.ToString());

		//Or this works
		/*var streamWriter = new StreamWriter (request.GetRequestStream ());
		streamWriter.Write (postVars.ToString());

		streamWriter.Close();*/
	}

	private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors) {
		// all Certificates are accepted
		return true;
	}
}