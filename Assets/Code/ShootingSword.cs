using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSword : MonoBehaviour
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
        // Once Player collide with this object, Player can shoot the sword.
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.SetShootingToTrue();
            Destroy(gameObject);
        }
    }
}
