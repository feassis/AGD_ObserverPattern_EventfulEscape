using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingPaintEvent : MonoBehaviour
{
    [SerializeField] private GameObject RegularImage;
    [SerializeField] private GameObject CreepyImage;

    private bool hasActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerView>(out  PlayerView view))
        {
            if (hasActivated)
            {
                return;
            }

            RegularImage.SetActive(false);
            CreepyImage.SetActive(true);
            GameService.Instance.GetSoundView().StopSoundEffects();
            GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.Scream);

            EventService.Instance.OnPaintingChangeEvent.InvokeEvent();
        }
    }
}
