using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("The AudioManager is null");
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        BackgroundMusic = BackgroundMusic_easy;
    }

    public AudioSource pickupAudio;
    public AudioSource levelupAudio_easy;
    public AudioSource levelupAudio_medium;
    public AudioSource levelupAudio_hard;

    public GameObject BackgroundMusic_easy;
    public GameObject BackgroundMusic_medium;
    public GameObject BackgroundMusic_hard;

    private GameObject BackgroundMusic;

    public void PlayPickUpSound()
    {
        pickupAudio.Play();
    }

    public void PlayLevelUpSound(int level)
    {
        BackgroundMusicState(false);
        if (level < 2)
            levelupAudio_easy.Play();
        else if (level < 4)
        {
            levelupAudio_medium.Play();
            BackgroundMusic = BackgroundMusic_medium;
        } 
        else
        {
            levelupAudio_hard.Play();
            BackgroundMusic = BackgroundMusic_hard;
        }
    }

    public void BackgroundMusicState(bool state)
    {
        if (state == true)
            BackgroundMusic.SetActive(true);
        else
            BackgroundMusic.SetActive(false);
    }
}
