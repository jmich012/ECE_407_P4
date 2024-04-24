using UnityEngine;

/**
 * @brief Controls the health and destruction behavior of game objects.
 */
public class Health : MonoBehaviour
{
    /**
     * @brief The initial health of the object.
     */
    public int health;

    /**
     * @brief Reference to the explosion prefab.
     */
    public GameObject m_explosion;

    /**
     * @brief Indicates whether the object is dead.
     */
    public bool isDead { get; private set; }

    private ParticleSystem m_explosionParticles;
    private AudioSource m_ExplosionAudio;


    private void Awake()
    {
        m_explosionParticles = Instantiate(m_explosion).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_explosionParticles.GetComponent<AudioSource>();
        m_explosionParticles.gameObject.SetActive(false);
        isDead = false;
    }

    private void Update()
    {
        if (gameObject.CompareTag("Player") && isDead)
            Manager.Instance.GameOver(2f);

    }

    /**
     * @brief Detects collisions with projectiles and decreases health accordingly.
     * @param projectile The collider of the projectile.
     */
    private void OnTriggerEnter(Collider projectile)
    {
        if (projectile.gameObject.CompareTag("Projectile"))
        {
            DecreaseHealth();
            Destroy(projectile.gameObject);
        }
    }

    /**
     * @brief Decreases the object's health and triggers destruction if health reaches zero.
     */
    private void DecreaseHealth()
    {
        if (health > 0)
        {
            health--;
        }
        else
        {
            Explode();
            Destroy(gameObject, 2.5f);
            isDead = true;
        }
    }

    /**
     * @brief Triggers the explosion effect when the object is destroyed.
     */
    private void Explode()
    {
        m_explosionParticles.transform.position = transform.position;
        m_explosionParticles.gameObject.SetActive(true);

        m_explosionParticles.Play();
        m_ExplosionAudio.Play();
    }
}
