using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform tankCamera;
    public Transform mapCamera;
    public float positionDamping = 0.02f;
    public float rotationDamping = 0.01f;


    private bool playerCamera = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            playerCamera = !playerCamera;
        }
    }
    private void FixedUpdate()
    {
        if (tankCamera != null && playerCamera)
        {
            transform.position = Vector3.Lerp(transform.position, tankCamera.position, positionDamping);
            transform.rotation = Quaternion.Lerp(transform.rotation, tankCamera.rotation, rotationDamping);
        }
        else if (mapCamera != null && !playerCamera) 
        {
            transform.position = mapCamera.position;
            transform.rotation = mapCamera.rotation;
        }
    }
}
