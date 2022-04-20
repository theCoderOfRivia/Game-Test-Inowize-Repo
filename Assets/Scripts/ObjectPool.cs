using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [Header("References"), Space(5)]
    [SerializeField] GameObject[] pooledEnemies;
    [SerializeField] Transform[] spawnPoints;

    List<Transform> availableSpawnPoints = new List<Transform>();
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public IEnumerator SpawnAnotherEnemy()
    {
        CheckAvailableSpawnPoints();

        for(int i = 0; i < pooledEnemies.Length; i++)
        {
            // We take the enemy we just killed and respawn it on another random spawn point
            if (!pooledEnemies[i].activeSelf)
            {
                int index = Random.Range(0, availableSpawnPoints.Count);

                availableSpawnPoints[index].gameObject.SetActive(true);
                pooledEnemies[i].transform.position = availableSpawnPoints[index].position;

                Enemy enemyComponent = pooledEnemies[i].GetComponent<Enemy>();
                // Makes the previous spawn point of the killed enemy available
                enemyComponent.spawnPoint.gameObject.SetActive(false);
                enemyComponent.spawnPoint = availableSpawnPoints[index];
                enemyComponent.patrolStartPos = availableSpawnPoints[index].GetChild(0).gameObject.transform;
                enemyComponent.patrolEndPos = availableSpawnPoints[index].GetChild(1).gameObject.transform;

                yield return new WaitForEndOfFrame();
                pooledEnemies[i].SetActive(true);

                break;
            }
        }
    }

    void CheckAvailableSpawnPoints()
    {
        availableSpawnPoints.Clear();

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (!spawnPoint.gameObject.activeSelf)
            {
                availableSpawnPoints.Add(spawnPoint);
            }
        }
    }
}
