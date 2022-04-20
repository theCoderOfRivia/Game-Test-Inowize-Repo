using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings"), Space(5)]
    [Tooltip("Damage dealt by this projectile.")]
    [SerializeField] float projectileDamage = 2f;
    [Tooltip("The speed at which this projectile travels")]
    [SerializeField] float projectileSpeed = 1000f;
    [Tooltip("The force of the projectile that pushes the enemy back.")]
    [SerializeField] float hitForce = 1f;
    [Tooltip("How much time in seconds it takes for the enemy to recover from the hit.")]
    [SerializeField] float slidingAfterHit = 0.2f;

    private Rigidbody projectileBody;

    bool alreadyHit;


    private void Awake()
    {
        projectileBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Projectile starts travelling
        projectileBody.AddForce(transform.forward * projectileSpeed);
        Destroy(gameObject, 10f);
    }


    private void OnCollisionEnter(Collision enemy)
    {
        // Checks if we hit a Zombie and makes sure to not interact with it again when the bullet is on the ground
        if (enemy.collider.CompareTag("Zombie") && alreadyHit == false)
        {
            // Enemy takes damage
            Enemy enemyComponent = enemy.collider.GetComponent<Enemy>();
            enemyComponent.TakeDamage(projectileDamage);

            if (enemyComponent.currentEnemyHealth > 0)
            {
                // Enemy is pushed back with a tween
                enemyComponent.transform.DOMove(enemyComponent.transform.position + (-enemyComponent.transform.forward * hitForce), slidingAfterHit);
            }
        }
        
        alreadyHit = true;
    }

    private void OnTriggerEnter(Collider enemy)
    {
        if (enemy.CompareTag("Player"))
        {
            // Enemy takes damage
            Player enemyComponent = enemy.GetComponent<Player>();
            enemyComponent.TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
    }
}
