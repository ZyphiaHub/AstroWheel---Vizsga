using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioMixer audioMixer;

    [Header("Audio Clip")]
    public AudioClip backGround;

    private const string MusicVolumeKey = "MusicVolume";
    private const float MinVolume = -80f;
    private const float MaxVolume = 0f;
    private const float VolumeStep = 0.1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Debug.LogWarning("Második  példány törölve!");
            Destroy(gameObject);
        }
    }
    /*private void Start()
    {
        musicSource.clip = backGround;
        musicSource.Play();
    }*/
    private void InitializeAudio()
    {
        float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.75f); // Alapérték: 75%
        SetMusicVolume(savedVolume);
    }

    public void SetMusicVolume(float volume)
    {
        float clampedVolume = Mathf.Clamp01(volume);
        float volumeDB = Mathf.Lerp(MinVolume, MaxVolume, clampedVolume);
        audioMixer.SetFloat("MusicVolume", volumeDB);
        PlayerPrefs.SetFloat(MusicVolumeKey, clampedVolume);
    }

    public void IncreaseVolume()
    {
        float currentVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.75f);
        SetMusicVolume(currentVolume + VolumeStep);
    }

    public void DecreaseVolume()
    {
        float currentVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.75f);
        SetMusicVolume(currentVolume - VolumeStep);
    }

    public float GetCurrentVolume()
    {
        return PlayerPrefs.GetFloat(MusicVolumeKey, 0.75f);
    }
}

