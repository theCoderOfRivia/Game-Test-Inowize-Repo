using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("References"), Space(5)]
    [SerializeField] EnemyData enemyData;

    Enemy enemy;
    Player player;
    NavMeshAgent agent;

    float enemyDamage;
    float meleeRadius;
    float attackCooldown;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        enemy = gameObject.GetComponent<Enemy>();

        enemyDamage = enemyData.damage;
        attackCooldown = enemyData.attackCooldown;
        meleeRadius = enemyData.meleeRadius;
    }

    void Update()
    {
        if (enemy.currentState == EnemyStates.Attack)
        {
            Attack();
        }
    }

    void Attack()
    {
        // Checks if the player is in the enemy's melee range so it can deal damage 
        float distance = Vector3.Distance(transform.position, player.gameObject.transform.position);
        attackCooldown -= Time.deltaTime;
        if (distance < meleeRadius)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            if (attackCooldown <= 0.0f)
            {
                player.TakeDamage(enemyDamage);
                attackCooldown = enemyData.attackCooldown;
            }
        }
        else
        {
            enemy.currentState = EnemyStates.Chase;
            attackCooldown = -1;
            agent.isStopped = false;
        }
    }
}
