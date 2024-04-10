/** 
 * @file Shooting.cs
 * @brief This script handles shooting functionality for a tank in Unity.
 */
using System.Net;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Rigidbody tankShell;
    public Transform firingLocation;
    public LineRenderer m_lineRenderer;
    public float launchForce = 1.0f;
    public float maxLaunchForce = 30.0f;
    public float minLaunchForce = 2.0f;
    [SerializeField]
    [Range(10, 100)]
    private int m_linePoints = 25;
    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float m_timeBetweenPoints = 0.1f;

    private bool m_canFire = true;
    private bool m_launched;
    private float m_timer = 0f;
    private float m_currentLaunchForce = 0.0f;
    private Vector3 m_velocity;
    



    private void Update()
    {
        if (m_canFire) 
        {
            // check if space has been pressed once
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_launched = false;
            }
            // check if space is continously held down.
            else if (Input.GetKey(KeyCode.Space) && !m_launched)
            {
                Debug.Log("Holding space");
                DrawProjectileArc();
            }
            else if (Input.GetKeyUp(KeyCode.Space) && !m_launched) 
            {
                Fire();
            }
            
        }
    }

    /** 
     * @brief Fires the tank shell with appropriate force and direction.
     */
    private void Fire()
    {
        m_launched = true;

        // Calculate the force required to launch the tank shell
        float force = CalculateForce(firingLocation.eulerAngles.x);

        // Instantiate the tank shell Rigidbody
        Rigidbody shellInstance = Instantiate(tankShell, firingLocation.position, firingLocation.rotation) as Rigidbody;

        m_velocity = firingLocation.forward * force;

        // Apply the calculated force to the tank shell Rigidbody
        shellInstance.velocity = m_velocity;
    }

    /** 
     * @brief Calculates the force required to launch the tank shell based on maxRange and firing angle.
     * @param maxRange Maximum range of the tank shell.
     * @param angle Firing angle of the tank.
     * @return The force required to launch the tank shell.
     */
    private float CalculateForce(float angle)
    {
        // Calculate the maximum range based on the firing angle
        float range = 0f;
        float g = Physics.gravity.magnitude;

        // Calculate the force required for the calculated 
        float force = Mathf.Sqrt((range * g) / (2f * Mathf.Cos(angle) * Mathf.Sin(360 - angle)));

        return force;
    }

    private void DrawProjectileArc() 
    {
        m_lineRenderer.enabled = true;
        m_lineRenderer.positionCount = Mathf.CeilToInt(m_linePoints / m_timeBetweenPoints) + 1;

        Vector3 startPos = firingLocation.position;
        Vector3 startVelocity = m_velocity / tankShell.mass;

        int i = 0;
        m_lineRenderer.SetPosition(i, startPos);
        for (float time = 0; time < m_linePoints; time += m_timeBetweenPoints)
        {
            i++;
            Vector3 point = startPos + time * startVelocity;
            point.y = startPos.y + startVelocity.y * time + (Physics.gravity.y * time * time) / 2f;

            m_lineRenderer.SetPosition(i, point);
        }
    }
}
