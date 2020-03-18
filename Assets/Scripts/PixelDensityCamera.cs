using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PixelDensityCamera : MonoBehaviour
{
    [SerializeField] public int targetWidth = 640;
    [SerializeField] public int pixelsToUnits = 100;
    Camera camera;
    private void Start()
    {
        camera = GameObject.FindObjectOfType<Camera>();
    }

    void Update()
    {
        int height = Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);
        camera.orthographicSize = height / pixelsToUnits / 2;   
    }
}
