using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider masterSlider;
    public Slider weaponSlider;
    public Slider playerSlider;
    public Slider enemySlider;
    public Slider uiSlider;
    public Slider mapSlider;
    public Slider musicSlider;
    public Slider boostSlider;

    DataPersistenceManager dataPersistenceManager;

    void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        masterSlider.value = dataPersistenceManager.gameData.MasterVol;
        weaponSlider.value = dataPersistenceManager.gameData.WeaponVol;
        playerSlider.value = dataPersistenceManager.gameData.PlayerVol;
        enemySlider.value = dataPersistenceManager.gameData.EnemyVol;
        uiSlider.value = dataPersistenceManager.gameData.UIVol;
        mapSlider.value = dataPersistenceManager.gameData.MapVol;
        musicSlider.value = dataPersistenceManager.gameData.MusicVol;
        boostSlider.value = dataPersistenceManager.gameData.BoostsVol;

    }

    public void SetMasterLvl(Slider slider)
    {
        masterMixer.SetFloat("MasterVol", slider.value);
        dataPersistenceManager.gameData.MasterVol = slider.value;
    }
    public void SetWeaponLvl(Slider slider)
    {
        masterMixer.SetFloat("WeaponVol", slider.value);
        dataPersistenceManager.gameData.WeaponVol = slider.value;
    }
    public void SetPlayerLvl(Slider slider)
    {
        masterMixer.SetFloat("PlayerVol", slider.value);
        dataPersistenceManager.gameData.PlayerVol = slider.value;
    }
    public void SetEnemyLvl(Slider slider)
    {
        masterMixer.SetFloat("EnemyVol", slider.value);
        dataPersistenceManager.gameData.EnemyVol = slider.value;
    }
    public void SetUILvl(Slider slider)
    {
        masterMixer.SetFloat("UIVol", slider.value);
        dataPersistenceManager.gameData.UIVol = slider.value;
    }
    public void SetMapLvl(Slider slider)
    {
        masterMixer.SetFloat("MapVol", slider.value);
        dataPersistenceManager.gameData.MapVol = slider.value;
    }
    public void SetMusicLvl(Slider slider)
    {
        masterMixer.SetFloat("MusicVol", slider.value);
        dataPersistenceManager.gameData.MusicVol = slider.value;
    }
    public void SetBoostsLvl(Slider slider)
    {
        masterMixer.SetFloat("BoostsVol", slider.value);
        dataPersistenceManager.gameData.BoostsVol = slider.value;
    }
    public void SaveGame()
    {
        dataPersistenceManager.SaveGame();
    }
}
