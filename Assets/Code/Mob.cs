//reference: https://www.youtube.com/watch?v=_lREXfAMUcE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mob : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 5f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;

    //public static event Action<Mob> OnEnemykilled;
    [SerializeField] float health, maxHealth = 3f;

    [SerializeField] HealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        //target = Gameobject.Find("Player");

        startPosition = transform.position;
        targetPosition = startPosition + Vector3.right * moveDistance;
    }


    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            if (movingRight)
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


}



  
