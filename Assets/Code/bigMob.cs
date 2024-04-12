//reference: https://www.youtube.com/watch?v=_lREXfAMUcE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bigMob : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 5f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;
    public bool fireballAttack = false;  // fireball attacking mob
    public bool puffAttack = false; // puff attacking mob
    [SerializeField] float health, maxHealth = 3f;

    [SerializeField] HealthBar healthBar;

    public Transform player;

    Animator animator;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    public float scaleFactor = 1.5f;
    public float scaleDuration = 1f;
    private bool isAttacking = false;

    public GameObject fireballPrefab;

    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.right * moveDistance;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        animator = GetComponent<Animator>();
    }

    private void StartAttack()
    {

        // do fireball attack
        if (fireballAttack)
        {
            StartCoroutine(PerformAttackFireball());
        }

    }

    // the mob will attack when it reaches both eages of their routes

    IEnumerator PerformAttackFireball()
    {
        isAttacking = true;

        Vector3 spawnPosition = transform.position; // use mob's current position
        spawnPosition += -transform.right * 0.5f; // fireball start from left

        if (fireballPrefab != null)
        {
            Vector2 directionToTarget = player.position - transform.position;
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;


            Quaternion fireballRotation = Quaternion.Euler(0, 0, 0);
            GameObject fireballInstance = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity); // get fireball instance

            fireballInstance.transform.rotation = Quaternion.Euler(0, 0, angle + 180);

            // ignore collision between fireball and mob
            Physics2D.IgnoreCollision(fireballInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        yield return new WaitForSeconds(1);
        isAttacking = false;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        Destroy(gameObject);
        FinishLine finishLine = FindObjectOfType<FinishLine>(); 
        Door door = FindObjectOfType<Door>(); 

        if(finishLine != null && door != null)
        {
            finishLine.DefeatSuperMob(); 
            door.DefeatSuperMob(); 
            finishLine.ActivateFinishLine();
            door.ActivateFinishLine();
        }
    }


    void Update()
    {
        // if attacking, stay still
        if (!isAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 1f)
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
                StartAttack();
            }
        }

        if (player != null)
        {



        }
    }



    // attack players
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAttacking && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(1);
        }

        if (collision.gameObject.GetComponent<Projectile>())
        {
            //Destroy(gameObject);
            gameObject.GetComponent<bigMob>().TakeDamage(1);
        }
    }




}



