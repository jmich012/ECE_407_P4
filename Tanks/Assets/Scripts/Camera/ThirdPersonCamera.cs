using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform tank; // The target object to follow
    public Vector3 offset = new Vector3(0f,7f, -15f); // The offset from the target position
    public float damping = 5f; // The smoothness of the camera movement

    void LateUpdate()
    {
        if (tank != null)
        {
            // Calculate the desired camera position based on the tank's position and offset
            Vector3 desiredPosition = tank.position + offset;

            // Smoothly move the camera towards the desired position
            //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, damping * Time.deltaTime);
            //transform.position = smoothedPosition;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, damping * Time.deltaTime);

            // Make the camera look at the tank
            transform.LookAt(tank);
        }
    }
}
