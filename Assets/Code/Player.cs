using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
    public float WalkSpeed;
    public float JumpForce;
    public AnimationClip _walk, _jump;
    public Animation _Legs;
    public Transform _Blade, _GroundCast;
    public Camera cam;
    public bool mirror;

    public float health = 10f; 
    public float maxHealth = 10f; 
    public HealthBar healthBar; 


    private bool _canJump, _canWalk;
    private bool _isWalk, _isJump;
    private float rot, _startScale;
    private Rigidbody2D rig;
    private Vector2 _inputAxis;
    private RaycastHit2D _hit;

    public Transform blade; 
    public float swingSpeed = 1f;
    public Vector3 swingRotation = new Vector3(0f, 0f, -85f); 

    private Quaternion originalRotation; 
    private Quaternion targetRotation; 
    private bool isSwinging = false; 
    private bool canDealDamage = false;

    public GameObject projectilePrefab;

    

    
    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

	void Start ()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
        _startScale = transform.localScale.x;
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);

        if (blade != null)
        {
            originalRotation = blade.localRotation;
        }
	}

    void Update()
    {
        if (_hit = Physics2D.Linecast(new Vector2(_GroundCast.position.x, _GroundCast.position.y + 0.2f), _GroundCast.position))
        {
            if (!_hit.transform.CompareTag("Player"))
            {
                _canJump = true;
                _canWalk = true;
            }
        }
        else _canJump = false;

        _inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (_inputAxis.y > 0 && _canJump)
        {
            _canWalk = false;
            _isJump = true;
        }

        if(healthBar.transform != null)
        {
            healthBar.transform.localScale = new Vector3(Mathf.Abs(healthBar.transform.localScale.x) * Mathf.Sign(transform.localScale.x), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
            healthBar.transform.localRotation = Quaternion.identity; 
        }

        if (Input.GetMouseButtonDown(0) && !isSwinging) 
        {
            StartCoroutine(SwingBlade());
        }

        //shoot
        if (Input.GetMouseButtonDown(1))
        {
            GameObject newProjectile = Instantiate(projectilePrefab);
            newProjectile.transform.position = transform.position;
            newProjectile.transform.rotation = blade.rotation;
        }
    }

    private IEnumerator SwingBlade()
    {
        isSwinging = true;
        canDealDamage = true;
        float time = 0f;
        targetRotation = Quaternion.Euler(swingRotation) * originalRotation; 

        while (time < 1f)
        {
            blade.localRotation = Quaternion.Lerp(originalRotation, targetRotation, time);
            time += Time.deltaTime * swingSpeed;
            yield return null;
        }

        time = 0f;
        while (time < 1f)
        {
            blade.localRotation = Quaternion.Lerp(targetRotation, originalRotation, time);
            time += Time.deltaTime * swingSpeed;
            yield return null;
        }

        blade.localRotation = originalRotation; 
        isSwinging = false;
        canDealDamage = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (canDealDamage && isSwinging && other.gameObject.CompareTag("Mob"))
        {
            var mob = other.GetComponent<Mob>(); 
            if (mob != null)
            {
                mob.TakeDamage(1); 
                canDealDamage = false;
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 dir = cam.ScreenToWorldPoint(Input.mousePosition) - _Blade.transform.position;
        dir.Normalize();

        if (cam.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x + 0.2f)
            mirror = false;
        if (cam.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x - 0.2f)
            mirror = true;

        if (!mirror)
        {
            rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(_startScale, _startScale, 1);
            _Blade.transform.rotation = Quaternion.AngleAxis(rot, Vector3.forward);
        }
        if (mirror)
        {
            rot = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(-_startScale, _startScale, 1);
            _Blade.transform.rotation = Quaternion.AngleAxis(rot, Vector3.forward);
        }

        if (_inputAxis.x != 0)
        {
            rig.velocity = new Vector2(_inputAxis.x * WalkSpeed * Time.deltaTime, rig.velocity.y);

            if (_canWalk)
            {
                _Legs.clip = _walk;
                _Legs.Play();
            }
        }

        else
        {
            rig.velocity = new Vector2(0, rig.velocity.y);
        }

        if (_isJump)
        {
            rig.AddForce(new Vector2(0, JumpForce));
            _Legs.clip = _jump;
            _Legs.Play();
            _canJump = false;
            _isJump = false;
        }
    }

    public bool IsMirror()
    {
        return mirror;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, _GroundCast.position);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
}
