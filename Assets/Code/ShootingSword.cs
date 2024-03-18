using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSword : MonoBehaviour
{
    public string superMobTag = "SuperMob";
    public bool shootingSkill = false;
    public bool buildingSkill = false;
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
            if (shootingSkill)
            {
                player.SetShootingToTrue();
                // boss on level1
                GameObject superMob = GameObject.FindGameObjectWithTag(superMobTag);
                if (superMob != null)
                {
                    Rigidbody2D rb = superMob.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.gravityScale = 1;
                        superMob.GetComponent<SuperMob>().StartGeneratingMobs();
                    }
                }
            }

            // level2 building skill
            if (buildingSkill)
            {
                player.SetBuildingToTrue();
            }

            Destroy(gameObject);

        }
    }
}
