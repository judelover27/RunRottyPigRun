using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class PlatformLauncher : StructureObject
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

    public override void OnInteract()
    {
        if (isPlayerOnPlatform && playerRigidbody != null)
        {
            LaunchPlayer();
        }
    }


    private void LaunchPlayer()
    {
        playerRigidbody.velocity = Vector3.zero;
        CharacterManager.Instance.Player.controller.canMove = false;
        StartCoroutine(CoroutineLaunch());
        StartCoroutine(DisableMovementForSeconds(3f)); // 

        isPlayerOnPlatform = false;
    }

    IEnumerator CoroutineLaunch()
    {
        yield return new WaitForSeconds(0.1f);
        CharacterManager.Instance.Player.controller.canMove = false;
        playerRigidbody.AddForce((launchDirection+transform.forward).normalized * launchForce, ForceMode.Impulse);
        Debug.LogError($"error{playerRigidbody.velocity}");

    }

    IEnumerator DisableMovementForSeconds(float duration)
    {
        
        yield return new WaitForSeconds(duration);
        CharacterManager.Instance.Player.controller.canMove = true;
    }
}
