using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{

    public float speed;
    public int startingPoint;
    public Transform[] points;

    private int i;
    private Vector3 originalLocalScale;

    void Start()
    {
        transform.position = points[startingPoint].position;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (marked as "Is Trigger") attached to this object
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Set the platform as the parent of the player
           
            other.transform.SetParent(this.transform);
        }
    }

    // OnTriggerExit2D is called when the Collider2D other has stopped touching the trigger (marked as "Is Trigger") attached to this object
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Remove the platform as the parent of the player
           
            other.transform.SetParent(null);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Check if the collision is with the player
        {
            originalLocalScale = collision.transform.localScale;
            collision.transform.SetParent(transform); // Set the platform as the parent of the player
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Check if the player left the platform
        {
            collision.transform.SetParent(null); // Unset the parent
            collision.transform.localScale = originalLocalScale;
        }

    }
}
