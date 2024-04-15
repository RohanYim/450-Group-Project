using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float TotalTime { get; private set; } = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);


        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddLevelCompletionTime(float time)
    {
        TotalTime += time;
    }

    public void ResetTotalTime()
    {
        TotalTime = 0f;
    }

    public void SaveTimeIfInTopThree()
    {
        List<float> topTimes = new List<float>();
        for (int i = 0; i < 3; i++)
        {
            topTimes.Add(PlayerPrefs.GetFloat($"TopTime{i}", float.MaxValue));
        }

        topTimes.Add(TotalTime);
        topTimes.Sort();
        topTimes = topTimes.Take(3).ToList();

        for (int i = 0; i < topTimes.Count; i++)
        {
            PlayerPrefs.SetFloat($"TopTime{i}", topTimes[i]);
        }

        PlayerPrefs.Save();
    }


}
