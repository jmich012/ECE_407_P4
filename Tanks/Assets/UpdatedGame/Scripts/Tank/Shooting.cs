using UnityEngine;

/** 
 * @file Shooting.cs
 * @brief This script handles shooting functionality for a tank in Unity.
 */
public class Shooting : MonoBehaviour
{
    /**
     * @brief The prefab of the tank shell to be fired.
     */
    public Rigidbody tankShell;

    /**
     * @brief The position where the tank shell is fired from.
     */
    public Transform firingLocation;

    /**
     * @brief The line renderer component for drawing projectile trajectory.
     */
    public LineRenderer m_lineRenderer;

    /**
     * @brief The time between each point on the projectile trajectory line.
     */
    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float m_timeBetweenPoints = 0.1f;

    /**
     * @brief The number of points to draw on the projectile trajectory line.
     */
    [SerializeField]
    [Range(10, 100)]
    private int m_linePoints = 25;

    /**
     * @brief The time it takes to reload after firing.
     */
    public float reloadTime = 1.5f;

    /**
     * @brief The initial force applied to the tank shell when fired.
     */
    public float launchForce = 1.0f;

    /**
     * @brief The maximum force that can be applied to the tank shell.
     */
    public float maxLaunchForce = 30.0f;

    /**
     * @brief The minimum force that can be applied to the tank shell.
     */
    public float minLaunchForce = 2.0f;

    private bool m_canFire = true;
    private bool m_launched;
    private float m_timer = 0f;
    private float m_currentLaunchForce = 0.0f;
    private Rigidbody m_shellInstance;

    /**
     * @brief Updates the shooting behavior based on user input.
     */
    private void Update()
    {
        if (m_canFire)
        {
            // check if space has been pressed once
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_launched = false;
                m_currentLaunchForce = minLaunchForce;
            }
            // check if space is continuously held down.
            else if (Input.GetKey(KeyCode.Space) && !m_launched)
            {
                if (m_currentLaunchForce >= maxLaunchForce)
                {
                    m_currentLaunchForce = maxLaunchForce;
                }
                else
                {
                    m_currentLaunchForce += 0.1f;
                }
                DrawProjectileArc();
            }
            else if (Input.GetKeyUp(KeyCode.Space) && !m_launched)
            {
                Fire();
                m_canFire = false;
            }
        }
        else
        {
            m_lineRenderer.enabled = false;
        }

        if (m_timer < reloadTime)
        {
            m_timer += Time.deltaTime;
        }
        else if (m_timer >= reloadTime)
        {
            m_canFire = true;
            m_timer = 0f;
        }
    }

    /** 
     * @brief Fires the tank shell with appropriate force and direction.
     */
    private void Fire()
    {
        m_launched = true;

        // Calculate the force required to launch the tank shell
        float force = m_currentLaunchForce;

        // Instantiate the tank shell Rigidbody
        m_shellInstance = Instantiate(tankShell, firingLocation.position, firingLocation.rotation) as Rigidbody;

        // Apply the calculated force to the tank shell Rigidbody
        m_shellInstance.velocity = force * firingLocation.forward;
    }

    /**
     * @brief Draws the projectile trajectory arc using a line renderer.
     */
    private void DrawProjectileArc()
    {
        m_lineRenderer.enabled = true;
        m_lineRenderer.positionCount = Mathf.CeilToInt(m_linePoints / m_timeBetweenPoints) + 1;

        Vector3 startPos = firingLocation.position;
        Vector3 startVelocity = m_currentLaunchForce / tankShell.mass * firingLocation.forward;

        int i = 0;
        m_lineRenderer.SetPosition(i, startPos);

        bool hasCollided = false; // Flag to track collision

        for (float time = 0; time < m_linePoints; time += m_timeBetweenPoints)
        {
            i++;
            Vector3 point = startPos + time * startVelocity;
            point.y = startPos.y + startVelocity.y * time + (Physics.gravity.y * time * time) / 2f;

            // Check for collision using a raycast
            RaycastHit hit;
            if (Physics.Raycast(startPos, point - startPos, out hit, (point - startPos).magnitude))
            {
                // If hit something, stop drawing arc
                hasCollided = true;
                point = hit.point;
                break;
            }

            m_lineRenderer.SetPosition(i, point);
        }

        // If no collision, continue drawing the arc until its end
        if (!hasCollided)
        {
            Vector3 endPoint = startPos + m_linePoints * m_timeBetweenPoints * startVelocity;
            m_lineRenderer.SetPosition(i, endPoint);
        }
    }
}
