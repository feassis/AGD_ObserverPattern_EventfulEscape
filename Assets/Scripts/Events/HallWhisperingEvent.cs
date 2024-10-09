using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallWhisperingEvent : MonoBehaviour
{
    [SerializeField] private float soundCooldown = 78f;

    private bool isActive = false;
    private float soundTimer = 0;

    private void Update()
    {
        if(soundTimer > 0)
        {
            soundTimer -= Time.deltaTime;
        }

        if (isActive)
        {
            if(soundTimer <= 0)
            {
                GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.WhispetingSounds);
                soundTimer = soundCooldown;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerView>(out PlayerView playerView))
        {
            isActive = true;
            EventService.Instance.OnWhisperingStart.InvokeEvent();
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerView>(out PlayerView playerView))
        {
            isActive = false;
            EventService.Instance.OnWhisperingEnded.InvokeEvent();
        }
    }
}
