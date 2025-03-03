using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Unity.VisualScripting;

public class PlayerMove : MonoBehaviour
{
    // Serialized fields
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] [Range(0.5f, 10)] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] Transform moveDirectionReference = null;
    [SerializeField] float jumpForce = 8f;
    [SerializeField] ColliderCheck groundChecker = null;
    [SerializeField] Animator animator = null;
    [SerializeField] float accelerationFactor = 9f;
    [SerializeField] RotateWithParent playerRotator = null;
    [SerializeField] FlashlightWeightControl flashlightWeight = null;
    [SerializeField] float runStaminaConsumptionRate = 1f;
    [SerializeField] float jumpStaminaConsumption = 2f;
    [SerializeField] float staminaRecoveryRate = 1f;
    [SerializeField] float staminaDepletionPenaltyTime = 3f;
    [SerializeField] UnityEvent onJump = null;
    [SerializeField] GameObject flashlight = null;
    public bool isFlashlightOn = true;

    float breathTime = 0f;
    float forwardValue = 0f;
    float sidewaysValue = 0f;
    public float runMix = 0f; 

    public static PlayerMove instance;

    private void Awake()
    {
        instance = this;
        // Load data
        // Current health should be equal to saved health
        SaveManager.instance.playerData.hp = SaveManager.instance.playerData.saveHP;
        // Current items should be equal to saved items
        SaveManager.instance.playerData.itemList = SaveManager.instance.playerData.saveItemList;
        // Save once after entering the level
        SaveManager.instance.Save();
    }

    [SerializeField] SayStuff throwableHint = null;
    bool hasShownThrowableHint = false;

    public void OnItemChange()
    {
        // If currently holding throwable and hint has not been shown, show it
        if (SaveManager.instance.playerData.HasItem(jarID) && !hasShownThrowableHint)
        {
            hasShownThrowableHint = true;
            SaySystem.instance.StartSay(throwableHint);
        }
    }

    Vector3 spawnPoint = Vector3.zero;
    // Initialization (not game initialization, but individual initialization)
    void Start()
    {
        SaveManager.instance.OnItemChangeEvent += OnItemChange;

        // Try to find an object named "RevivalPosition" in the game scene
        GameObject revivalPosition = GameObject.Find("RevivalPosition");
        // If found, use that position
        if(revivalPosition != null)
        {
            spawnPoint = revivalPosition.transform.position;
        }
        else
        {
            spawnPoint = this.transform.position;
        }
    }

    private void OnDisable()
    {
        SaveManager.instance.OnItemChangeEvent -= OnItemChange;
    }

    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] KeyCode throwKey = KeyCode.F;
    [SerializeField] int jarID = 3;
    [SerializeField] KeyCode flashlightKey = KeyCode.R;
    
    float lastThrowTime = 0;

    // Called every frame
    void Update()
    {
        // Toggle flashlight
        if (Input.GetKeyDown(flashlightKey))
        {
            isFlashlightOn = !isFlashlightOn;
            flashlight.SetActive(isFlashlightOn);
        }

        if (Input.GetKeyDown(throwKey) && Time.time > lastThrowTime + 2.5f && SaveManager.instance.playerData.HasItem(jarID) && !isDown)
        {
            lastThrowTime = Time.time;
            animator.SetTrigger("Throw");
        }

        if (Input.GetKey(KeyCode.LeftShift) && Time.time > breathTime)
        {
            // Run mix will gradually move towards 1
            runMix = Mathf.Lerp(runMix, 1f, Time.deltaTime * 5f);
        }
        else if(Input.GetKey(crouchKey))
        {
            runMix = Mathf.Lerp(runMix, -1, Time.deltaTime * 5f);
        }
        else
        {
            // Run mix will gradually move towards 0
            runMix = Mathf.Lerp(runMix, 0f, Time.deltaTime * 5f);
        }
        // Set run mix for animator
        animator.SetFloat("sp", runMix);

        // Input phase
        float rawInputForward = Input.GetAxisRaw("Vertical");
        float rawInputSideways = Input.GetAxisRaw("Horizontal");

        float animInputForward = animator.GetFloat("animWS");
        float animInputSideways = animator.GetFloat("animAD");
        float animControlPercentage = animator.GetFloat("animP");

        // Determine which value to use based on animation control percentage
        rawInputForward = Mathf.Lerp(rawInputForward, animInputForward, animControlPercentage);
        rawInputSideways = Mathf.Lerp(rawInputSideways, animInputSideways, animControlPercentage);

        // If in dialogue or down, reset player or animation input
        if (SaySystem.instance.isPlay || isDown)
        {
            rawInputForward = 0f;
            rawInputSideways = 0f;
        }

        // Buffer WSAD
        forwardValue = Mathf.MoveTowards(forwardValue, rawInputForward, Time.deltaTime * accelerationFactor);
        sidewaysValue = Mathf.MoveTowards(sidewaysValue, rawInputSideways, Time.deltaTime * accelerationFactor);

        // Write the buffered WSAD values to the animator
        animator.SetFloat("ws", forwardValue);
        animator.SetFloat("ad", sidewaysValue);

        Vector3 moveAmount;
        moveAmount.x = sidewaysValue;
        moveAmount.z = forwardValue;
        moveAmount.y = 0f;

        // Convert local coordinates to world coordinates
        moveAmount = moveDirectionReference.TransformDirection(moveAmount);

        float speed = 0f;
        if(runMix > 0f)
        {
            // Mix between walk speed and run speed
            speed = Mathf.Lerp(walkSpeed, runSpeed, runMix);
        }
        else
        {
            // Convert range from 1~0 to 0~1
            speed = Mathf.Lerp(walkSpeed, runSpeed, runMix + 1f);
        }

        // Multiply by walk speed
        moveAmount = Vector3.ClampMagnitude(moveAmount, 1f) * speed;

        // Set Y value to the current Y value before modifying rigid body
        moveAmount.y = rigidBody.velocity.y;

        // Set the rigid body's velocity to the move amount (world coordinates)
        rigidBody.velocity = moveAmount;

        // Check if space key is pressed
        bool isSpacePressed = Input.GetKeyDown(KeyCode.Space);

        // If space key is pressed, ground checker detects something, and not down
        if (isSpacePressed && groundChecker.yes && !SaySystem.instance.isPlay && Time.time > breathTime && !isDown)
        {
            Vector3 currentVelocity = rigidBody.velocity;
            // Set Y value to jump force
            currentVelocity.y = jumpForce;
            // Set the modified velocity back to the rigid body
            rigidBody.velocity = currentVelocity;
            SaveManager.instance.playerData.pw -= jumpStaminaConsumption;

            onJump.Invoke();
        }

        // If forward value plus sideways value is greater than 0.1 or ground checker determines not on the ground
        if (Mathf.Abs(forwardValue) + Mathf.Abs(sidewaysValue) > 0.1f || !groundChecker.yes)
        {
            playerRotator.shouldFollow = true;
        }
        else
        {
            playerRotator.shouldFollow = false;
        }

        // If holding an item, 100% follow the camera
        if (flashlightWeight.shouldRaiseHand == true)
        {
            playerRotator.shouldFollow = true;
        }

        // If run mix is greater than 0.1 and (absolute value of moveAmount's X is greater than 0.5 or absolute value of moveAmount's Z is greater than 0.5)
        if (runMix > 0.1f && (Mathf.Abs(moveAmount.x) > 0.5f || Mathf.Abs(moveAmount.z) > 0.5f))
        {
            SaveManager.instance.playerData.pw -= Time.deltaTime * runStaminaConsumptionRate;
            // If stamina is depleted, set penalty time to future seconds
            if (SaveManager.instance.playerData.pw <= 0f) 
            {
                breathTime = Time.time + staminaDepletionPenaltyTime;
            } 
        }
        else
        {
            if (SaveManager.instance.playerData.pw < SaveManager.instance.playerData.maxPw) 
            {
                SaveManager.instance.playerData.pw += Time.deltaTime * staminaRecoveryRate;
            }  
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GetKnockedOut();
        }

        // Press space to get up
        if (canGetUp == true && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("GetUp");
            Invoke("StandUpFully", 1.5f);
            canGetUp = false;
        }
    }

    void StandUpFully()
    {
        isDown 
	}


    [SerializeField] Animator knockoutTransition = null;
	public bool isDown = false;
	bool canGetUp = false;

	public void GetKnockedOut()
	{
		// Prevent multiple knockouts
		if (isDown)
			return;

		isDown = true;

		if (SaveManager.instance.playerData.hp <= 1)
		{
			animator.SetTrigger("Hit");
			knockoutTransition.SetTrigger("Dead");
			Invoke("ReturnToMainMenu", 5f);
		}
		else
		{
			animator.SetTrigger("Hit");
			knockoutTransition.SetTrigger("Play");
			// Execute teleport after a delay of two seconds
			Invoke("TeleportToSpawnPoint", 2f);
			Invoke("PrepareToGetUp", 6f);
			// Deduct one point of health
			SaveManager.instance.playerData.hp -= 1;
		}
	}

	void TeleportToSpawnPoint()
	{
		this.transform.position = spawnPoint;
	}

	[SerializeField] SayStuff reviveDialogue = null;

	void PrepareToGetUp()
	{
		canGetUp = true;
		SaySystem.instance.StartSay(reviveDialogue);
	}

	void ReturnToMainMenu()
	{
		SceneManager.LoadScene(0);
	}
