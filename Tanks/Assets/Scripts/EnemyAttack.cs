using System.Collections;
using System.Diagnostics.Tracing;
using System.Threading;
using UnityEditor.Purchasing;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;
    public float scanSpeed;
    public float bulletVelocity;
    public float fireRate = 10f;

    public GameObject playerRef;
    public Transform origin;
    public Transform firingLocation;
    public Rigidbody bulletRigidBody;


    public LayerMask playerMask;
    public LayerMask obstructionMask;

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
                    Debug.Log(counter);
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

    private void Fire()
    {
        Vector3 firingDirection = playerRef.transform.position - firingLocation.position;

        bullet = Instantiate(bulletRigidBody, firingLocation.position, firingLocation.rotation ) as Rigidbody;

        bullet.velocity = bulletVelocity * firingDirection;

        Destroy(bullet, 0.5f);
    }
}
