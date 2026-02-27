using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1f)]
    private float zoomSpeed;
    
    [SerializeField]
    private float minZoom;
    
    [SerializeField]
    private float maxZoom;
    
    private float velocity;
    private float smoothTime;
    
    [SerializeField]
    private Camera cam;

    private void OnEnable()
    {
        PlayerController.Instance.PlayerControls.Zoom += HandleZoom;
    }

    private void OnDisable()
    {
        PlayerController.Instance.PlayerControls.Zoom -= HandleZoom;
    }
    

    private void Update()
    {
        UpdateCamera();
    }

    private void HandleZoom(Vector2 vector2)
    {
        var newZoomX = Mathf.Clamp(cam.sensorSize.x - vector2.x, minZoom, maxZoom);
        var newZoomY = Mathf.Clamp(cam.sensorSize.y - vector2.y, minZoom, maxZoom);
        var newVector = new Vector2(newZoomX, newZoomY);
        cam.sensorSize = newVector;
    }

    public void UpdateCamera()
    {
        gameObject.transform.position = PlayerController.Instance.gameObject.transform.position;
    }
}