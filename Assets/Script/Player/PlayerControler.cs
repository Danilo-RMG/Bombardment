using System.Collections;
using System.Collections.Generic;
using System.Data;
using Cinemachine;

using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    // State Machine
   [HideInInspector] public StateMachine stateMachine;
   [HideInInspector] public Dead deadState;
   [HideInInspector] public Idle idleState;
   [HideInInspector] public Jump jumpState;
   [HideInInspector] public Walking walkingState;
   [HideInInspector] public Attack attackState;

    // Internal Properties
   [HideInInspector] public Rigidbody MyRigid;
   [HideInInspector] public Collider MyCollider;
   [HideInInspector] public Animator thisAnimator;
   [HideInInspector] public Vector2 movementVector;
   [HideInInspector] public Vector2 mouseVector;
   [HideInInspector] public bool isGrounded = false;

    // Movement controls props
  [Header("MovementControls")]
   public float Speed;
   public float speedDamping = 0.1f;
   [HideInInspector] public float maxSpeed = 5f;
   [HideInInspector] public bool hasJumpInput;
   [HideInInspector]  public bool hasJumped = false;
   public float Jumpforce = 3f;
   public float JumpMovementFactor = 0.3f;
  

    // Cam controls props
   [HideInInspector] public bool rotateOnMove;
  [Header("CameraControls")]
   public GameObject camRot;
   public CinemachineVirtualCamera normalCam;
   public CinemachineVirtualCamera DeadCam;
   public CinemachineImpulseSource source;
   public float sensX = 100;
   public float sensY = 100;
   private float xRot;
   private float yRot;

    // Attack props
   [HideInInspector] public bool attacking;
   [HideInInspector] public bool hasAttacked = false;
   [HideInInspector] public float attackCooldown = 0;
   [HideInInspector] public float attackCooldownSave = 0.4f;
  [Header("Attack")]
   public GameObject slash;
   public float attackRange = 0.5f;
   public AudioSource whoosh;
   public AudioSource audioClip;

  // VFX
  [Header("VFX")]
   public GameObject VFX_Impact;
   [HideInInspector] public bool effectActive = false;

  [Header("Knockback")]
  // Knockback props
   public float knockbackForce = 5f;
 
  void Awake()
   {
    MyRigid = GetComponent<Rigidbody>();
    MyCollider = GetComponent<Collider>();
    thisAnimator = GetComponent<Animator>();
    source = GetComponent<CinemachineImpulseSource>();
    audioClip = GetComponent<AudioSource>();
   }

  void Start()
   {
    // Get states
     stateMachine = new StateMachine();
     deadState = new Dead(this);
     idleState = new Idle(this);
     walkingState = new Walking(this);
     jumpState = new Jump(this);
     attackState = new Attack(this);
     stateMachine.ChangeState(idleState);
    // Lock the camera and hide cursor
     //Cursor.lockState = CursorLockMode.Locked;
     //Cursor.visible = false;
   }

  void Update()
   {
    // Check game over
     if(GameManager.Instance.isGameOver)
       {
        if(stateMachine.currentStateName != deadState.name)
         {
          stateMachine.ChangeState(deadState);
           return;
         }
       }
    // Check game pause
     if(GameManager.isPaused)
      {
        return;
      }
    // Create input vector
     Vector2 playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
      float X = Mathf.Min(playerInput.x, maxSpeed);
      float Y = Mathf.Min(playerInput.y, maxSpeed);
       movementVector = new Vector2(X,Y);
    // Create mouse vector
     Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
      mouseVector = new Vector2(mouseInput.x, mouseInput.y);
    // Create jump input
     if(!hasJumped)
      {
       if(Input.GetKey(KeyCode.Space) && isGrounded)
        {
         hasJumpInput = true;
         hasJumped = true;
        }
         else
          {
           hasJumpInput = false;
          }
      }
    // Create attack input
    if(!hasAttacked)
     {
      if(Input.GetMouseButton(0) && attackCooldown == 0)
       {
        attacking = true;
        hasAttacked = true;
       }
     }

    // Control camera
     CamRotation();
    // Detect ground
     DetectGround();
    // Detect speed
     playerSpeed();
    // Check attack cooldown
     attackCooldownReset();
    stateMachine.Update();
   }
  void LateUpdate()
   {
    stateMachine.LateUpdate();
   }
  void FixedUpdate()
   {
    stateMachine.FixedUpdate();
   }
  public Quaternion GetForward()
   {
    Camera cam = Camera.main;
     float eulerY = cam.transform.eulerAngles.y;
      return Quaternion.Euler(0, eulerY, 0);
   }
  public void CamRotation()
   {
    if(!GameManager.Instance.isGameOver && !GameManager.isPaused)
     {
      if(rotateOnMove)
       {
        // Calculate vector with sensitivy
         float mouseX = mouseVector.x * sensX;
         float mouseY = mouseVector.y * sensY;
          yRot += mouseX;
          xRot -= mouseY;
          xRot = Mathf.Clamp(xRot, -13f, 50f);
        // Rotate cam and orientation x,y -angle
         camRot.transform.rotation = Quaternion.Euler(xRot, yRot, 0);
       }
        else
         {
          // Calculate vector with sensitivy
           float mouseX = mouseVector.x * sensX;
           float mouseY = mouseVector.y * sensY;
            yRot += mouseX;
            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -13f, 50f);
          // Rotate cam and orientation x,y -angle
           camRot.transform.rotation = Quaternion.Euler(xRot, yRot, 0);
            transform.rotation = Quaternion.Euler(0, yRot, 0);
         }
     }
   }
  public void RotateBodyToFaceInput()
   {
    if(movementVector.IsZero()) return;
     Camera cam = Camera.main;
     Vector3 InputVector = new Vector3(movementVector.x, 0f, movementVector.y);
      Quaternion q1 = Quaternion.LookRotation(InputVector, Vector3.up);
      Quaternion q2 = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
       Quaternion toRatation = q1 * q2;
        Quaternion newRotation = Quaternion.LerpUnclamped(transform.rotation, toRatation, 0.15f);
            
        MyRigid.MoveRotation(newRotation);
   }
  public void DetectGround()
   {
    //Reset flag
     isGrounded = false;
    // Detect ground
     Vector3 origin = transform.position + Vector3.up * 0.2f;
     Vector3 direction = Vector3.down;
      Bounds bounds = MyCollider.bounds;
       float radius = bounds.size.x * 0.2f;
       float maxDistance = bounds.size.y * 0.25f;
     if(Physics.SphereCast(origin, radius, direction, out var hitInfo, maxDistance))
      {
       GameObject hitObject = hitInfo.transform.gameObject;
        if(hitObject.CompareTag("Platform"))
         {
          isGrounded = true;
         }
      }
   }
  public void playerSpeed()
   {
    // Update Animator
     float velocity = MyRigid.velocity.magnitude;
     float velocityRate = velocity / Speed;
      thisAnimator.SetFloat("fSpeed", velocityRate);
   }
  public void attackCooldownReset()
   {
    if(attackCooldown > 0)
     {
      attackCooldown -= Time.deltaTime;
       if(attackCooldown < 0)
        {
         attackCooldown = 0;
        }
     }
   }
  public void PerformAttack()
   {
    // Define the attack origin and direction
     Vector3 attackOrigin = transform.position;
     Vector3 attackDirection = transform.forward;
    // Use a sphere cast to detect targets within range
     RaycastHit[] hits = Physics.SphereCastAll(attackOrigin, attackRange, attackDirection, 0f);
      foreach (RaycastHit hit in hits)
       {
        // Access the hit object's GameObject
         GameObject hitObject = hit.collider.gameObject;
          if (hitObject != null)
           {
            if(hitObject.CompareTag("Bomb")) 
             {
               Transform bombTransform = hitObject.transform;
               Rigidbody bombRigid = hitObject.GetComponent<Rigidbody>();
              // Calculate impulseVector
               Vector3 impulseDirection = (hitObject.transform.position - transform.position).normalized;
               impulseDirection = impulseDirection * knockbackForce;
              // Apply force
               bombRigid.AddForce(impulseDirection, ForceMode.Impulse);
              // Create effects
               if(!effectActive)
                {
                 source.GenerateImpulse(Camera.main.transform.forward);
                 Instantiate(VFX_Impact, hitObject.transform.position, bombTransform.rotation); 
                  audioClip.Play();
                  effectActive = true;
                }
           }
        }
        }
    attackCooldown = attackCooldownSave;
   }
   
  /*void OnGUI()
   {
    GUI.Label(new Rect(5, 5, 100, 50), stateMachine.currentStateName);
   }*/
}
 