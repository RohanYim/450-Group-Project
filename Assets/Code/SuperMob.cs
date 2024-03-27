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
    public float speed = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;
    // Change the max health bar to 10f
    public float health, maxHealth = 10f;
    Rigidbody2D rb;

    public HealthBar healthBar;
    public GameObject MobPrefab;




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
    // generate small mobs
    void GenerateMobs()
    {
        Vector3 puffMobPosition = new Vector3(transform.position.x - 40, -41, transform.position.z);
        Vector3 fireballMobPosition = new Vector3(transform.position.x -20, -41, transform.position.z);
        GameObject puffMob = Instantiate(MobPrefab, puffMobPosition, Quaternion.identity);
        puffMob.GetComponent<Mob>().puffAttack = true;
        GameObject fireballMob = Instantiate(MobPrefab, fireballMobPosition, Quaternion.identity);
        fireballMob.GetComponent<Mob>().fireballAttack = true;
    }

    public void StartGeneratingMobs()
    {
        StartCoroutine(GenerateMobsAfterDelay(5f));
    }

    private IEnumerator GenerateMobsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        GenerateMobs(); 
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
        // only show finish line when SuperMod is defeated
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
        // 计算SuperMob当前位置与起始位置的距离
        float distance = transform.position.x - startPosition.x;

        // 如果到达了左侧或右侧的最大距离，改变方向
        if (distance >= moveDistance)
        {
            movingRight = false;
        }
        else if (distance <= -moveDistance)
        {
            movingRight = true;
        }

        // 根据当前方向移动SuperMob
        if (movingRight)
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
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



  
