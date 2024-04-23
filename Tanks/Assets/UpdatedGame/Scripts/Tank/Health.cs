using UnityEngine;

public class Health : MonoBehaviour
{ 
    public int health;
    public GameObject m_explosion;
    public bool isDead { get; private set; }

    private ParticleSystem m_explosionParticles;
    private AudioSource m_ExplosionAudio;
    

    private void Awake()
    {
        m_explosionParticles = Instantiate (m_explosion).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_explosionParticles.GetComponent<AudioSource>();
        m_explosionParticles.gameObject.SetActive(false);
        isDead = false;
    }

    private void Update()
    {
        if (gameObject.CompareTag("Player") && isDead)
            Manager.Instance.GameOver(2f);

    }
    private void OnTriggerEnter(Collider projectile)
    {
        if (projectile.gameObject.CompareTag("Projectile"))
        {
            DecreaseHealth();
            Destroy(projectile.gameObject);
        }
    }

    private void DecreaseHealth()
    {
        if (health > 0)
        {
            health--;
        }
        else 
        {
            Explode();
            Destroy(gameObject,2.5f);
            isDead = true;
        }
    }

    private void Explode()
    {
        m_explosionParticles.transform.position = transform.position;
        m_explosionParticles.gameObject.SetActive (true);

        m_explosionParticles.Play();
        m_ExplosionAudio.Play();
    }
}
