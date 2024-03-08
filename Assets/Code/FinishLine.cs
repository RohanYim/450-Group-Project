using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// only show finish line when SuperMod is defeated
public class FinishLine : MonoBehaviour
{
    //void Start()
    //{
    //    GetComponent<Collider2D>().enabled = false; 
    //    GetComponent<SpriteRenderer>().enabled = false;
    //}
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ActivateFinishLine()
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
