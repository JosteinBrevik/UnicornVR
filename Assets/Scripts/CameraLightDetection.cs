using UnityEngine;
using System.Collections;
using System.IO;

public class CameraLightDetection : MonoBehaviour 
{
	WebCamTexture webCamTexture;
	public GameObject renderQuad;
	public int darknessResoution = 1000;
	public TextMesh scoreText;
	Renderer quadRenderer;
	Texture2D photo;
	void Start() {
		webCamTexture = new WebCamTexture();
		//renderer.material.mainTexture = webCamTexture;
		webCamTexture.Play();
		quadRenderer = renderQuad.GetComponent<Renderer>() as Renderer;
		photo = new Texture2D(webCamTexture.width, webCamTexture.height);
		InvokeRepeating ("TakePhoto", 1f, 0.5f);
	}

	void Update(){
		
	}

	void TakePhoto(){
		photo.SetPixels(webCamTexture.GetPixels());
		photo.Apply();
		quadRenderer.material.SetTexture("_MainTex", photo); //Only to display image
		CalculateDarkness(webCamTexture.GetPixels32());
	}

	void CalculateDarkness(Color32[] pixels){
		print (pixels.Length);
		print (pixels);
		int sum = 0;
		for (int i = 0; i < pixels.Length; i+=darknessResoution) {
			sum += pixels [i].r + pixels [i].b + pixels [i].g ; //Don't want alpha channel
		}
		scoreText.text = (sum * darknessResoution/pixels.Length).ToString();
	}
}