using System.Collections;
using UnityEngine;

/**
 * @brief Controls the behavior of an enemy's attack mechanism.
 */
public class EnemyAttack : MonoBehaviour
{
    /**
     * @brief The radius within which the enemy can detect the player.
     */
    public float radius;

    /**
     * @brief The angle of the enemy's field of view.
     */
    [Range(0, 360)]
    public float angle;

    /**
     * @brief The speed at which the enemy scans its surroundings.
     */
    public float scanSpeed;

    /**
     * @brief The velocity of the bullets fired by the enemy.
     */
    public float bulletVelocity;

    /**
     * @brief The rate of fire for the enemy.
     */
    public float fireRate = 10f;

    /**
     * @brief Reference to the player GameObject.
     */
    public GameObject playerRef;

    /**
     * @brief The origin point from which the enemy performs its scans.
     */
    public Transform origin;

    /**
     * @brief The location from which bullets are fired.
     */
    public Transform firingLocation;

    /**
     * @brief The rigidbody of the bullet.
     */
    public Rigidbody bulletRigidBody;

    /**
     * @brief Reference to the particle system for muzzle flash effects.
     */
    public ParticleSystem m_ParticleSystem;

    /**
     * @brief Reference to the audio source for firing sounds.
     */
    public AudioSource m_AudioSource;

    /**
     * @brief The layer mask for detecting the player.
     */
    public LayerMask playerMask;

    /**
     * @brief The layer mask for detecting obstructions.
     */
    public LayerMask obstructionMask;

    /**
     * @brief Indicates whether the player is within the enemy's field of view.
     */
    public bool canSeePlayer;

    private Rigidbody bullet;
    private float counter = 0f;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Scan());
    }

    private void Update()
    {
        if (!canSeePlayer)
            origin.Rotate(new Vector3(0f, scanSpeed * Time.deltaTime, 0f), Space.Self);

        if (counter > 0)
            counter -= Time.deltaTime;
    }

    /**
     * @brief Performs a continuous scan for the player within the enemy's field of view.
     * @returns An IEnumerator used for yielding.
     */
    private IEnumerator Scan()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FOVCheck();
        }
    }

    /**
     * @brief Checks if the player is within the enemy's field of view and fires if detected.
     */
    private void FOVCheck()
    {
        Collider[] rangeCheck = Physics.OverlapSphere(origin.position, radius, playerMask);

        if (rangeCheck.Length != 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector3 direction = (target.position - origin.position).normalized;

            if (Vector3.Angle(origin.forward, direction) < angle / 2)
            {
                float distanceToPlayer = Vector3.Distance(origin.position, target.position);

                if (!Physics.Raycast(origin.position, direction, distanceToPlayer, obstructionMask))
                {
                    canSeePlayer = true;
                    if (counter <= 0)
                    {
                        Fire();
                        counter = 1f / fireRate;
                    }
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
            canSeePlayer = false;

    }

    /**
     * @brief Fires a bullet at the player's position.
     */
    private void Fire()
    {
        Vector3 firingDirection = playerRef.transform.position - firingLocation.position;

        bullet = Instantiate(bulletRigidBody, firingLocation.position, firingLocation.rotation) as Rigidbody;

        bullet.velocity = bulletVelocity * firingDirection;

        if (m_AudioSource != null)
        {
            m_AudioSource.enabled = true;
            m_ParticleSystem.Play();
            m_AudioSource.Play();
            m_AudioSource.enabled = false;
        }

        Destroy(bullet, 0.5f);
    }
}
