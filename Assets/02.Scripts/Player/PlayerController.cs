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
    public bool isMove;
    public bool isRun;
    private bool isJumping;
    public bool canMove = true;
    private Vector2 curMovementInput;
    public float jumpPower = 100f;
    public LayerMask groundLayerMask;
    private CapsuleCollider capsuleCollider;

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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if(canMove)
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        if (isRun)
            dir *= runSpeed;
        else
            dir *= moveSpeed;

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
            isRun = true;
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
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            CharacterManager.Instance.Player.animationHandler.Jump();

            if (!isMove)
            {
                canMove = false;
                isJumping = true;
                StartCoroutine(Jump(1f));

            }
            else
            {
                isJumping = true;
                StartCoroutine(Jump(0.5f));
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

    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = (Cursor.lockState == CursorLockMode.Locked);
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
