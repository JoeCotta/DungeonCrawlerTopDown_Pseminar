using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{

    [SerializeField] private AudioSource[] backgroundMusic;
    [SerializeField] private AudioSource[] bossBackgroundMusic;
    private AudioSource currentAudioSource;
    private AudioSource bossAudioSource;
    bool isBoss = false;
    bool isSwitching = false;
    float t_out = 0;
    float t_in = 0;
    bool isMusicTransitionToBoss = false;
    bool isMusicTransitionFromBoss = false;

    void Awake() {
        GameObject[] possibleSameInstances = GameObject.FindGameObjectsWithTag("BackgroundMusicPlayer");
        if(possibleSameInstances.Length > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
        currentAudioSource = backgroundMusic[0];
        playRandomMusic();
    }

    void Update()
    {

        if(Input.GetKeyDown("n"))
        {
            stopAllMusic();
            playRandomMusic();
        }

        playRandomMusic();
        switchMusicType();

        if (isMusicTransitionToBoss) musicTransitionToBoss();
        else if (isMusicTransitionFromBoss) musicTransitionFromBoss();
    }

    void switchMusicType()
    {
        // starts the music transition if the boss was spawned / killed
        bool isBoss = GameManager.isBossSpawned;

        if (this.isBoss == isBoss) return;
        else this.isBoss = isBoss;

        isSwitching = true;
        musicTransition();
    }

    void musicTransition()
    {
        // starts the correct transition
        if (currentAudioSource && currentAudioSource.isPlaying) musicTransitionToBoss();
        else musicTransitionFromBoss();
    }

    void musicTransitionToBoss()
    {
        isMusicTransitionToBoss = true;
        if (t_out == 0f) playRandomBossMusic();

        // decrease Volume of normal Music
        if (t_out < 2)
        {
            currentAudioSource.volume = (2 - t_out) / 2;
            t_out += Time.deltaTime;
        }
        // after 2 seconds stop normal music
        else
        {
            currentAudioSource.Stop();
        }

        // increase Volume of boss Music after normal music is stopped
        if (t_out >= 2 && t_in < 2)
        {
            bossAudioSource.volume = t_in / 2;
            t_in += Time.deltaTime;
        }
        // resetting everything
        else if(t_in >= 2)
        {
            bossAudioSource.volume = 1f;
            currentAudioSource.volume = 0f;
            isSwitching = false;
            t_out = 0;
            t_in = 0;
            isMusicTransitionToBoss = false;
        }
    }

    void musicTransitionFromBoss()
    {
        isMusicTransitionFromBoss = true;
        if (t_out == 0f) playRandomNormalMusic();

        // decrease Volume of boss Music
        if (t_out < 2)
        {
            bossAudioSource.volume = (2 - t_out) / 2;
            t_out += Time.deltaTime;
        }
        // after 2 seconds stop boss music
        else
        {
            bossAudioSource.Stop();
        }

        // increase Volume of normal Music after boss music is stopped
        if (t_out >= 2 && t_in < 2)
        {
            currentAudioSource.volume = t_in / 2;
            t_in += Time.deltaTime;
        }
        // resetting everything
        else if(t_in >= 2)
        {
            currentAudioSource.volume = 1f;
            isSwitching = false;
            t_out = 0;
            t_in = 0;
            isMusicTransitionFromBoss = false;
        }
    }

    void playRandomMusic()
    {
        // tries to play a new Song 
        if (isSwitching) return;

        if (isBoss) playRandomBossMusic();
        else playRandomNormalMusic();
    }


    void playRandomBossMusic()
    {
        if(bossAudioSource && bossAudioSource.isPlaying) return;
        if(currentAudioSource) currentAudioSource.Stop();

        bossAudioSource = bossBackgroundMusic[Random.Range(0, bossBackgroundMusic.Length)]; 
        bossAudioSource.Play();
        if (isSwitching) bossAudioSource.volume = 0f;
    }
    void playRandomNormalMusic()
    {
        if(currentAudioSource && currentAudioSource.isPlaying) return;
        if (bossAudioSource) bossAudioSource.Stop();

        currentAudioSource = backgroundMusic[Random.Range(0, backgroundMusic.Length)]; 
        currentAudioSource.Play();

        if (isSwitching) currentAudioSource.volume = 0f;
    }

    void stopAllMusic()
    {
        if(bossAudioSource) bossAudioSource.Stop();
        if(currentAudioSource) currentAudioSource.Stop();
    }
}
