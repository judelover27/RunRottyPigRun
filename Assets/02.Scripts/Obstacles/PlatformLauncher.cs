using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformLauncher : MonoBehaviour
{
    [Header("Launcher Settings")]
    public Vector3 launchDirection = Vector3.up;
    public float launchForce = 30f;
    public float launchDelay = 2f;
    public bool autoLaunch = true;

    private bool isPlayerOnPlatform = false;
    private Rigidbody playerRigidbody;
    private Coroutine launchCoroutine;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;
            playerRigidbody = other.GetComponent<Rigidbody>();

            if (autoLaunch)
            {
                if (launchCoroutine != null)
                    StopCoroutine(launchCoroutine);
                launchCoroutine = StartCoroutine(AutoLaunch());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
            playerRigidbody = null;

            if (launchCoroutine != null)
                StopCoroutine(launchCoroutine);
        }
    }

    IEnumerator AutoLaunch()
    {
        yield return new WaitForSeconds(launchDelay);
        if (isPlayerOnPlatform && playerRigidbody != null)
        {
            LaunchPlayer();
        }
    }

    public void OnLaunchInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isPlayerOnPlatform && playerRigidbody != null)
        {
            LaunchPlayer();
        }
    }

    private void LaunchPlayer()
    {
        playerRigidbody.velocity = Vector3.zero; // 기존 속도 초기화
        playerRigidbody.AddForce((launchDirection + CharacterManager.Instance.Player.transform.forward).normalized * launchForce, ForceMode.Impulse);
        isPlayerOnPlatform = false;
    }
}

