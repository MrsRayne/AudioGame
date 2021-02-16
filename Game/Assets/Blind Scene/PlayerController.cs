using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Characters.FirstPerson;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IGrounded, IMovementSpeed, ICollisionForce
{
    public Camera mainCamera;
    private CharacterController characterController = null;

    public float WalkSpeed => 4f;

    public bool Grounded => isGrounded;
    bool isGrounded;

    public float CollisionForce { get; private set; }
    public RigidbodyFirstPersonController.AdvancedSettings advancedSettings = new RigidbodyFirstPersonController.AdvancedSettings();
    public UnityEvent onLand = default;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    // Camera rotation sensitivity.
    private const float sensitivity = 2.0f;

    // Maximum allowed vertical rotation angle in degrees.
    private const float clampAngleDegrees = 80.0f;

    // Position 
    [SerializeField] GameObject windowTarget;
    [SerializeField] GameObject tableTarget;
    [SerializeField] GameObject catTarget;

    [SerializeField] GameObject birdSounds;
    [SerializeField] GameObject CatSound;

    public bool window = false;
    public bool table = false;
    public bool cat = false;


    Vector3 move;
    Vector3 target;

    // Spatial Sound Control
    public UnityEvent onPreLeftFootstep;
    public UnityEvent onLeftFootstep;
    public UnityEvent onPreRightFootstep;
    public UnityEvent onRightFootstep;

    public LayerMask obstacleMask;

    Coroutine movementCoroutine;
    public bool movement = false;
    bool leftFootstep;
    public float movementDuration = 0.4f;
    private float movementSpeed = 2f;


    

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Vector3 rotation = mainCamera.transform.localRotation.eulerAngles;
        rotationX = rotation.x;
        rotationY = rotation.y;
    }

    void FixedUpdate()
    {
        isGrounded = true;

    }

    void LateUpdate()
    {
        // Rotation

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotationX += sensitivity * mouseY;
        rotationY += sensitivity * mouseX;
        rotationX = Mathf.Clamp(rotationX, -clampAngleDegrees, clampAngleDegrees);
        mainCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);

        CollisionForce = WalkSpeed;

        

        SetCursorLock();

        Physics.SyncTransforms();

        // Change Position

        if (window)
        {
            target = windowTarget.transform.position;

            Vector3 offset = target - characterController.transform.position;
            float distance = Vector3.Distance(characterController.transform.position, target);
            offset = offset.normalized * 2f;
            Physics.SyncTransforms();

            movement = true;

            if (distance > 0.1f)
            {
                move = offset * 5f;
            }
            else if(distance <= 0.1f)
            {
                move = Vector3.zero;
                movement = false;
                window = false;
            }            
        }
        if (table)
        {
            target = tableTarget.transform.position;
            movement = true;

            Vector3 offset = target - characterController.transform.position;
            float distance = Vector3.Distance(characterController.transform.position, target);
            offset = offset.normalized * 2f;
            Physics.SyncTransforms();

            movement = true;

            if (distance > 0.1f)
            {
                move = offset * 4f;
            }
            else if (distance <= 0.1f)
            {
                move = Vector3.zero;
                movement = false;
                table = false;
            }
        }
        if (cat)
        {
            target = catTarget.transform.position;
            movement = true;

            Vector3 offset = target - characterController.transform.position;
            float distance = Vector3.Distance(characterController.transform.position, target);
            offset = offset.normalized * 2f;
            Physics.SyncTransforms();

            movement = true;

            if (distance > 0.1f)
            {
                move = offset * 4f;
            }
            else if (distance <= 0.1f)
            {
                move = Vector3.zero;
                movement = false;
                cat = false;
            }
        }
        
        // Movement

        if (movement)
        {
            movementCoroutine = StartCoroutine(FootstepSoundTrigger());
        }

        characterController.SimpleMove(move);
    }

    
    // Sets the cursor lock for first-person control.
    private void SetCursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Change Position
    public void WindowPosition()
    {
        window = true;
    }
    public void TablePosition()
    {
        table = true;
    }
    public void CatPosition()
    {
        cat = true;
    }
    public void PetCat()
    {
        CatSound.GetComponents<AudioSource>()[1].Play();
    }
    public void OpenWindow()
    {
        birdSounds.SetActive(true);
    }
    public void CloseWindow()
    {
        birdSounds.SetActive(false);
    }
    IEnumerator FootstepSoundTrigger()
    {
        var startTime = Time.time;

        PreFootstepEvents();

        yield return new WaitForSeconds(1f);

        FootstepEvents();
        movementCoroutine = null;
    }

    void PreFootstepEvents()
    {
        if (leftFootstep)
        {
            onPreLeftFootstep?.Invoke();
        }
        else
        {
            onPreRightFootstep?.Invoke();
        }
    }

    void FootstepEvents()
    {
        if (leftFootstep)
        {
            onLeftFootstep?.Invoke();
        }
        else
        {
            onRightFootstep?.Invoke();
        }
        leftFootstep = !leftFootstep;
    }
}
