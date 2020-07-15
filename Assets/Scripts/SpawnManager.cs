using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private GameObject[] powerups;
    private bool stopSpawning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3);

        while(!stopSpawning)
        {
            Vector3 spawnLocation = new Vector3(Random.Range(-9.5f, 9.5f), 7, 0);
            GameObject newEnemy = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;

            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3);
        
        while(!stopSpawning)
        {
            int randomPowerup = Random.Range(0, 3);
            Vector3 spawnLocation = new Vector3(Random.Range(-9, 9), 8, 0);
            Instantiate(powerups[randomPowerup], spawnLocation, Quaternion.identity);
            
            yield return new WaitForSeconds(Random.Range(7, 12));
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }
}
