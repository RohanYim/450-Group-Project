using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_audio : MonoBehaviour
{
    private static Test_audio instance = null;

    private AudioSource audioSource;  
    public float volumeChangeAmount = 0.1f;  
    public static Test_audio Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void IncreaseVolume()
    {
        if (audioSource != null && audioSource.volume < 1)
        {
            audioSource.volume += volumeChangeAmount;
        }
    }

    public void DecreaseVolume()
    {
        if (audioSource != null && audioSource.volume > 0)
        {
            audioSource.volume -= volumeChangeAmount;
        }
    }


}
