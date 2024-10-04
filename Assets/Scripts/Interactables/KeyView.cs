using UnityEngine;

public class KeyView : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        int currentKeys = GameService.Instance.GetPlayerController().KeysEquipped;
        currentKeys++;
        GameService.Instance.GetInstructionView().HideInstruction();
        GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.KeyPickUp);

        EventService.Instance.OnKeyPickUp.InvokeEvent(currentKeys);

        gameObject.SetActive(false);
    }
}
