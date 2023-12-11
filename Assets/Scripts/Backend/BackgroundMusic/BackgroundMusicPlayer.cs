using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{

    [SerializeField] private AudioSource[] backgroundMusic;
    [SerializeField] private AudioSource[] bossBackgroundMusic;
    private AudioSource currentAudioSource;
    private AudioSource bossAudioSource;
    float t_out = 0;
    float t_in = 0;
    bool startBossSound = false;

    void Awake() {
        GameObject[] possibleSameInstances = GameObject.FindGameObjectsWithTag("BackgroundMusicPlayer");
        if(possibleSameInstances.Length > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
        currentAudioSource = backgroundMusic[0];
        playRandomMusic();
    }

    void Update()
    {
        if (GameManager.isBossSpawned && (!bossAudioSource || !bossAudioSource.isPlaying)) startBossSound = true;
        else if (!currentAudioSource.isPlaying && !GameManager.isBossSpawned) playRandomMusic();
        else if(Input.GetKeyDown("n")) playRandomMusic();

        if (startBossSound) manageBossMusic();
    }

    void playRandomMusic()
    {
        if (bossAudioSource) bossAudioSource.Stop();
        currentAudioSource.Stop();
        currentAudioSource = backgroundMusic[Random.Range(0, backgroundMusic.Length)]; 
        currentAudioSource.Play();
    }

    void manageBossMusic()
    {
        if (t_out == 0f) playRandomBossMusic();

        if (t_out < 2)
        {
            currentAudioSource.volume = (2 - t_out) / 2;
            t_out += Time.deltaTime;
        }
        else
        {
            currentAudioSource.Stop();
        }

        if (t_out >= 2 && t_in < 2)
        {
            bossAudioSource.volume = t_in / 2;
            t_in += Time.deltaTime;
        }
        else if(t_in >= 2)
        {
            bossAudioSource.volume = 1f;
            startBossSound = false;
            t_out = 0;
            t_in = 0;
        }
    }
    void playRandomBossMusic()
    {
        bossAudioSource = bossBackgroundMusic[Random.Range(0, bossBackgroundMusic.Length)]; 
        bossAudioSource.Play();
        bossAudioSource.volume = 0f;
    }
}
