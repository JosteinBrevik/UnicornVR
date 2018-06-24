using UnityEngine;
using System.Collections;
using System.IO;

public class CameraLightDetection : MonoBehaviour
{
    //webCamTexture used to get pixel info, scoretext displays it
    WebCamTexture webCamTexture;
    //Photo takes input from webCamTexture and displays it on the renderQuad, for testing
    Texture2D photo;
    //darknessResolution decides how many pixels are looked at (every x pixels), updateTimeMillis how often
    //Need testing to see how precise res is needed, and how often we can update the camera without impacting performance
    public int divideResolutionBy = 4;
    public float updateTimeMillis = 500f;
    private int value;          // holds the average grayscale value
    public int threshold = 200; //if value > threshold => faceslapp

    //Instantiates everything 
    void Start()
    {
        webCamTexture = new WebCamTexture
        {
            requestedHeight = 10,
            requestedWidth = 10,
            requestedFPS = updateTimeMillis / 1000
        };

        webCamTexture.Play();
        photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        InvokeRepeating("TakePhoto", 5f, updateTimeMillis / 1000f);
    }


    void Update()
    {

    }
    //Takes a photo, displays it on the quadRenderer. Not needed if you're just getting the score
    void TakePhoto()
    {
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();
        //quadRenderer.material.SetTexture("_MainTex", photo); //Only to display image

        CalculateDarkness(webCamTexture.GetPixels32());
    }

    //Looks at every x pixels, calculates their grayscale score and finds the average lightness of the image
    //x = darknessResolution
    void CalculateDarkness(Color32[] pixels)
    {
        Debug.Log((pixels.Length / divideResolutionBy).ToString());

        int sum = 0;
		for (int i = 0; i < (int)pixels.Length; i += divideResolutionBy)
        {
            sum += (pixels[i].r + pixels[i].b + pixels[i].g) / 3; //Don't want alpha channel
        }
        //Score is average greyscale value, 0 to 255
        value = sum / (pixels.Length / divideResolutionBy);

        // If change, change game state (speed up, jump, shoot etc)
        if (value > threshold)
        {
            //GameManager.Instance.GameState = GameState.SpeedUp;
            Debug.Log(value.ToString() + " pixel val");
        }
        //scoreText.text = value.ToString()
    }

    //TODO: Look at trends of darknessScores, locate spikes and tell listener when someone slaps their face
    // Get delta value, if > ~70 => faceslapp detected?
    //Mean square error? Change from frame to frame?
}