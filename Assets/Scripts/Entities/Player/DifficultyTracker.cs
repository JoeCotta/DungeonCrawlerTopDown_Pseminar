using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyTracker : MonoBehaviour, IDataPersistence
{
    public float timeInRun;
    static public string difficultyLevel;
    [SerializeField] float interval;
    [SerializeField] GameObject enemyPrefab;
    static public float healthMultiplier;
    static public float dmgMultiplier;
    static public float speedMultiplier = 1;
    static public float bulletSpeedMultiplier = 1;

    void Update()
    {
        if (GameManager.isPaused) return;
        timeInRun += Time.deltaTime;
        UpdateDifficultyLevel();
        if (!GameManager.hardcoreMode) SetDifficultyVariables();
        else HARDCORE_SetDifficultyVariables();

        //If Difficulty hard or higher chance to spawn enemy at random
        if (timeInRun > 1200) ChanceToSpawnEnemy();
    }

    void UpdateDifficultyLevel()
    {
        switch (timeInRun)
        {
            case < 150:
                difficultyLevel = "Very Easy";
                break;
            case < 300:
                difficultyLevel = "Easy";
                break;
            case < 600:
                difficultyLevel = "Normal";
                break;
            case < 900:
                difficultyLevel = "Harder than Normal";
                break;
            case < 1200:
                difficultyLevel = "Hard";
                break;
            case < 1500:
                difficultyLevel = "Harder";
                break;
            case < 1800:
                difficultyLevel = "Very Hard";
                break;
            case < 2100:
                difficultyLevel = "Impossible";
                break;
            default:
                difficultyLevel = "Unbelievable";
                break;
        }
    }

    void SetDifficultyVariables()
    {
        switch (difficultyLevel)
        {
            case "Very Easy":
                healthMultiplier = 0.5f;
                dmgMultiplier = 0.5f;
                bulletSpeedMultiplier = 0.75f;
                break;
            case "Easy":
                healthMultiplier = 0.75f;
                dmgMultiplier = 0.75f;
                bulletSpeedMultiplier = 0.75f;
                break;
            case "Normal":
                healthMultiplier = 1;
                dmgMultiplier = 1;
                bulletSpeedMultiplier = 1f;
                break;
            case "Harder than Normal":
                healthMultiplier = 1.5f;
                dmgMultiplier = 1;
                bulletSpeedMultiplier = 1.25f;
                break;
            case "Hard":
                healthMultiplier = 2;
                dmgMultiplier = 1.5f;
                bulletSpeedMultiplier = 1.25f;
                break;
            case "Harder":
                healthMultiplier = 2;
                dmgMultiplier = 2;
                bulletSpeedMultiplier = 1.5f;
                break;
            case "Very Hard":
                healthMultiplier = 3f;
                dmgMultiplier = 2;
                bulletSpeedMultiplier = 1.75f;
                break;
            case "Impossible":
                healthMultiplier = 5;
                dmgMultiplier = 3;
                bulletSpeedMultiplier = 1.75f;
                break;
            case "Unbelievable":
                healthMultiplier = 5;
                dmgMultiplier = 3;
                bulletSpeedMultiplier = 2f;
                speedMultiplier = 1.5f;
                break;
            default:
                healthMultiplier = 5;
                dmgMultiplier = 3;
                bulletSpeedMultiplier = 2f;
                speedMultiplier = 1.5f;
                break;
        }
    }

    void HARDCORE_SetDifficultyVariables()
    {
        switch (difficultyLevel)
        {
            case "Very Easy":
                healthMultiplier = 1f;
                dmgMultiplier = 1f;
                bulletSpeedMultiplier = 1.5f;
                speedMultiplier = 1.1f;
                break;
            case "Easy":
                healthMultiplier = 1.25f;
                dmgMultiplier = 1.25f;
                bulletSpeedMultiplier = 2f;
                speedMultiplier = 1.1f;
                break;
            case "Normal":
                healthMultiplier = 2f;
                dmgMultiplier = 1.5f;
                bulletSpeedMultiplier = 2f;
                speedMultiplier = 1.25f;
                break;
            case "Harder than Normal":
                healthMultiplier = 2f;
                dmgMultiplier = 2f;
                bulletSpeedMultiplier = 2.5f;
                speedMultiplier = 1.25f;
                break;
            case "Hard":
                healthMultiplier = 2.5f;
                dmgMultiplier = 2.5f;
                bulletSpeedMultiplier = 3f;
                speedMultiplier = 1.4f;
                break;
            case "Harder":
                healthMultiplier = 3.5f;
                dmgMultiplier = 2.5f;
                bulletSpeedMultiplier = 3f;
                speedMultiplier = 1.4f;
                break;
            case "Very Hard":
                healthMultiplier = 5f;
                dmgMultiplier = 3.5f;
                bulletSpeedMultiplier = 3f;
                speedMultiplier = 1.5f;
                break;
            case "Impossible":
                healthMultiplier = 8f;
                dmgMultiplier = 4f;
                bulletSpeedMultiplier = 3f;
                speedMultiplier = 1.6f;
                break;
            case "Unbelievable":
                healthMultiplier = 10;
                dmgMultiplier = 6;
                bulletSpeedMultiplier = 4f;
                speedMultiplier = 1.75f;
                break;
            default:
                healthMultiplier = 10;
                dmgMultiplier = 6;
                bulletSpeedMultiplier = 4f;
                speedMultiplier = 1.75f;
                break;
        }
    }

    void ChanceToSpawnEnemy()
    {
        interval += Time.deltaTime;
        if(interval > 150)
        {
            interval = 0;
            if(Random.value < 0.8f)
            {
                RaycastHit2D hit;
                switch (Random.value)
                {
                    case < 0.25f:
                        hit = Physics2D.Raycast(transform.position, Vector2.up, 10, LayerMask.GetMask("Map"));
                        if (hit)
                        {
                            Debug.Log(hit);
                            Instantiate(enemyPrefab, hit.point + Vector2.down, Quaternion.Euler(0, 0, 0));
                        }
                        else Instantiate(enemyPrefab, transform.position + Vector3.up * 10, Quaternion.Euler(0, 0, 0));
                        break;
                    case < 0.5f:
                        hit = Physics2D.Raycast(transform.position, Vector2.left, 10, LayerMask.GetMask("Map"));
                        if (hit)
                        {
                            Debug.Log(hit.point);
                            Instantiate(enemyPrefab, hit.point + Vector2.right, Quaternion.Euler(0, 0, 0));
                        }
                        else Instantiate(enemyPrefab, transform.position + Vector3.left * 10, Quaternion.Euler(0, 0, 0));
                        break;
                    case < 0.75f:
                        hit = Physics2D.Raycast(transform.position, Vector2.down, 10, LayerMask.GetMask("Map"));
                        if (hit)
                        {
                            Debug.Log(hit.point);
                            Instantiate(enemyPrefab, hit.point + Vector2.up, Quaternion.Euler(0, 0, 0));
                        }
                        else Instantiate(enemyPrefab, transform.position + Vector3.down * 10, Quaternion.Euler(0, 0, 0));
                        break;
                    default:
                        hit = Physics2D.Raycast(transform.position, Vector2.right, 10, LayerMask.GetMask("Map"));
                        if (hit)
                        {
                            Debug.Log(hit.point);
                            Instantiate(enemyPrefab, hit.point + Vector2.left, Quaternion.Euler(0, 0, 0));
                        }
                        else Instantiate(enemyPrefab, transform.position + Vector3.right * 10, Quaternion.Euler(0, 0, 0));
                        break;
                }
            }
        }
    }

    public void LoadData(GameData data)
    {
        timeInRun = data.timeInRun;
    }

    public void SaveData(ref GameData data)
    {
        data.timeInRun = timeInRun;
    }
}
