using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public RoomManagment manager;
    public GameObject enemy;


    private void spawnEnemy()
    {
        int spawnerTier = 2;
        int enemyCount = 1;
        //if zombie Spawner
        if (Random.value <= 0.5f)
        {
            spawnerTier = 1;
            enemyCount = 3;
            //to fix room opening before all dead
            manager.enemysDead += 1 - 3;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyRef = Instantiate(enemy, transform.position, Quaternion.identity);
            enemyRef.GetComponent<Enemy>().manager = manager;
            enemyRef.GetComponent<Enemy>().enemyTier = spawnerTier;
        }

        // plays spawn Sound
        manager.playSoundEnemySpawn();
    }

    public void OnParticleSystemStopped()
    {
        //spawns enemy and destroys its self 
        spawnEnemy();
        Destroy(gameObject);
    }


}
