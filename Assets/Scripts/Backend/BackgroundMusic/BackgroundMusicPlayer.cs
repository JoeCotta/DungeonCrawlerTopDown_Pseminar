using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{

    [SerializeField] private AudioSource[] backgroundMusic;
    private AudioSource currentAudioSource;
    void Awake() {
        GameObject[] possibleSameInstances = GameObject.FindGameObjectsWithTag("BackgroundMusicPlayer");
        if(possibleSameInstances.Length > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
        currentAudioSource = backgroundMusic[0];
        playRandomMusic();
    }

    void Update()
    {
        if (!currentAudioSource.isPlaying) playRandomMusic();
        if(Input.GetKeyDown("n")) playRandomMusic();
    }

    void playRandomMusic()
    {
        currentAudioSource.Stop();
        currentAudioSource = backgroundMusic[Random.Range(0, backgroundMusic.Length)]; 
        currentAudioSource.Play();
    }
}
