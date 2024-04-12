using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// only show finish line when SuperMob is defeated
public class FinishLine : MonoBehaviour
{
    public bool SuperMobDefeated = false;
    public bool BossDefeated = false;

    void Start()
    {
       GetComponent<Collider2D>().enabled = false; 
       GetComponent<SpriteRenderer>().enabled = false;
    }

    public void DefeatBoss() {
        BossDefeated = true;
    }

    public void DefeatSuperMob() {
        SuperMobDefeated = true;
    }

    public void aliveBoss() {
        BossDefeated = false;
    }

    public void aliveSuperMob() {
        SuperMobDefeated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (GameManager.Instance != null) {
                GameManager.Instance.AddLevelCompletionTime(Time.timeSinceLevelLoad);
            } 
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                GameManager.Instance.SaveTimeIfInTopThree();
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ActivateFinishLine()
    {
        if (SuperMobDefeated && BossDefeated) {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;

        }
        
    }
}
