using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Behaviour : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float runSpeed = 9f;
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

    public Camera cam;
    public float effectTime;

    private Vector3 direction;

    public InventorySO inventory;  //I can make this private
    public GameObject playerHUD;

    public PartyListScriptableObject party;

    public enum ControllerMode{BattleMode,OverWorldMode};
    [SerializeField] public ControllerMode currentMode;

    void Awake()
    {
        //SetControllerMode(SceneManager.GetActiveScene().name);

        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _animator = GetComponentInChildren<Animator>();
        _ctrl = GetComponent<CharacterController>();
        cam = Camera.main;    //<--- change this
    }

    void Start()
    {

        //2- Find and return GameBehavior script attached to Game Manager object in scene
        currentSpeed = moveSpeed;
        canMove = true;

    }

    void Update()
    {

        //--------------------------------------------------------------Player Mechanics----------------------------------------------------//

        if (currentMode == ControllerMode.BattleMode) return;

        if(_animator.GetBool("InBattle") == true)
        {
            _animator.SetBool("InBattle", false);
        }



        //Open Inventory Player Button - I -
        if (Input.GetKeyDown(KeyCode.I))
        {
            playerHUD.GetComponent<PlayerHUD>().OpenInventory();
        }

        //Interact Player Button - E -
        GameObject nearestGameObject = GetNearestGameObject();
        if (nearestGameObject == null) return;
        if (Input.GetKey(KeyCode.E))
        {
            var interactable = nearestGameObject.GetComponent<IInteractable>();
            if (interactable == null) return;
            interactable.Interact();
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
        /*
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _animator.SetTrigger("attack");
        }
        */
        

//---------------------------------------------------------------Animator States-----------------------------------------------------//

        /*
        AnimatorStateInfo animInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (animInfo.IsName("Attack") && animInfo.normalizedTime < 1)
        {
            canMove = false;
        }
        else if(currentMode == ControllerMode.BattleMode)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
        */

    }

    void FixedUpdate()
    {
        if(direction.magnitude >= 0.1f && canMove == true)
        {
            Move();
        }
    }
    
    public void SetControllerMode(string scene)
    {
        switch (scene)
        {
            case string b when b.Contains("Battle"):
                currentMode = ControllerMode.BattleMode;
                _animator.SetBool("InBattle", true);
                break;
            default:
                currentMode = ControllerMode.OverWorldMode;
                _animator.SetBool("InBattle", false);
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
        _rb.MovePosition(this.transform.position + moveDir * currentSpeed * Time.fixedDeltaTime);

    }


    public void DoTakeHitAnimation()
    {
        _animator.SetTrigger("Hit");
    }

    private void OnApplicationQuit()
    {
        inventory.skillInventory.Clear();
        party.enemyParty.Clear();
    }
    
}
