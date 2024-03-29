﻿using UnityEngine;
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
    public int divideResolutionBy = 4;
    public int updateTimeMillis = 500;

    private GameObject mainCam;

    //Instantiates everything 
    void Start()
    {
        GvrViewer.Create();
        webCamTexture = new WebCamTexture();

        webCamTexture.requestedHeight = 10;
        webCamTexture.requestedWidth = 10;
        webCamTexture.requestedFPS = updateTimeMillis / 1000;

        webCamTexture.Play();
        quadRenderer = renderQuad.GetComponent<Renderer>() as Renderer;
        photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        InvokeRepeating("TakePhoto", 1f, (float)updateTimeMillis / 1000);
    }

    void Awake()
    {
        mainCam = GameObject.Find("Main Camera");
    }

    void Update()
    {

    }
    //Takes a photo, displays it on the quadRenderer. Not needed if you're just getting the score
    void TakePhoto()
    {
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();
        quadRenderer.material.SetTexture("_MainTex", photo); //Only to display image

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
		Debug.Log(sum + " " + ((int)pixels.Length / divideResolutionBy).ToString() + " " + mainCam.transform.rotation[0]);
        //Score is average greyscale value, 0 to 255
        scoreText.text = (sum / (pixels.Length / divideResolutionBy)).ToString() + " " + (mainCam.transform.rotation).ToString();
    }

    //TODO: Look at trends of darknessScores, locate spikes and tell listener when someone slaps their face
    //Mean square error? Change from frame to frame?
}