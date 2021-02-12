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
    private Vector3 lastTarget;

    // Spatial Sound Control
    public UnityEvent onPreLeftFootstep;
    public UnityEvent onLeftFootstep;
    public UnityEvent onPreRightFootstep;
    public UnityEvent onRightFootstep;

    Vector3 offset;
    public LayerMask obstacleMask;

    Coroutine movementCoroutine;
    bool movement = false;
    bool leftFootstep;
    public float movementDuration = 0.4f;
    private float movementSpeed = 2f;

    [Min(0.01f)] public float runMultiplier = 2f;

    public bool window = false;


    // Start is called before the first frame update
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

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotationX += sensitivity * mouseY;
        rotationY += sensitivity * mouseX;
        rotationX = Mathf.Clamp(rotationX, -clampAngleDegrees, clampAngleDegrees);
        mainCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);

        float running = Input.GetKey(KeyCode.LeftShift) ? runMultiplier : 1f;
        CollisionForce = WalkSpeed * running;

        characterController.SimpleMove(movementSpeed * lastTarget);

        if (movement)
        {
            movementCoroutine = StartCoroutine(FootstepSoundTrigger());
        }

        SetCursorLock();

        if (window)
        {
            Vector3 offset = windowTarget.transform.position - characterController.transform.position;
            Physics.SyncTransforms();
            offset = offset.normalized * 2f;
            characterController.SimpleMove(offset * 4f);
        }
        

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
        Movement(windowTarget.transform.position);
        lastTarget = windowTarget.transform.position;
    }
    public void TablePosition()
    {
        StartCoroutine(LerpPosition(tableTarget.transform.position, 2));
    }
    public void CatPosition()
    {
        StartCoroutine(LerpPosition(catTarget.transform.position, 2));
    }

    void Movement(Vector3 targetPosition)
    {
        //Vector3 offset = targetPosition - this.transform.position;
        //Debug.Log(offset);

        if (offset.magnitude > 0.1f)
        {
            characterController.SimpleMove(offset*0.5f);
           
            //StartCoroutine(ChangePosition(targetPosition));
        }
        else if(offset.magnitude <= 0.1f)
        {
            StopCoroutine(ChangePosition(targetPosition));
        }

        //Debug.Log(offset.magnitude);
    }

    IEnumerator ChangePosition(Vector3 targetPosition)
    {        
        Vector3 targetVector = targetPosition - transform.position;
        while (true)
        {
            characterController.SimpleMove(targetVector);
            yield return null;
            Debug.Log(targetPosition);
        }
        

        /*float distance = Vector3.Distance(targetPosition, characterController.transform.position);

        while (offset.magnitude > 0.1f)
        {
            //offset = offset * 2f;
            characterController.Move(offset * Time.deltaTime);
            //Debug.Log(offset);

            yield return null;
        }

        //transform.position = targetPosition;
        //characterController.Move(Vector3.zero * Time.deltaTime);
        */
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            movement = true;
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        movement = false;
        lastTarget = targetPosition;

    }

    IEnumerator FootstepSoundTrigger()
    {
        var startTime = Time.time;

        PreFootstepEvents();

        yield return new WaitForSeconds(0.1f);

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
