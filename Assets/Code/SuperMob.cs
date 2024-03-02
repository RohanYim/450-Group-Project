//reference: https://www.youtube.com/watch?v=_lREXfAMUcE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperMob : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 5f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;
    // Change the max health bar to 10f
    public float health, maxHealth = 10f;
    Rigidbody2D rb;

    public HealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    public Vector3 attackScale = new Vector3(200f, 1f, 200f);
    public float scaleDuration = 5f; 
    private bool isAttacking = false;

    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.right * moveDistance;
        rb = GetComponent<Rigidbody2D>();
    }

    private void StartAttack()
    {
        StartCoroutine(PerformAttack());
    }

    // the mob will attack when it reaches both eages of their routes
    IEnumerator PerformAttack()
    {
        isAttacking = true;

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(attackScale.x, originalScale.y, attackScale.z);

        float timer = 0;
        while (timer <= scaleDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / scaleDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;
        while (timer <= scaleDuration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, timer / scaleDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        isAttacking = false;
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
        // After the knight take the yellow sword, the super mob will fall down onto the platform and move right and left.
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 1f && rb.gravityScale != 0)
        {
            if (movingRight)
            {
                targetPosition = startPosition - Vector3.right * moveDistance;
            }
            else
            {
                targetPosition = startPosition + Vector3.right * moveDistance;
            }
            movingRight = !movingRight; 

            if (!isAttacking)
            {
                // StartAttack();
            }
        }
    }


    // attack players
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAttacking && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(1);
        }

        // If attacked by Projectile, minus 1 health point
        if (collision.gameObject.GetComponent<Projectile>())
        {
            gameObject.GetComponent<SuperMob>().TakeDamage(1);
        }
    }




}



  
