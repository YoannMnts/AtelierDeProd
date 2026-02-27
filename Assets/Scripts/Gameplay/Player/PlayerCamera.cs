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
    private GameObject cam;

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
        Debug.Log($"Zoom: {vector2.y}");
        var scroll = vector2.y * zoomSpeed;
        var newPosition = new Vector3(cam.transform.position.x, cam.transform.position.y, scroll);
        newPosition.z = Mathf.Clamp(newPosition.z, minZoom, maxZoom);
        cam.transform.position +=  newPosition;
    }

    public void UpdateCamera()
    {
        gameObject.transform.position = PlayerController.Instance.gameObject.transform.position;
    }
}