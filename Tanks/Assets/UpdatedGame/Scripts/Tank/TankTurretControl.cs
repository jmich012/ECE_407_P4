using UnityEngine;

/**
 * @brief Controls the rotation of a tank's turret and cannon.
 */
public class TankTurretControl : MonoBehaviour
{
    /**
     * @brief The speed at which the turret rotates.
     */
    public float turretRotationSpeed = 50.0f;

    /**
     * @brief The speed at which the cannon rotates.
     */
    public float cannonRoatationSpeed = 100.0f;

    /**
     * @brief Reference to the cannon transform.
     */
    public Transform cannon;

    /**
     * @brief Reference to the firing position transform.
     */
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

    /**
     * @brief Rotates the specified object in the given direction along the specified axis.
     * @param objTransform The transform of the object to rotate.
     * @param direction The direction of rotation (-1 for counter-clockwise, 1 for clockwise).
     * @param axis The axis around which to rotate the object.
     * @param rotationSpeed The speed of rotation.
     */
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
