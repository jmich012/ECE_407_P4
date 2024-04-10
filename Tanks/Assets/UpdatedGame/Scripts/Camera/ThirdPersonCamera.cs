/** 
 * @file ThirdPersonCamera.cs
 * @brief Defines the behavior of a third-person camera that can switch between two camera views.
 */

using UnityEngine;

/** 
 * @class ThirdPersonCamera
 * @brief Controls the behavior of a third-person camera.
 */
public class ThirdPersonCamera : MonoBehaviour
{
    /** 
     * @brief The transform of the camera following the tank.
     * @details This transform determines the position and orientation of the camera when following the tank.
     */
    public Transform tankCamera;

    /** 
     * @brief The transform of the camera showing the map.
     * @details This transform determines the position and orientation of the camera when showing the map.
     */
    public Transform mapCamera;

    /** 
     * @brief The damping factor for camera position interpolation.
     * @details This factor determines the speed at which the camera's position adjusts to follow the target.
     */
    public float positionDamping = 0.02f;

    /** 
     * @brief The damping factor for camera rotation interpolation.
     * @details This factor determines the speed at which the camera's orientation adjusts to follow the target.
     */
    public float rotationDamping = 0.01f;

    private bool playerCamera = true;

    /** 
     * @brief Updates the camera state.
     * @details This method checks for input to toggle between the player and map cameras.
     */
    private void Update()
    {
        // Toggle camera when 'F' key is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerCamera = !playerCamera;
        }
    }

    /** 
     * @brief Updates camera position and rotation based on the chosen camera mode.
     * @details This method interpolates the camera's position and rotation to follow the tank or show the map,
     * depending on the currently selected camera mode.
     */
    private void FixedUpdate()
    {
        if (tankCamera != null && playerCamera)
        {
            // Interpolate camera position and rotation towards tank camera
            transform.position = Vector3.Lerp(transform.position, tankCamera.position, positionDamping);
            transform.rotation = Quaternion.Lerp(transform.rotation, tankCamera.rotation, rotationDamping);
        }
        else if (mapCamera != null && !playerCamera)
        {
            // Set camera position and rotation to map camera
            transform.position = mapCamera.position;
            transform.rotation = mapCamera.rotation;
        }
    }
}
