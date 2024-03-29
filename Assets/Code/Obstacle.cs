using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Debug.Log(11);
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                float newHealth = player.health/2;
                player.TakeDamage(player.health - newHealth);
            }
        }
    }
}
