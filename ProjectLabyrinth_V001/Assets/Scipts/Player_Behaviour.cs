using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Behaviour : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float runSpeed = 9f;
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

    private Animator _animator;
    private bool isAttacking;
    private bool canMove;

    public Camera cam;
    public float effectTime;

    private Vector3 direction;

    public InventorySO inventory;  //I can make this private
    public GameObject playerHUD;

    private enum ControllerMode{BattleMode,OverWorldMode,};
    private ControllerMode currentMode;

    void Awake()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "Battle":
                currentMode = ControllerMode.BattleMode;
                break;
            case "BossBattle":
                currentMode = ControllerMode.BattleMode;
                break;
            default:
                currentMode = ControllerMode.OverWorldMode;
                break;
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _animator = GetComponentInChildren<Animator>();
        _ctrl = GetComponent<CharacterController>();
        cam = Camera.main;
        //2- Find and return GameBehavior script attached to Game Manager object in scene

        currentSpeed = moveSpeed;
    }

    void Update()
    {
        float _vInput = Input.GetAxisRaw("Vertical");
        float _hInput = Input.GetAxisRaw("Horizontal");
        direction = new Vector3(_hInput, 0f, _vInput).normalized;


        //Character Movement and Animation Controller                          
        //Check if the player is trying to move
//--------------------------------------------------------------Movement Detection + Movement Mechanics----------------------------------------------------//
        if (direction.magnitude >= 0.1f && canMove == true)
        {

            //DodgeRolling
            bool roll = Input.GetButtonDown("Roll");

            if (roll)
            {
                DodgeRoll();
            }

            //Check if the player is trying to run - Player Running -
            if (Input.GetKey(KeyCode.LeftShift))
            {

                currentSpeed = runSpeed;

                _animator.SetBool("walking?", false);
                _animator.SetBool("running?", true);
            }  
            else
            {

                _animator.SetBool("running?", false);
                _animator.SetBool("walking?", true);
                currentSpeed = moveSpeed;
              
            }
        }
        else
        {
            _animator.SetBool("walking?", false);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _animator.SetTrigger("attack");
        }
        */
        

//---------------------------------------------------------------Animator States-----------------------------------------------------//
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

//--------------------------------------------------------------Player Mechanics----------------------------------------------------//
        
        if(currentMode != ControllerMode.BattleMode)
        {
            //Open Inventory Player Button - I -
            if (Input.GetKeyDown(KeyCode.I))
            {
                OpenInventory();
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
        }
    }

    void FixedUpdate()
    {
        if(direction.magnitude >= 0.1f && canMove == true)
        {
            Move();
        }
    }
    

    void DodgeRoll()
    {

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


    public void UpdateInventory()
    { 
  
        //inventory.fillInventory(skill, 1);
    }


    public void OpenInventory()
    {
        playerHUD.SetActive(!playerHUD.activeSelf);
    }

    
    private bool IsGrounded()
    {
        Vector3 endPosition = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        return Physics.CheckCapsule(_col.bounds.center, endPosition, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);
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

    private void OnApplicationQuit()
    {
        inventory.skillInventory.Clear();
    }
}
