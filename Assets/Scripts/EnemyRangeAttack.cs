using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangeAttack : MonoBehaviour
{
    [Header("References"), Space(5)]
    [SerializeField] EnemyData enemyData;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform posBulletSpawn;
    [SerializeField] Transform gun;

    Enemy enemy;
    Player player;
    NavMeshAgent agent;

    float rangeRadius;
    float attackCooldown;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        agent = gameObject.GetComponent<NavMeshAgent>();

        enemy = gameObject.GetComponent<Enemy>();

        attackCooldown = enemyData.attackCooldown;
        rangeRadius = enemyData.rangeRadius;
    }

    void Update()
    {
        if (enemy.currentState == EnemyStates.Attack)
        {
            Attack();
            RotateGun();
        }
    }

    void Attack()
    {
        // Checks if the player is in the enemy's range so it can shoot
        float distance = Vector3.Distance(transform.position, player.gameObject.transform.position);
        attackCooldown -= Time.deltaTime;
        if (distance < rangeRadius)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            if (attackCooldown < 0.0f)
            {
                Instantiate(bullet, posBulletSpawn.position, Quaternion.LookRotation(posBulletSpawn.forward));
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

    void RotateGun()
    {
        gun.LookAt(player.transform.position);
    }
}
