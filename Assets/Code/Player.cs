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
    public Vector3 cameraOffset = new Vector3(0f, 1f, -10f);
    public float cameraFollowSpeed = 10f;
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
    private bool isShooting = false;

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
	}

    void Update()
    {
        _inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        CheckJumpAbility();
        HandleAttackAndShoot();
    }

    void FixedUpdate()
    {
        HandleMirrorAndRotation();
        HandleMovementAndJump();
    }

    void CheckJumpAbility()
    {
        _hit = Physics2D.Linecast(new Vector2(_GroundCast.position.x, _GroundCast.position.y + 0.2f), _GroundCast.position);
        if (_hit && !_hit.transform.CompareTag("Player"))
        {
            _canJump = true;
            _canWalk = true;
        }
        else
        {
            _canJump = false;
        }
        
        if (_inputAxis.y > 0 && _canJump)
        {
            _canWalk = false;
            _isJump = true;
        }
    }

    void HandleAttackAndShoot()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging) 
        {
            blade.GetComponent<Blade>().StartSwing();
        }

        if (Input.GetMouseButtonDown(1) && isShooting)
        {
            GameObject newProjectile = Instantiate(projectilePrefab, transform.position, blade.rotation);
        }
    }

    void HandleMirrorAndRotation()
    {
        Vector3 dir = cam.ScreenToWorldPoint(Input.mousePosition) - _Blade.transform.position;
        dir.Normalize();

        mirror = cam.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x;

        if (!mirror)
        {
            transform.localScale = new Vector3(_startScale, _startScale, 1);
            _Blade.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
        }
        else
        {
            transform.localScale = new Vector3(-_startScale, _startScale, 1);
            _Blade.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg, Vector3.forward);
        }
    }

    void HandleMovementAndJump()
    {
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

    // player being attacked
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

    // Once the knight pick up the yellow sword, knight can shoot the sword.
    public void SetShootingToTrue()
    {
        isShooting = true;
    }



}
