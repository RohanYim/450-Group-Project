using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioSource audioSource; 
    public float volumeChangeAmount = 0.1f; 
    public void PlayGame()
    {
        GameManager.Instance.ResetTotalTime();
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
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
                print(1);
            }
 
        }
    }

}
