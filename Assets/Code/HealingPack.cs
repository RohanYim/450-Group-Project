using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPack : MonoBehaviour
{
    //public int healthAmount = 5; // The amount of health this pack gives

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(2);

    //    if (other.CompareTag("Player"))
    //    {
    //        Player player = other.GetComponent<Player>();
    //        if (player != null)
    //        {
    //            // Increase the player's health but don't exceed the maxHealth
    //            player.health = Mathf.Min(player.health + healthAmount, player.maxHealth);

    //            // Update the health bar, if you have one
    //            if (player.healthBar != null)
    //            {

    //                player.healthBar.UpdateHealthBar(player.health,  player.maxHealth);
    //            }

    //            Destroy(gameObject); // Remove the healing pack after pickup
    //        }
    //    }
    //}

    public int healthAmount = 5; // The amount of health this pack gives

    private void OnTriggerEnter2D(Collider2D other) // Use OnTriggerEnter2D for 2D physics
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // Increase the player's health but don't exceed the maxHealth
                player.health = Mathf.Min(player.health + healthAmount, player.maxHealth);

                // Update the health bar, if you have one
                if (player.healthBar != null)
                {
                    player.healthBar.UpdateHealthBar(player.health, player.maxHealth);
                }

                Destroy(gameObject, 0.5f); // Remove the healing pack after pickup
            }
        }
    }


}





