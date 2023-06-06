using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public RoomManagment manager;
    public GameObject enemy;

    private void spawnEnemy()
    {
        GameObject enemyRef = Instantiate(enemy, transform.position, Quaternion.identity);
        enemyRef.GetComponent<Enemy>().manager = manager;
    }

    public void OnParticleSystemStopped()
    {
        //spawns enemy and destroys its self 
        spawnEnemy();
        Destroy(gameObject);
    }


}
