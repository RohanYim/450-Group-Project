using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public static MenuController instance;
    // Outlets

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake() {
        instance = this;
        Hide();
    }

    public void Show() {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        Player.instance.isPaused = true;
    }

    public void Hide() {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        if(Player.instance != null) {
            Player.instance.isPaused = false;
        }
    }

    public void LoadLevel1() {
        SceneManager.LoadScene("level_1");
        if (GameManager.Instance != null) {
            GameManager.Instance.AddLevelCompletionTime(Time.timeSinceLevelLoad);
        } 
        Hide();
    }

    public void LoadLevel2() {
        SceneManager.LoadScene("level_2");
        if (GameManager.Instance != null) {
            GameManager.Instance.AddLevelCompletionTime(Time.timeSinceLevelLoad);
        } 
        Hide();
    }

    public void LoadLevel3() {
        SceneManager.LoadScene("level_3");
        if (GameManager.Instance != null) {
            GameManager.Instance.AddLevelCompletionTime(Time.timeSinceLevelLoad);
        } 
        Hide();
    }

    public void LoadLevel4() {
        SceneManager.LoadScene("level_4");
        if (GameManager.Instance != null) {
            GameManager.Instance.AddLevelCompletionTime(Time.timeSinceLevelLoad);
        } 
        Hide();
    }

}

