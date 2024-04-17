using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {

    //        Debug.Log(11);
    //        Player player = collision.gameObject.GetComponent<Player>();
    //        if (player != null)
    //        {
    //            float newHealth = player.health - 2f;
    //            player.TakeDamage(player.health - newHealth);
    //        }
    //    }
    //}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(11); // This will log "11" to the console when the player collides with the obstacle.
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                // Directly apply damage to the player
                player.TakeDamage(2f); // Assuming TakeDamage method subtracts health
            }
        }
    }



}
