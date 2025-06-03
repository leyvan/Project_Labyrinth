using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class Player_Behaviour : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float runSpeed = 10f;
    private float walkMagnitude = 1f;
    public float currentSpeed;

    public float turnSmoothTime = 0.2f;
    private float turnSmoothVelocity;

    private Rigidbody _rb;

    [HideInInspector]
    public Animator _animator;
    private bool isAttacking;


    public bool canMove;

    [SerializeField] private CinemachineFreeLook thirdPersonCam;
    [SerializeField] private CinemachineFreeLook enemyLockOnCam;
    public Camera cam;

    private Vector3 direction;

    public InventorySO inventory;  //I can make this private
    private PlayerHUD playerHUD;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expText;

    public PartyListScriptableObject party;
    
    private Canvas playerInfo;
    private Slider playerHealth;
    
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
    private float enemyTargetFOV = 80;
    private bool targetLockOn;
    private bool lockOnIsActive = false;

    private PlayerData playerData;
    
    

    void Start()
    {
        playerData = GetComponent<PlayerData>();
        playerHUD = GetComponentInChildren<PlayerHUD>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        shockwave = transform.GetChild(5).GetComponentInChildren<ParticleSystem>();

        defaultCameraTarget = transform.GetComponentInChildren<Transform>().Find("CameraTarget");
        
        cam = Camera.main;    //<--- change this
        
        currentSpeed = moveSpeed;
        canMove = true;
        GameEvents.current.onPlayerInMenu += OnPlayerOpensAMenu;
        GameEvents.current.onDialogueEventTriggered += () => OnDialogueEvent(true);
        GameEvents.current.onDialogueEventEnded += () => OnDialogueEvent(false);
        GameEvents.current.onBattleEnded += UpdateStats;
        
    }
    
    void Update()
    {
        //--------------------------------------------------------------Player Mechanics----------------------------------------------------//
        if (currentMode == ControllerMode.BattleMode) return;

        if (canMove && !inMenu && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        if(_animator.GetBool("InBattle"))
        {
            _animator.SetBool("InBattle", false);
        }

        //Open Inventory Player Button - I -
        if (Input.GetKeyDown(KeyCode.I))
        {
            playerHUD.ToggleInventory();
            GameEvents.current.PlayerInMenu(!inMenu);
        }

        if (inMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            playerHUD.ToggleInventory();
            GameEvents.current.PlayerInMenu(!inMenu);
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
            if (interactableObj != null) interactableObj.Interact();

            interactableObjectInProximity = false;
            interactableObj = null;
        }

        if (inMenu) return;
        //INPUT DETECTION -------------------------------------------------------------------------------------------------------------------------//
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
        if(thirdPersonCam == null) 
            thirdPersonCam = GameObject.FindGameObjectWithTag("CameraController").transform.Find("ThirdPerson Camera").GetComponent<CinemachineFreeLook>();
        
        if (isPlayerInMenu)
        {
            thirdPersonCam.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inMenu = true;
        }
        else
        {
            thirdPersonCam.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inMenu = false;
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
                playerHealth = GameObject.FindGameObjectWithTag("PlayerBattleInfo").transform.GetChild(0).Find("PlayerHealth").GetComponent<Slider>();
                playerHUD.gameObject.SetActive(false);
                break;
            case string c when !c.Contains("Battle"):
                currentMode = ControllerMode.OverWorldMode;
                thirdPersonCam = GameObject.FindGameObjectWithTag("CameraController").transform.Find("ThirdPerson Camera").GetComponent<CinemachineFreeLook>(); 
                playerHealth = playerHUD.transform.GetComponentInChildren<Slider>();
                ResetHealthBar();
                playerHUD.gameObject.SetActive(true);
                break;
        }
    }
    
    private GameObject GetNearestGameObject()
    {
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

        Vector3 moveDir = new Vector3(transform.forward.x, _rb.velocity.y, transform.forward.z);
        _rb.MovePosition(this.transform.position + moveDir * currentSpeed * Time.deltaTime);
    }


    public void DoTakeHitAnimation()
    {
        _animator.SetTrigger("Hit");
    }

    private void OnApplicationQuit()
    {
        playerData.ResetHealth();
        inventory.skillInventory.Clear();
        party.enemyParty.Clear();
    }
//---------------------------------------------------------------------------------------------------------------------------------
    public float GetAttackDmg()
    {
        return playerData.GetMainCharacterAttackDmg();
    }

    /*
    IEnumerator DoHitAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        _animator.SetTrigger("gotHit");
        SetHealthBar();
    }
    */

    public void DoMagicAttackAnim(BaseSkill skill)
    {
        /*
        _animator.SetTrigger("MagicAttack");
        shockwave.Play();
        GameObject fireball = Instantiate(projectile, firePoint.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
        */
        StartCoroutine(MagicAttackAnim(skill.attribute.ToString()));
    }

    IEnumerator MagicAttackAnim(string skill)
    {
        _animator.SetTrigger("MagicAttack");
        yield return new WaitForSeconds(0.2f);
        shockwave.Play();
        yield return new WaitForSeconds(0.1f);
        
        
        GameObject fireball = Instantiate(projectile, firePoint.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
        fireball.GetComponent<Projectile>().SetEffectForProjectile(skill);
    }
    
    public void SetHealth(float newHealth)
    {
        playerData.SetMainCharacterCurrentHealth(newHealth);
        SetHealthBar(newHealth, playerData.GetMainCharacterStats().maxHealth);
    }
    
    public void SetHealthBar(float currHealth, float maxHealth)
    {
        
        playerHealth.value = currHealth/maxHealth;
    }

    public void SetCanMove(bool lockOn)
    {
        canMove = lockOn;
    }
    

    private void OnDialogueEvent(bool dialogueInProgress)
    {
        thirdPersonCam.m_XAxis.m_InputAxisName = dialogueInProgress ?  "" : "Mouse X";
        thirdPersonCam.m_YAxis.m_InputAxisName = dialogueInProgress ?  "" : "Mouse Y";
        
        SetCanMove(!dialogueInProgress);
    }

    public void UpdateStats()
    {
        Debug.Log("CALLING UPDATE STATS =======================================");
        
        levelText.text = "Lvl " + GetComponent<PlayerData>().GetPlayerLevel();
        expText.text = GetComponent<PlayerData>().GetPlayerCurrentXP() + "/" + GetComponent<PlayerData>().GetRequiredExp();
    }

    private void ResetHealthBar()
    {
        playerData.ResetHealth();
        SetHealth(playerData.GetMainCharacterCurrentHealth());
    }

    private void OnDestroy()
    {
        GameEvents.current.onPlayerInMenu -= OnPlayerOpensAMenu;
        GameEvents.current.onBattleEnded -= UpdateStats;
    }
}
