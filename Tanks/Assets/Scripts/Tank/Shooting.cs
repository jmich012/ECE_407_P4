/** 
 * @file Shooting.cs
 * @brief This script handles shooting functionality for a tank in Unity.
 */

using UnityEngine;

public class Shooting : MonoBehaviour
{
    /** 
     * @brief Rigidbody of the tank shell prefab.
     */
    public Rigidbody tankShell;

    /** 
     * @brief Transform representing the firing location of the tank.
     */
    public Transform firingLocation;

    /** 
     * @brief Maximum range of the tank shell in meters. Defaulted to 50m.
     */
    public float maxRange = 50f;

    public float launchMultiplier = 1.0f;

    private bool isPlayer = false; /**< Indicates if the tank is controlled by the player */
    private bool canFire = true;    /**< Indicates if the tank can currently fire */
    private float timer = 0f;       /**< Timer for controlling the reload time */

    private void Awake()
    {
        // Check if the tank is controlled by the player
        isPlayer = gameObject.CompareTag("Player");
    }

    private void Update()
    {
        // Check if the tank is controlled by the player and can currently fire
        if (isPlayer && canFire)
        {
            // Check if the spacebar is pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Fire the tank shell
                Fire();

                // Disable shooting until reload time is over
                canFire = false;

                // Set the reload timer
                timer = maxRange / 10f; // Just a simple conversion for the timer (you might need to adjust this)
            }
        }

        // Check if the tank is currently reloading
        if (!canFire)
        {
            // Update the reload timer
            timer -= Time.deltaTime;

            // If the timer has reached or passed zero, enable shooting again
            if (timer <= 0f)
            {
                canFire = true;
            }
        }
    }

    /** 
     * @brief Fires the tank shell with appropriate force and direction.
     */
    private void Fire()
    {
        // Calculate the force required to launch the tank shell
        float force = CalculateForce(maxRange, firingLocation.localEulerAngles.x) * launchMultiplier;

        // Instantiate the tank shell Rigidbody
        Rigidbody shellInstance = Instantiate(tankShell, firingLocation.position, firingLocation.rotation) as Rigidbody;

        // Apply the calculated force to the tank shell Rigidbody
        shellInstance.velocity = firingLocation.forward * force;
    }

    /** 
     * @brief Calculates the force required to launch the tank shell based on maxRange and firing angle.
     * @param maxRange Maximum range of the tank shell.
     * @param angle Firing angle of the tank.
     * @return The force required to launch the tank shell.
     */
    private float CalculateForce(float maxRange, float angle)
    {
        // Check if the firing angle is close to zero
        if (Mathf.Approximately(angle, 0f))
        {
            // Handle the case where the firing angle is close to zero
            angle = Mathf.Epsilon; // Set a small offset to avoid division by zero
        }

        // Calculate the force required for the given range and angle
        float g = Physics.gravity.magnitude;
        float force = Mathf.Sqrt((maxRange * g) / (2f * Mathf.Cos(angle) * Mathf.Sin(360 - angle)));

        return force;
    }
}
