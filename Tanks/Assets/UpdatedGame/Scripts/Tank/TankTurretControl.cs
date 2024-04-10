using UnityEngine;

public class TankTurretControl : MonoBehaviour
{

    public float turretRotationSpeed = 50.0f;
    public float cannonRoatationSpeed = 100.0f;
    public Transform cannon;
    public Transform FiringPosition;


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

        if (cannon.localEulerAngles.x >= 270f || cannon.localEulerAngles.x <= maxVerticalRotation)
        {
            isUp = true;
        }
        else 
        {
            isUp = false;
        }
        //FiringPosition.rotation = cannon.rotation;
    }

    void RotateObject(Transform objTransform, int direction, Vector3 axis, float rotationSpeed)
    {
        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.Euler(objTransform.rotation.eulerAngles + axis * direction * rotationSpeed * Time.deltaTime);

        objTransform.rotation = targetRotation;

        if (axis == Vector3.right) 
        {
            if (direction == -1 && objTransform.rotation.x > -90.0f && objTransform.rotation.x <= -89.5f)
            {
                objTransform.rotation = Quaternion.Euler(-90f, objTransform.rotation.y, objTransform.rotation.z);
            }
        }
    }
}
