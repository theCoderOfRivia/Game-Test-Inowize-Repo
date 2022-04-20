using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EnemyStates
{
    Patrol,
    Chase,
    Attack
}

public class Enemy : MonoBehaviour
{
    [HideInInspector] public EnemyStates currentState = EnemyStates.Patrol;

    [Header("Enemy Settings"), Space(5)]
    [SerializeField] float aggroRadius = 20f;

    [Header("References"), Space(5)]
    [SerializeField] EnemyData enemyData;
    public Transform patrolStartPos;
    public Transform patrolEndPos;
    public Transform spawnPoint;
    [SerializeField] AudioClip enemyHurt;

    Player player;
    NavMeshAgent agent;

    float defaultAggro;
    float defaultSpeed;
    [HideInInspector] public float currentEnemyHealth;

    Vector3 currentPatrolDestination;

    AudioSource audioSource;

    Image healthBarImage;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        healthBarImage = gameObject.GetComponentInChildren<Image>();
        audioSource = gameObject.GetComponent<AudioSource>();

        defaultAggro = aggroRadius;
        defaultSpeed = agent.speed;

        currentEnemyHealth = enemyData.health;
        healthBarImage.fillAmount = currentEnemyHealth / enemyData.health;

        currentPatrolDestination = patrolStartPos.position;
    }

    private void OnEnable()
    {
        agent.speed = defaultSpeed;
        aggroRadius = defaultAggro;

        currentEnemyHealth = enemyData.health;
        healthBarImage.fillAmount = currentEnemyHealth / enemyData.health;

        currentPatrolDestination = patrolStartPos.position;
        currentState = EnemyStates.Patrol;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyStates.Patrol:
                SearchForPlayer();
                Patrol();
                break;

            case EnemyStates.Chase:
                Chase();
                break;

            case EnemyStates.Attack:
                LookAtPlayer();
                break;

            default:
                break;
        }
    }


    void SearchForPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < aggroRadius)
        {
            currentState = EnemyStates.Chase;
        }
    }

    void Patrol()
    {

        agent.SetDestination(currentPatrolDestination);

        float distance = Vector3.Distance(currentPatrolDestination, transform.position);
        if (distance < 1.5f)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            if (currentPatrolDestination == patrolStartPos.position)
            {
                currentPatrolDestination = patrolEndPos.position;
            }
            else
            {
                currentPatrolDestination = patrolStartPos.position;
            }
        }
        else
        {
            agent.isStopped = false;
        }
    }

    void Chase()
    {

        agent.SetDestination(player.gameObject.transform.position);

        LookAtPlayer();

        float distance = Vector3.Distance(transform.position, player.gameObject.transform.position);
        if(enemyData.enemyType == EnemyData.EnemyType.Melee)
        {
            if (distance < enemyData.meleeRadius)
            {
                currentState = EnemyStates.Attack;
            }
            else if (distance > aggroRadius)
            {
                currentState = EnemyStates.Patrol;
            }
        }
        else
        {
            if (distance < enemyData.rangeRadius)
            {
                currentState = EnemyStates.Attack;
            }
            else if (distance > aggroRadius)
            {
                currentState = EnemyStates.Patrol;
            }
        }
    }


    public void TakeDamage(float amount)
    {
        currentEnemyHealth -= amount;

        // If the enemy is attacked and in Patrol state then get aggressive
        if(currentState == EnemyStates.Patrol)
        {
            float aggroRadiusDefault = aggroRadius;
            float agentSpeedDefault = agent.speed;
            aggroRadius = 1000f;
            agent.speed = 25f;
            StartCoroutine(AggroAndSpeedBackToDefaultOverTime(7f, aggroRadiusDefault, agentSpeedDefault));
        }

        if (currentEnemyHealth <= 0)
        {
            PointsSystem.instance.IncreasePoints(enemyData.enemyType);
            gameObject.SetActive(false);
        }
        else
        {
            audioSource.PlayOneShot(enemyHurt);
            healthBarImage.fillAmount = currentEnemyHealth / enemyData.health;
        }
    }

    private void OnDisable()
    {
        ObjectPool.instance.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(ObjectPool.instance.SpawnAnotherEnemy());
    }


    void LookAtPlayer()
    {
        // Looks at player without rotating on the x axis
        agent.transform.LookAt(player.gameObject.transform.position);
        Vector3 eulerAngles = agent.transform.rotation.eulerAngles;
        eulerAngles = new Vector3(0, eulerAngles.y, eulerAngles.z);
        agent.transform.rotation = Quaternion.Euler(eulerAngles);
    }

    IEnumerator AggroAndSpeedBackToDefaultOverTime(float duration, float targetAggro, float targetSpeed)
    {
        float currentTime = 0;
        float startAggro = aggroRadius;
        float startSpeed = agent.speed;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            aggroRadius = Mathf.Lerp(startAggro, targetAggro, currentTime / duration);
            agent.speed = Mathf.Lerp(startSpeed, targetSpeed, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
