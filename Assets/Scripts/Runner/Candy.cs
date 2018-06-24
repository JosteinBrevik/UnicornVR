﻿using UnityEngine;

public class Candy : MonoBehaviour
{
    //candy found in https://www.assetstore.unity3d.com/en/#!/content/12512

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed);
    }

    public int ScorePoints = 100;
    public float rotateSpeed = 50f;
}
