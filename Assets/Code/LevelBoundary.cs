using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class LevelBoundary : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            /*if (other.gameObject.GetComponent<Player>())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }*/
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}