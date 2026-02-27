using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private void Update()
    {
        UpdateCamera();
    }

    public void UpdateCamera()
    {
        this.gameObject.transform.position = PlayerController.Instance.gameObject.transform.position;
    }
}