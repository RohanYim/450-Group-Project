using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

  
    public GameObject healingPackPrefab;
    private GameObject spawnedHealingPack;
    private Animator animator;

    private bool isOpen = false;


    private void Awake()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{ 
    //    if (!isOpen && collision.CompareTag("Player") && spawnedHealingPack == null)
    //    {
    //        isOpen = true;
    //        OpenChest();
    //    }
    //}

    //void OpenChest()
    //{
    //    animator.SetTrigger("Open");

    //    // Spawn the healing pack at the chest's location (or slightly above it)
    //    spawnedHealingPack = Instantiate(healingPackPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

    //    // disable the chest collider here to prevent re-opening
    //    GetComponent<Collider2D>().enabled = false;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen && collision.CompareTag("Player"))
        {
            isOpen = true;
            animator.SetTrigger("Open");
            Invoke(nameof(SpawnHealingPack), 0.5f); // This will call SpawnHealingPack after 0.5 seconds
        }
    }

    private void SpawnHealingPack()
    {
        // Instantiate the healing pack prefab with a slight offset above the chest
        if (healingPackPrefab != null)
        {
            spawnedHealingPack = Instantiate(healingPackPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }
}
