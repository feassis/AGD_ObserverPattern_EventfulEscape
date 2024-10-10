using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollJumpScareEvent : MonoBehaviour
{
    [SerializeField] private GameObject dollGameObject;
    [SerializeField] private SoundType sound;
    [SerializeField] private float jumpScareDuration = 2.0f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerView>(out PlayerView playerView))
        {
            GetComponent<Collider>().enabled = false;
            GameService.Instance.GetSoundView().PlaySoundEffects(sound);
            EventService.Instance.OnDollJumpScare.InvokeEvent();
            StartCoroutine(DollJumpsCare());
        }
    }


    private IEnumerator DollJumpsCare()
    {
        dollGameObject.SetActive(true);
        yield return new WaitForSeconds(jumpScareDuration);
        dollGameObject.SetActive(false);
    }
}
