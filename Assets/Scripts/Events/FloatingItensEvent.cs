using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItensEvent : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int keysDesiredNum = 5;
    [SerializeField] private float soundCoolDown = 2.5f;
    [SerializeField] private SoundType sound;

    private readonly int floatUpAnim = Animator.StringToHash("floatUp");
    private readonly int floatDownAnim = Animator.StringToHash("floatDown");

    private bool isActive;
    private float soundTimer = 0;

    private void Update()
    {
        if(soundTimer > 0)
        {
            soundTimer -= Time.deltaTime;
        }

        if (!isActive)
        {
            return;
        }

        if(soundTimer <= 0)
        {
            GameService.Instance.GetSoundView().PlaySoundEffects(sound);
            soundTimer = soundCoolDown;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int keys = GameService.Instance.GetPlayerController().KeysEquipped;

        if(keys != keysDesiredNum)
        {
            return;
        }

        if(other.TryGetComponent<PlayerView>(out PlayerView playerView))
        {
            animator.Play(floatUpAnim);
            EventService.Instance.OnFloatingEventStart.InvokeEvent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int keys = GameService.Instance.GetPlayerController().KeysEquipped;

        if (keys != keysDesiredNum)
        {
            return;
        }

        if (other.TryGetComponent<PlayerView>(out PlayerView playerView))
        {
            animator.Play(floatDownAnim);
            EventService.Instance.OnFloatingEventEnd.InvokeEvent();
        }
    }
}
