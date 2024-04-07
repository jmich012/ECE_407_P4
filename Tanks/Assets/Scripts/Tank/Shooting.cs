using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Rigidbody tankShell;
    public Transform firingLocation;
    public float maxRange = 50f; // Defaulted to 50m

    private bool isPlayer = false;
    private bool canFire = true;
    private float timer = 0f;

    private void Awake()
    {
        isPlayer = gameObject.CompareTag("Player");
    }

    void Update()
    {
        if (isPlayer && canFire)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
                canFire = false; // Disable shooting until reload time is over
                timer = maxRange / 10f; // Just a simple conversion for the timer (you might need to adjust this)
            }
        }

        if (!canFire)
        {
            // Update the timer
            timer -= Time.deltaTime;

            // If the timer has reached or passed zero, enable shooting again
            if (timer <= 0f)
            {
                canFire = true;
            }
        }
    }

    private void Fire()
    {

        // Instantiate the tankShell Rigidbody
        Rigidbody shellInstance = Instantiate(tankShell, firingLocation.position, firingLocation.rotation) as Rigidbody;

        // Apply the calculated force to the tankShell Rigidbody
        shellInstance.velocity = firingLocation.forward * CalculateForce(maxRange);
    }

    // Function to calculate force based on maxRange
    private float CalculateForce(float range)
    {
        float angleRadians = firingLocation.eulerAngles.x * Mathf.Deg2Rad; // Convert angle to radians
        Debug.Log("Angle (Radians): " + angleRadians);

        float g = Physics.gravity.magnitude;
        Debug.Log("Gravity: " + g);

        float force = Mathf.Sqrt((range * g) / Mathf.Sin(2 * angleRadians));
        Debug.Log("Force: " + force);

        return force;
    }


}
