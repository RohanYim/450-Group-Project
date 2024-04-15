using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance = null;
    public AudioSource audioSource;
    public float volumeChangeAmount = 0.1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            
        }
        else if (Instance != this)
        {
            Destroy(gameObject); 
        }
    }

    
    public void IncreaseVolume()
    {
        if (audioSource.volume < 1)
        {
            audioSource.volume += volumeChangeAmount;
        }
    }

    public void DecreaseVolume()
    {
        if (audioSource.volume > 0)
        {
            audioSource.volume -= volumeChangeAmount;
            if (audioSource.volume < 0)
            {
                audioSource.volume = 0;
            }
        }
    }


}
