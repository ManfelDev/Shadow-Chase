using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingClouds : MonoBehaviour
{
    [SerializeField] private RawImage backgroundImage;
    [SerializeField] private float scrollSpeed = 0.05f;

    private void Update()
    {
        float offset = Time.time * (-scrollSpeed);
        float x = offset % 1.0f;
        backgroundImage.uvRect = new Rect(x, 0, 1, 1);
    }
}