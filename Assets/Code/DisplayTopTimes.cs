using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 
public class DisplayTopTimes : MonoBehaviour
{
    public TextMeshProUGUI Time1Text; 
    public TextMeshProUGUI Time2Text;
    public TextMeshProUGUI Time3Text;

    void Start()
    {
        DisplayTimes();
    }

    void DisplayTimes()
    {
        for (int i = 0; i < 3; i++)
        {
            float time = PlayerPrefs.GetFloat($"TopTime{i}", float.MaxValue);
            string timeString;

            if (time != float.MaxValue)
            {
                int hours = (int)time / 3600;
                int minutes = ((int)time % 3600) / 60;
                int seconds = (int)time % 60;
                timeString = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
            }
            else
            {
                timeString = "--:--:--";
            }
            switch (i)
            {
                case 0:
                    Time1Text.text = $"{timeString}";
                    break;
                case 1:
                    Time2Text.text = $"{timeString}";
                    break;
                case 2:
                    Time3Text.text = $"{timeString}";
                    break;
            }
        }
    }

    public void restart() {
        SceneManager.LoadScene(0);
    }

}
