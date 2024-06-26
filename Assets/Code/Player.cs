﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

    public static Player instance;
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

    public GameObject bushPrefab;


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
    private bool isBuilding = false;
    public int shootLeft = 50;
    public int buildingLeft = 50;

    public bool isInvincible = false;   
    public bool canFly = false;         

    public float lastDamageTime = 0;      
    public float damageCooldown = 1.0f;  

    public GameObject projectilePrefab;
    public Transform aimPivot;

    // Which scence the character is in 
    private string SceneName;

    private float jumpCooldown = 0.2f; 
    private float lastJumpTime; 
    public LayerMask groundLayer;

    public float teleportDistance = 5f;
    public float teleportCooldown = 2f; 
    private float lastTeleportTime = -2f;

    // Audio
    public AudioSource attackSound;
    public AudioSource hitSound;

    public bool isPaused;

    public TMP_Text shootleft;
    public TMP_Text buildleft;


    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);  
        }

    }

	void Start ()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
        _startScale = transform.localScale.x;
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        SceneName = SceneManager.GetActiveScene().name;
        updateShoots(shootLeft);
        updateBuilds(buildingLeft);
	}

    void Update()
    {
        if(isPaused) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInvincibility();
        }

        if (canFly)
        {
            HandleFlight();
        }

        _inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        CheckJumpAbility();
        HandleAttackAndShoot();

        Vector3 mousePostion = Input.mousePosition;
        Vector3 mousePostionInWorld = Camera.main.ScreenToWorldPoint(mousePostion);
        Vector3 fromPlayerToMouse = mousePostionInWorld - transform.position;

        float radiansToMouse = Mathf.Atan2(fromPlayerToMouse.y, fromPlayerToMouse.x);
        float angleToMouse = radiansToMouse * Mathf.Rad2Deg;

        // T for create a bush
        if (Input.GetKeyDown(KeyCode.T) && isBuilding && buildingLeft > 0) {
            CreateBushAtMousePosition();
        }

        /*if (Input.GetMouseButtonDown(1) && SceneName == "Level_4")
        {

            GameObject newProjectile = Instantiate(projectilePrefab);
            newProjectile.transform.position = transform.position;
            newProjectile.transform.rotation = _Blade.rotation;
            if (mirror)
            {
                newProjectile.transform.Rotate(0, 0, 180);
            }

        }*/

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformTeleport();
        }

        //aimPivot.rotation = Quaternion.Euler(0, 0, angleToMouse);
    }

    void HandleFlight()
    {
        float verticalInput = Input.GetAxis("Vertical") * 10; 
        float horizontalInput = Input.GetAxis("Horizontal") * 10;
        rig.velocity = new Vector2(rig.velocity.x, verticalInput);
        rig.velocity = new Vector2(horizontalInput, verticalInput);
    }

    void CreateBushAtMousePosition() {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePositionInWorld.z = 0; 

        Instantiate(bushPrefab, mousePositionInWorld, Quaternion.identity);
        buildingLeft--;
        updateBuilds(buildingLeft);
    }

    void FixedUpdate()
    {
        HandleMirrorAndRotation();
        HandleMovementAndJump();
    }

    void ToggleInvincibility()
    {
        isInvincible = !isInvincible; 
        canFly = isInvincible;        

        rig.gravityScale = isInvincible ? 0 : 2; 

        if (!isInvincible)
        {
            rig.velocity = new Vector2(rig.velocity.x, 0); 
            rig.velocity = new Vector2(_inputAxis.x * WalkSpeed * Time.deltaTime, rig.velocity.y);
        }
    }

    void CheckJumpAbility()
    {
        if (Time.time - lastJumpTime < jumpCooldown) return;
        _hit = Physics2D.Linecast(new Vector2(_GroundCast.position.x, _GroundCast.position.y + 0.2f), _GroundCast.position, groundLayer);
        if (_hit) {
            _canJump = true;
            _canWalk = true;
        } else {
            _canJump = false;
        }
        
        if (_inputAxis.y > 0 && _canJump)
        {
            _canWalk = false;
            _isJump = true;
            lastJumpTime = Time.time;
        }
    }

    void HandleAttackAndShoot()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging) 
        {
            blade.GetComponent<Blade>().StartSwing();
            attackSound.Play();
        }

        if (Input.GetMouseButtonDown(1) && isShooting && shootLeft > 0)
        {
            shootLeft -= 1;
            updateShoots(shootLeft);
            GameObject newProjectile = Instantiate(projectilePrefab);
            newProjectile.transform.position = transform.position;
            newProjectile.transform.rotation = _Blade.rotation;
            if (mirror)
            {
                newProjectile.transform.Rotate(0, 0, 180);
            }

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
            if (SceneName =="Level_1" || SceneName == "Level_2" || SceneName == "Level_3" || SceneName == "Level_4")
            {
                rig.velocity = new Vector2(_inputAxis.x * WalkSpeed * Time.deltaTime, rig.velocity.y);

                if (_canWalk)
                {
                    _Legs.clip = _walk;
                    _Legs.Play();
                }
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
        if (Time.time - lastDamageTime < damageCooldown) return;  
        hitSound.Play();  

        if (isInvincible) return;  

        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);  
        lastDamageTime = Time.time;  

        if (health <= 0)
        {
            Die();  
        }
    }


    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        FinishLine finishLine = FindObjectOfType<FinishLine>(); 
        Door door = FindObjectOfType<Door>(); 

        if(finishLine != null && door != null)
        {
            finishLine.aliveSuperMob(); 
            finishLine.aliveBoss(); 
            door.aliveBoss(); 
            door.aliveSuperMob();
        }
    }

    // Once the knight pick up the yellow sword, knight can shoot the sword.
    public void SetShootingToTrue()
    {
        print(shootLeft);
        shootLeft = 50;
        updateShoots(shootLeft);
        isShooting = true;
    }

    // Once the knight pick up the yellow sword on level2, knight can build the bushes.
    public void SetBuildingToTrue()
    {
        buildingLeft = 50;
        isBuilding = true;
        updateBuilds(buildingLeft);
    }

    public void updateShoots(int num) {
        if(shootleft != null){
            shootleft.text = "Shoot lefts: " + num;
        }
    }

    public void updateBuilds(int num) {
        if(buildleft != null) {
            buildleft.text = "Build lefts: " + num;
        }
        
    }

    void PerformTeleport()
    {
       
        if (Time.time - lastTeleportTime >= teleportCooldown)
        {
      
            Vector3 teleportDirection = mirror ? Vector3.left : Vector3.right;

    
            transform.position += teleportDirection * teleportDistance;

            lastTeleportTime = Time.time;
        }
    }

    public void AddKnockback(Vector2 force)
    {
        GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }

}
