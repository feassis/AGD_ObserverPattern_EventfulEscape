using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollFollowEvent : MonoBehaviour
{
    [SerializeField] private Transform dollToRotate;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float giggleCoolDown = 1f;
    [SerializeField] private int keysToDisapearOnDoorOpened = 4;

    private Transform player;
    private Camera cam;
    private float giggleTimer = 0;

    private void OnEnable()
    {
        EventService.Instance.OnDollRotationStart.AddListener(CheckIfThisDollShouldBeDisabled);
    }

    private void OnDisable()
    {
        if(player != null)
        {
            player = null;
            cam = null;
            EventService.Instance.OnDollRotationEnded.InvokeEvent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if(other.TryGetComponent<PlayerView>(out PlayerView playerView))
        {
            player = playerView.transform;
            cam = playerView.PlayerCamera;
            EventService.Instance.OnDollRotationStart.InvokeEvent();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerView>(out PlayerView playerView))
        {
            player = null;
            cam = null;
            EventService.Instance.OnDollRotationEnded.InvokeEvent();
        }
    }

    private void Update()
    {
        if (giggleTimer > 0)
        {
            giggleTimer -= Time.deltaTime;
        }

        if (player != null && !IsInCameraView())
        {
            RotateTowardsPlayer();

            if (giggleTimer <= 0)
            {
                GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.SpookyGiggle);
                giggleTimer = giggleCoolDown;
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        if (player == null) return;

        // Calculate target position with the same y-coordinate as the object
        Vector3 targetPosition = new Vector3(player.position.x, dollToRotate.position.y, player.position.z);

        // Calculate the direction to the target
        Vector3 direction = (targetPosition - dollToRotate.position).normalized;

        // Create a rotation to face the target
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target
        dollToRotate.rotation = Quaternion.Slerp(dollToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private bool IsInCameraView()
    {
        // Convert the object's position to viewport coordinates
        Vector3 viewportPoint = cam.WorldToViewportPoint(dollToRotate.position);

        // Check if the object is outside the camera's view
        // If x or y is less than 0 or greater than 1, the object is off-screen
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0;
    }

    private void CheckIfThisDollShouldBeDisabled()
    {
        int keys = GameService.Instance.GetPlayerController().KeysEquipped;

        if(keys == keysToDisapearOnDoorOpened)
        {
            gameObject.SetActive(false);
        }
    }
}
