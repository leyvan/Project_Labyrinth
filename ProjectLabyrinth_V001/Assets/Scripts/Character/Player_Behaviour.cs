using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class Player_Behaviour : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float runSpeed = 10f;
    private float walkMagnitude = 1f;
    public float currentSpeed;
    public float rotateSpeed = 75f;
    public float jumpVelocity = 5f;
    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;

    public float turnSmoothTime = 0.2f;
    private float turnSmoothVelocity;

    private CharacterController _ctrl;
    private Rigidbody _rb;
    private CapsuleCollider _col;

    [HideInInspector]
    public Animator _animator;
    private bool isAttacking;


    public bool canMove;
    private Transform playerCamTarget;

    [SerializeField] private CinemachineFreeLook thirdPersonCam;
    [SerializeField] private CinemachineFreeLook enemyLockOnCam;
    public Camera cam;
    public float effectTime;

    private Vector3 direction;

    public InventorySO inventory;  //I can make this private
    private PlayerHUD playerHUD;

    public PartyListScriptableObject party;
    public CharacterStats mainCharacterStats;
    
    private Canvas playerInfo;
    private Slider playerHealth;

    private float health = 1f;
    private float maxHealth = 1f;
    private float attack;

    private bool dead = false; 
    
    public enum ControllerMode{BattleMode,OverWorldMode};
    [SerializeField] public ControllerMode currentMode;

    public GameObject projectile;
    private ParticleSystem shockwave;
    public Transform firePoint;

    public bool inMenu = false;

    private bool interactableObjectInProximity = false;
    private IInteractable interactableObj;

    private Transform enemyCameraTarget;
    private Transform defaultCameraTarget;
    public float defaultFOV;
    private float enemyTargetFOV = 80;
    private bool targetLockOn;
    private bool lockOnIsActive = false;

    public bool TargetLockOn {
        get
        {
            return targetLockOn;
        }
        set
        {
            targetLockOn = value;
        }
    }
    
    void Awake()
    {
        //SetControllerMode(SceneManager.GetActiveScene().name);
        playerCamTarget = transform.GetChild(1).transform;
        playerHUD = GetComponentInChildren<PlayerHUD>();
        playerHealth = playerHUD.transform.GetComponentInChildren<Slider>();
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _animator = GetComponentInChildren<Animator>();
        _ctrl = GetComponent<CharacterController>();
        shockwave = transform.GetChild(5).GetComponentInChildren<ParticleSystem>();

        defaultCameraTarget = transform.GetComponentInChildren<Transform>().Find("CameraTarget");
        
        cam = Camera.main;    //<--- change this
    }

    void Start()
    {

        //2- Find and return GameBehavior script attached to Game Manager object in scene
        currentSpeed = moveSpeed;
        canMove = true;
        GameEvents.current.onPlayerInMenu += OnPlayerOpensAMenu;

        if (HealthManager.Instance != null)
        {
            SetHealth(HealthManager.Instance.GetPersistentHealth());
        }
        
        SetDamage(mainCharacterStats.attack);
    }
    
    void Update()
    {
        //--------------------------------------------------------------Player Mechanics----------------------------------------------------//
        if (currentMode == ControllerMode.BattleMode) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (canMove == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            
        }
        
        if(_animator.GetBool("InBattle") == true)
        {
            _animator.SetBool("InBattle", false);
        }

        //Open Inventory Player Button - I -
        if (Input.GetKeyDown(KeyCode.I))
        {
            playerHUD.OpenInventory();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            LockOntoTarget(targetLockOn);
        }

        if (lockOnIsActive == true)
        {
            if (enemyCameraTarget != null)
            {
                transform.LookAt(enemyCameraTarget);
            }
        }

        //Interact Player Button - E -
        if (Input.GetKey(KeyCode.E) && interactableObjectInProximity)
        {
            interactableObj.Interact();
        }
        

        float _vInput = Input.GetAxisRaw("Vertical");
        float _hInput = Input.GetAxisRaw("Horizontal");
        direction = new Vector3(_hInput, 0f, _vInput).normalized;
        //Character Movement and Animation Controller                          
        //Check if the player is trying to move
        //--------------------------------------------------------------Movement Detection + Movement Mechanics----------------------------------------------------//
        if (direction.magnitude >= 0.1f && canMove == true)
        {
            _animator.SetBool("walking?", true);
            //Check if the player is trying to run - Player Running -
            if (Input.GetKey(KeyCode.W))
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    currentSpeed = runSpeed;
                    _animator.SetBool("running?", true);
                }
                else
                {

                    _animator.SetBool("running?", false);
                    _animator.SetBool("walking?", true);
                    currentSpeed = moveSpeed;

                }

            }
        }
        else
        {
            _animator.SetBool("running?", false);
            _animator.SetBool("walking?", false);
        }

    }

    void FixedUpdate()
    {
        if(direction.magnitude >= 0.1f && canMove == true)
        {
            if (inMenu == true) return;
            
            Move();
        }
    }

    private void OnPlayerOpensAMenu(bool isPlayerInMenu)
    {
        if (isPlayerInMenu)
        {
            thirdPersonCam.enabled = false;
        }
        else
        {
            thirdPersonCam.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();
        //var enemyTarget = other.GetComponent<ACEnemyAI>();
        if (interactable != null)
        {
            interactableObjectInProximity = true;
            interactableObj = interactable;
        }
        else if(other.tag == "Enemy")
        {
            enemyCameraTarget = other.transform;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();
        //var enemyTarget = other.GetComponent<ACEnemyAI>();
        if (interactable != null)
        {
            interactableObjectInProximity = false;
            interactableObj = null;
        }
        else if(other.tag == "Enemy")
        {
            enemyCameraTarget = null;
        }
    }
    
    
    private void LockOntoTarget(bool lockOn)
    {
        if (lockOnIsActive == false)
        {
            if (enemyCameraTarget == null) return;
            thirdPersonCam.enabled = false;
            enemyLockOnCam.enabled = true;
            Debug.LogError("ON");
        }
        else
        {
            thirdPersonCam.enabled = true;
            enemyLockOnCam.enabled = false;
            thirdPersonCam.m_LookAt = defaultCameraTarget;
            Debug.LogError("OFF");
        }

        targetLockOn = !lockOn;
        lockOnIsActive = !lockOnIsActive;
    }



    public void SetControllerMode(string scene)
    {
        switch (scene)
        {
            case string b when b.Contains("Battle"):
                currentMode = ControllerMode.BattleMode;
                _animator.SetBool("InBattle", true);
                playerHUD.gameObject.SetActive(false);
                break;
            case string c when !c.Contains("Battle"):
                currentMode = ControllerMode.OverWorldMode;
                _animator.SetBool("InBattle", false);
                playerHUD.gameObject.SetActive(true);
                break;
        }
    }

 
    private GameObject GetNearestGameObject()
    {
        //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            var nearestObj = hit.transform.gameObject;
            if(nearestObj != null)
            {
                return nearestObj;
            }
        }
        return null;

        
    }

   
    
    public ControllerMode GetCurrentControllerMode()
    {
        return currentMode;
    }

    void Move()
    {
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
        //_rb.MoveRotation(Quaternion.Euler(0f,smoothAngle, 0f));

        //Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
        //_ctrl.Move(moveDir * currentSpeed * Time.deltaTime);
        Vector3 moveDir = new Vector3(transform.forward.x, _rb.velocity.y, transform.forward.z);
        _rb.MovePosition(this.transform.position + moveDir * currentSpeed * Time.deltaTime);

    }


    public void DoTakeHitAnimation()
    {
        _animator.SetTrigger("Hit");
    }

    private void OnApplicationQuit()
    {
        mainCharacterStats.curHealth = maxHealth;
        inventory.skillInventory.Clear();
        party.enemyParty.Clear();
    }
//---------------------------------------------------------------------------------------------------------------------------------
    public float GetAttackDmg()
    {
        return mainCharacterStats.attack;
    }

    /*
    IEnumerator DoHitAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        _animator.SetTrigger("gotHit");
        SetHealthBar();
    }
    */

    public void DoMagicAttackAnim()
    {
        /*
        _animator.SetTrigger("MagicAttack");
        shockwave.Play();
        GameObject fireball = Instantiate(projectile, firePoint.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
        */
        StartCoroutine(MagicAttackAnim());
    }

    IEnumerator MagicAttackAnim()
    {
        _animator.SetTrigger("MagicAttack");
        yield return new WaitForSeconds(0.2f);
        shockwave.Play();
        yield return new WaitForSeconds(0.1f);
        GameObject fireball = Instantiate(projectile, firePoint.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
    }
    
    public void SetDamage(float newAttack)
    {
        attack = newAttack;
    }
    
    public void SetHealth(float newHealth)
    {
        health = newHealth;
        maxHealth = mainCharacterStats.maxHealth;
        SetHealthBar();
    }

    public void SetOverWorldHealth()
    {
        if (HealthManager.Instance == null) return;
        health = HealthManager.Instance.GetPersistentHealth();
        SetHealthBar();
    }
    
    public void SetHealthBar()
    {
        playerHealth.value = health/maxHealth;
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public Slider GetHealthBar()
    {
        return playerHealth;
    }
    
    

    private void OnEnable()
    {
        SetOverWorldHealth();
    }
}
