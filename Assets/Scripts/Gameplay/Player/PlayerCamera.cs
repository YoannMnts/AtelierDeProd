using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1f)]
    private float zoomSpeed;
    
    [SerializeField]
    private float minZoom;
    
    [SerializeField]
    private float maxZoom;
    
    [SerializeField]
    private CinemachinePositionComposer positionComposer;
    
    private float velocity;
    private float smoothTime;
    

    private void OnValidate()
    {
        positionComposer.CameraDistance = minZoom;
    }

    private void OnEnable()
    {
        PlayerController.Instance.PlayerControls.Zoom += HandleZoom;
    }

    private void OnDisable()
    {
        PlayerController.Instance.PlayerControls.Zoom -= HandleZoom;
    }

    private void HandleZoom(Vector2 vector2)
    {
        var zoom = Mathf.Clamp(positionComposer.CameraDistance - vector2.y, minZoom, maxZoom);
        positionComposer.CameraDistance = zoom;
    }
}