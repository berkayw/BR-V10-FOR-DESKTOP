using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovementManager : MonoBehaviourPunCallbacks
{
    private CharacterController characterController;
    
    //public VariableJoystick joystick;
    //public GameObject joyStickBackground;
    
    public float movementSpeed;
    public float rotationSpeed;
    
    //Jump
    public float jumpForce = 2f;  
    public float gravity = -10f;
    public Vector3 velocity;
    private bool isGrounded;
    public float airControlFactor = 1f; // Havada kontrol ne kadar olmalı
    
    public string gamePlatform;
    
    //INPUTS
    public Vector2 pcInput;
    public Vector2 mobileInput;
    
    public Animator playerAnimator;
    private Camera camera;

    //public CinemachineBrain cinemachineBrain;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        camera = Camera.main;
        
        //Access cinemachine and configure
        /*cinemachineBrain = CinemachineCore.Instance.GetActiveBrain(0);
        cinemachineBrain.ActiveVirtualCamera.Follow = transform;
        cinemachineBrain.ActiveVirtualCamera.LookAt = transform;*/
    }
    
   /* private void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }
        
        //CheckGamePlatform();
        gamePlatform = "pc";
        HandleInputs();
        
        switch(gamePlatform) 
        {
            case "pc":
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
                HandleMovement(pcInput.x, pcInput.y);
                break;
            
            /*case "mobile":
                HandleMovement(joystick.Direction.x, joystick.Direction.y);
                break;
        }
    }*/

   private void Update()
   {
       if (!photonView.IsMine)
       {
           return;
       }
       
       HandleInputs();

       isGrounded = characterController.isGrounded;

       if (isGrounded && velocity.y < 0)
       {
           velocity.y = -2f; // Zıpladıktan sonra yere yapışmasını sağlamak
       }
       
       if (isGrounded)
       {
           HandleMovement(pcInput.x, pcInput.y);

           if (Input.GetButtonDown("Jump"))
           {
               Jump();
           }
       }
       else
       {
           HandleAirMovement(pcInput.x, pcInput.y); // Havada hareket
       }

       ApplyGravity();
   }
    
    public void HandleInputs()
    {
        pcInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        pcInput.Normalize();
        
        /*mobileInput = new Vector2(joystick.Direction.x, joystick.Direction.y);
        mobileInput.Normalize();*/
    }
    
    /*public void HandleMovement(float inputX, float inputY) //without jump ability
    {
        Vector3 movementDirection = camera.transform.forward * inputY + camera.transform.right * inputX;
        movementDirection.y = 0f;
        
        characterController.SimpleMove(movementDirection * movementSpeed);

        if (movementDirection.magnitude <= 0)
        {
            playerAnimator.SetBool("run", false);
            return;
        }
            
        playerAnimator.SetBool("run", true);
        Vector3 targetDirection = Vector3.RotateTowards(characterController.transform.forward, movementDirection, rotationSpeed * Time.deltaTime, 0f);

        characterController.transform.rotation = Quaternion.LookRotation(targetDirection);
    }
    
    public void HandleMovement(float inputX, float inputY)
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Yere sağlam basmak için bir küçük kuvvet uygula
        }

        Vector3 movementDirection = camera.transform.forward * inputY + camera.transform.right * inputX;
        movementDirection.y = 0f;
        characterController.SimpleMove(movementDirection * movementSpeed);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);
            playerAnimator.SetTrigger("jump");
        }

        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (movementDirection.magnitude <= 0)
        {
            playerAnimator.SetBool("run", false);
            return;
        }

        playerAnimator.SetBool("run", true);
        Vector3 targetDirection = Vector3.RotateTowards(characterController.transform.forward, movementDirection, rotationSpeed * Time.deltaTime, 0f);
        characterController.transform.rotation = Quaternion.LookRotation(targetDirection);
    }*/

    
    public void HandleMovement(float inputX, float inputY)
    {
        Vector3 movementDirection = camera.transform.forward * inputY + camera.transform.right * inputX;
        movementDirection.y = 0f;

        characterController.SimpleMove(movementDirection * movementSpeed);

        if (movementDirection.magnitude <= 0)
        {
            playerAnimator.SetBool("run", false);
            return;
        }

        playerAnimator.SetBool("run", true);
        Vector3 targetDirection = Vector3.RotateTowards(characterController.transform.forward, movementDirection, rotationSpeed * Time.deltaTime, 0f);
        characterController.transform.rotation = Quaternion.LookRotation(targetDirection);
    }

    public void HandleAirMovement(float inputX, float inputY)
    {
        Vector3 movementDirection = camera.transform.forward * inputY + camera.transform.right * inputX;
        movementDirection.y = 0f;

        if (movementDirection.magnitude > 0f)
        {
            // Havada rotasyonu da ekliyoruz
            Vector3 targetDirection = Vector3.RotateTowards(characterController.transform.forward, movementDirection, rotationSpeed * Time.deltaTime, 0f);
            characterController.transform.rotation = Quaternion.LookRotation(targetDirection);
        }

        characterController.Move(movementDirection * (movementSpeed * airControlFactor) * Time.deltaTime);
    }

    public void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        //playerAnimator.SetTrigger("jump");
    }

    public void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    
    public void CheckGamePlatform() //enable for cross platform, must be can check device type then choose.
    {
        /*if (joyStickBackground.active)
        {
            gamePlatform = "mobile";
        }
        else*/
        {
            gamePlatform = "pc";
        }
    }

    
    
}
