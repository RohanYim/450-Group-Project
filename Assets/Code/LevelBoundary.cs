using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class LevelBoundary : MonoBehaviour
    {
        public float moveSpeed = 2f;
        public float moveDistance = 5f;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private bool movingRight = true;

        // Start is called before the first frame update
        void Start()
        {
            startPosition = transform.position;
            targetPosition = startPosition + Vector3.right * moveDistance;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (transform.position == targetPosition )
            {
                if (movingRight )
                {
                    targetPosition = startPosition;
                }
                else
                {
                    targetPosition = startPosition + Vector3.right * moveDistance;
                }
                movingRight = !movingRight;
            }
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
