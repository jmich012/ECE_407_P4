using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TankTurretControl : MonoBehaviour
{

    public float turretRotationSpeed = 50.0f;
    public float cannonRoatationSpeed = 100.0f;
    public Transform cannon;


    private bool isUp = false;
    private float maxVerticalRotation = -90.0f;

    // Update is called once per frame
    void Update()
    {
        // Rotate turret clockwise when right arrow key is held
        if (Input.GetKey(KeyCode.RightArrow))
        {
            RotateObject(transform, 1, Vector3.up, turretRotationSpeed);
        }

        // Rotate turret counter-clockwise when left arrow key is held
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            RotateObject(transform, -1, Vector3.up, turretRotationSpeed);
        }

        // Rotate cannon upwards when up arrow key is held
        if (Input.GetKey(KeyCode.UpArrow))
        {
            RotateObject(cannon, -1, Vector3.right, cannonRoatationSpeed);
        }

        // Rotate cannon downwards when down arrow key is held
        if (isUp && Input.GetKey(KeyCode.DownArrow))
        {
            RotateObject(cannon, 1, Vector3.right, cannonRoatationSpeed);
        }
    }

    void RotateObject(Transform objTransform, int direction, Vector3 axis, float rotationSpeed)
    {
        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.Euler(objTransform.rotation.eulerAngles + axis * direction * rotationSpeed * Time.deltaTime);

        // Clamp vertical rotation if necessary
        if (axis == Vector3.right && objTransform.localEulerAngles.x >= 0 && objTransform.localEulerAngles.x <= maxVerticalRotation)
        {
            // Allow downwards rotation only if the angle is greater than 0 degrees
            objTransform.rotation = Quaternion.Euler(maxVerticalRotation, objTransform.rotation.eulerAngles.y, objTransform.rotation.eulerAngles.z);
            isUp = true;
        }
        else
        {
            // Rotate freely if not constrained
            objTransform.rotation = targetRotation;
            isUp = false;
        }
    }
}
