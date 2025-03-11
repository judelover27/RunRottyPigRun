using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float runStamina = 20f;
    public float jumpStamina = 20f;
    public bool isMove;
    public bool isRun;
    public bool isFloat;
    private bool isJumping;
    public bool canMove = true;
    private Vector2 curMovementInput;
    public float jumpPower = 100f;
    public LayerMask groundLayerMask;
    private CapsuleCollider capsuleCollider;

    [Header("Climb")]
    public float climbSpeed = 3f;
    public float wallDetectRange = 1f;
    public float climbDuration = 1.5f;
    public float headOffset = 0.3f;
    public LayerMask wallLayer;
    public Transform head;

    private bool isClimbing = false;
    private Coroutine climbCoroutine;


    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    [Range(-30f, 30f)] public float camYRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    public Action inventory;

    public Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    private void FixedUpdate()
    {
        if (canMove)
            Move();
        else
            isMove = false;
    }

    private void Update()
    {
        CheckFloatState();
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        if (isRun && !UIManager.Instance.condition.stamina.recovering)
        {
            UIManager.Instance.condition.stamina.Subtract(Time.deltaTime * runStamina);
            dir *= runSpeed;
        }
        else
        {
            dir *= moveSpeed;
        }

        dir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, camYRot, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            curMovementInput = Vector2.zero;
            isMove = false;
            return;
        }

        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            isMove = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            isMove = false;
        }
    }

    public void OnRunInput(InputAction.CallbackContext context)
    {


        if (context.phase == InputActionPhase.Performed)
        {
            if (!UIManager.Instance.condition.stamina.recovering)
                isRun = true;
            else
                isRun = false;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRun = false;
        }

    }
    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded() && !UIManager.Instance.condition.stamina.recovering)
        {
            CharacterManager.Instance.Player.animationHandler.Jump();
            UIManager.Instance.condition.stamina.Subtract(jumpStamina);

            if (!isMove)
            {
                canMove = false;
                isJumping = true;
                StartCoroutine(Jump(.8f));

            }
            else
            {
                isJumping = true;
                StartCoroutine(Jump(0.2f));
            }
        }
    }

    public IEnumerator Jump(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        canMove = true;
        Invoke("SetJumpFalse", 2f);
    }

    void SetJumpFalse()
    {
        isJumping = false;
    }

    public void CheckFloatState()
    {
        if (Mathf.Abs(_rigidbody.velocity.y) > 1f)
        {
            isFloat = true;
            CharacterManager.Instance.Player.animationHandler.Float(isFloat);
        }

        if (isFloat)
            StartCoroutine(CoroutineIsGround());
    }

    IEnumerator CoroutineIsGround()
    {

        while (!IsGrounded())
        {
            yield return null;
        }

        isFloat = false;
        CharacterManager.Instance.Player.animationHandler.Float(isFloat);
    }

    bool IsGrounded()
    {
        if (isJumping) return false;

        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position +(transform.forward*0.2f)+(transform.up*0.01f), Vector3.down),
            new Ray(transform.position +(-transform.forward*0.2f)+(transform.up*0.01f), Vector3.down),
            new Ray(transform.position +(transform.right*0.2f)+(transform.up*0.01f), Vector3.down),
            new Ray(transform.position +(-transform.right*0.2f)+(transform.up*0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            Debug.DrawRay(rays[i].origin, rays[i].direction * (capsuleCollider.height / 2 + 0.1f), Color.red, 1f);
            Debug.Log("try jump");
            if (Physics.Raycast(rays[i], capsuleCollider.height / 2 + 0.1f, groundLayerMask))
            {
                Debug.Log("jump");
                return true;
            }
        }

        Debug.Log("no jump");
        return false;
    }

    public void OnClimbInput(InputAction.CallbackContext context)
    {
        if (IsNearWall()) // 벽 감지
        {
            Debug.Log("Climb");
            StartClimbing();
        }
    }

    private bool IsNearWall()
    {
        RaycastHit hit;
        Vector3 origin = head.position;
        Vector3 direction = transform.forward; // 캐릭터가 바라보는 방향

        // 벽 감지 Raycast (벽이 앞에 있는지 확인)
        bool wallDetected = Physics.Raycast(origin, direction, out hit, wallDetectRange, wallLayer);

        // 상단 부분 확인 (위쪽이 비어 있는지)
        Vector3 upperCheckOrigin = head.position + Vector3.up * headOffset;
        bool upperSpaceFree = !Physics.Raycast(upperCheckOrigin, direction, wallDetectRange, wallLayer);

        return wallDetected && upperSpaceFree;
    }


    private void StartClimbing()
    {
        if (!isClimbing)
        {
            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero;
            isClimbing = true;

            if (climbCoroutine != null)
                StopCoroutine(climbCoroutine);

            climbCoroutine = StartCoroutine(ClimbingCoroutine());
        }
    }

    IEnumerator ClimbingCoroutine()
    {
        float time = 0f;
        while (time < climbDuration && isClimbing)
        {
            time += Time.deltaTime;
            transform.position += Vector3.up * climbSpeed * Time.deltaTime;
            yield return null;
        }

        StopClimbing();
    }

    private void StopClimbing()
    {
        if (isClimbing)
        {
            _rigidbody.useGravity = true;
            isClimbing = false;
        }
    }

    public void ToggleCursor()
    {
        bool toggle = (Cursor.lockState == CursorLockMode.Locked);
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        canLook = true;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        canLook = false;
    }
}
