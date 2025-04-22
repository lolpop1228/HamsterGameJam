using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 90f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    [Header("Head Bobbing")]
    public float bobSpeed = 6f;
    public float bobAmount = 0.05f;
    private float defaultCamY;
    private float bobTimer = 0f;

    [Header("Sprint FOV")]
    public float defaultFOV = 60f;
    public float sprintFOV = 75f;
    public float fovSmoothSpeed = 8f;

    [Header("Footstep Audio")]
    public AudioSource footstepAudioSource;
    public AudioClip footstepClip;

    [Header("Stamina System")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 15f;
    public float staminaRegenDelay = 1.5f;
    private float regenDelayTimer = 0f;
    private bool isSprinting = false;
    public TextMeshProUGUI staminaText;

    private float baseWalkSpeed;
    private float baseRunSpeed;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        defaultCamY = playerCamera.transform.localPosition.y;
        playerCamera.fieldOfView = defaultFOV;

        currentStamina = maxStamina;

        baseWalkSpeed = walkSpeed;
        baseRunSpeed = runSpeed;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        bool sprintKey = Input.GetKey(KeyCode.LeftShift);
        bool moving = (inputX != 0 || inputY != 0);
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);

        isSprinting = sprintKey && moving && currentStamina > 0 && !isCrouching;

        float speed = canMove
            ? (isCrouching ? crouchSpeed : isSprinting ? runSpeed : walkSpeed)
            : 0;

        float curSpeedX = speed * inputY;
        float curSpeedY = speed * inputX;

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Preserve vertical velocity (falling)
        moveDirection.y = movementDirectionY;

        // Apply gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Crouch
        if (isCrouching && canMove)
        {
            characterController.height = crouchHeight;
        }
        else
        {
            characterController.height = defaultHeight;
        }

        // Move
        characterController.Move(moveDirection * Time.deltaTime);

        // Mouse look
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Head bobbing & footstep
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            float currentBobSpeed = isSprinting ? bobSpeed * 1.5f : isCrouching ? bobSpeed * 0.5f : bobSpeed;
            float currentBobAmount = isSprinting ? bobAmount * 1.5f : isCrouching ? bobAmount * 0.5f : bobAmount;

            float previousSin = Mathf.Sin(bobTimer);
            bobTimer += Time.deltaTime * currentBobSpeed;
            float newSin = Mathf.Sin(bobTimer);
            float bobOffset = newSin * currentBobAmount;

            if (newSin < 0 && previousSin >= 0)
            {
                if (footstepAudioSource != null && footstepClip != null)
                {
                    footstepAudioSource.PlayOneShot(footstepClip);
                }
            }

            Vector3 localPos = playerCamera.transform.localPosition;
            localPos.y = defaultCamY + bobOffset;
            playerCamera.transform.localPosition = localPos;
        }
        else
        {
            Vector3 localPos = playerCamera.transform.localPosition;
            localPos.y = Mathf.Lerp(localPos.y, defaultCamY, Time.deltaTime * bobSpeed);
            playerCamera.transform.localPosition = localPos;
            bobTimer = 0;
        }

        // Sprint FOV
        float targetFOV = isSprinting ? sprintFOV : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovSmoothSpeed);

        // Stamina system
        if (isSprinting)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            regenDelayTimer = 0f;
        }
        else
        {
            if (currentStamina < maxStamina)
            {
                regenDelayTimer += Time.deltaTime;
                if (regenDelayTimer >= staminaRegenDelay)
                {
                    currentStamina += staminaRegenRate * Time.deltaTime;
                    currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
                }
            }
        }

        // Stamina UI
        if (staminaText != null)
        {
            staminaText.text = Mathf.CeilToInt(currentStamina).ToString();
        }
    }
}
