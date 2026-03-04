using System;
using System.Collections.Generic;
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
    private LayerMask wallLayerMask;
    
    [SerializeField]
    private Material wallMaterial;
    
    [SerializeField]
    private int seeTroughDistance = 1000;

    [SerializeField]
    private CinemachinePositionComposer positionComposer;
    
    [SerializeField]
    private Camera cam;
    
    
    private float velocity;
    private float smoothTime;
    
    private int posID = Shader.PropertyToID("_Position");
    private int sizeID = Shader.PropertyToID("Size");
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
//        Gizmos.DrawLine(cam.transform.position, PlayerController.Instance.transform.position);
    }

    private void FixedUpdate()
    {
        var dir = (cam.transform.position - transform.position).normalized;
        var ray = new Ray(transform.position, dir);

        var value = Physics.Raycast(ray, out RaycastHit hit, seeTroughDistance, wallLayerMask) ? 1 : 0;
        wallMaterial.SetFloat(sizeID, value);
        
        var view = cam.WorldToViewportPoint(transform.position);
        wallMaterial.SetVector(posID, view);
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