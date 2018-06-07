using UnityEngine;
using System.Collections;
using System.IO;

public class CameraLightDetection : MonoBehaviour 
{
	//webCamTexture used to get pixel info, scoretext displays it
	WebCamTexture webCamTexture;
	public TextMesh scoreText;
	//Photo takes input from webCamTexture and displays it on the renderQuad, for testing
	Texture2D photo;
	public GameObject renderQuad;
	Renderer quadRenderer;
	//darknessResolution decides how many pixels are looked at (every x pixels), updateTimeMillis how often
	//Need testing to see how precise res is needed, and how often we can update the camera without impacting performance
	public int darknessResoution = 1000;
	public int updateTimeMillis = 500;

	//Instantiates everything 
	void Start() {
		webCamTexture = new WebCamTexture();
		webCamTexture.Play();
		quadRenderer = renderQuad.GetComponent<Renderer>() as Renderer;
		photo = new Texture2D(webCamTexture.width, webCamTexture.height);
		InvokeRepeating ("TakePhoto", 1f, (float) updateTimeMillis/1000);
	}

	void Update(){
		
	}
	//Takes a photo, displays it on the quadRenderer. Not needed if you're just getting the score
	void TakePhoto(){
		photo.SetPixels(webCamTexture.GetPixels());
		photo.Apply();
		quadRenderer.material.SetTexture("_MainTex", photo); //Only to display image

		CalculateDarkness(webCamTexture.GetPixels32());
	}

	//Looks at every x pixels, calculates their grayscale score and finds the average lightness of the image
	//x = darknessResolution
	void CalculateDarkness(Color32[] pixels){
		int sum = 0;
		for (int i = 0; i < pixels.Length; i+=darknessResoution) {
			sum += (pixels [i].r + pixels [i].b + pixels [i].g)/3 ; //Don't want alpha channel
		}
		//Score is average greyscale value, 0 to 255
		scoreText.text = (sum * darknessResoution/pixels.Length).ToString();
	}

	//TODO: Look at trends of darknessScores, locate spikes and tell listener when someone slaps their face
	//Mean square error? Change from frame to frame?
}