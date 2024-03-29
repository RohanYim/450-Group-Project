//reference: https://www.youtube.com/watch?v=_lREXfAMUcE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 5f;

    // drop projecttile or not
    public bool dropProjectileOnDeath = false;
    public GameObject projectileDropPrefab;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    public GameObject player;
    private float timer = 0f;
    public float appearanceInterval = 3f;

    public GameObject boss;

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
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.right * moveDistance;

        player = GameObject.FindGameObjectWithTag("Player");
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
        // drop Projectile
        if (dropProjectileOnDeath && projectileDropPrefab != null)
        {

            Instantiate(projectileDropPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }


    void Update()
    {


        timer += Time.deltaTime;
        if (timer >= appearanceInterval && boss!=null && boss.transform.position.x - player.transform.position.x < 10)
        {
            AppearBehindPlayer();
            timer = 0f;
        }

        Vector3 mousePostion = Input.mousePosition;
        Vector3 mousePostionInWorld = Camera.main.ScreenToWorldPoint(mousePostion);
    }



    // attack players
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(1);
        }

        if (collision.gameObject.GetComponent<Projectile>())
        {
            //Destroy(gameObject);
            gameObject.GetComponent<Skeleton>().TakeDamage(1);
        }
    }

    void AppearBehindPlayer()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x - 2, transform.position.y, transform.position.z);

        }
    }




}



