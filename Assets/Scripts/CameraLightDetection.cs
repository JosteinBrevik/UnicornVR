using UnityEngine;
using System.Collections;
using System.IO;

public class CameraLightDetection : MonoBehaviour 
{
	WebCamTexture webCamTexture;
	public GameObject renderQuad;
	void Start() {
		Debug.Log ("Started");
		webCamTexture = new WebCamTexture();
		//renderer.material.mainTexture = webCamTexture;
		webCamTexture.Play();
		TakePhoto ();
		Debug.Log ("Took pic");
	}

	void Update(){
		TakePhoto ();
	}
	void TakePhoto(){

		// NOTE - you almost certainly have to do this here:

		//yield return new WaitForEndOfFrame(); 

		// it's a rare case where the Unity doco is pretty clear,
		// http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
		// be sure to scroll down to the SECOND long example on that doco page 

		Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
		photo.SetPixels(webCamTexture.GetPixels());
		photo.Apply();
		Renderer quadRenderer = renderQuad.GetComponent<Renderer>() as Renderer;
		//renderQuad.transform.parent = this.transform;
		//renderQuad.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);

		quadRenderer.material.SetTexture("_MainTex", photo);
		//Encode to a PNG
		//byte[] bytes = photo.EncodeToPNG();
		//Write out the PNG. Of course you have to substitute your_path for something sensible
		//File.WriteAllBytes(your_path + "photo.png", bytes);
	}
}