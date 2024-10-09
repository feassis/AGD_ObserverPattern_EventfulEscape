using UnityEngine;

public class PlayerSanity : MonoBehaviour
{
    [SerializeField] private float sanityLevel = 100.0f;
    [SerializeField] private float sanityDropRate = 0.2f;
    [SerializeField] private float sanityDropAmountPerEvent = 10f;
    [SerializeField] private float sanityDropMultiplierPerEvent = 3f;
    private float maxSanity;
    private PlayerController playerController;

    private bool isUnderSupernaturalEvent;

    private void OnEnable()
    {
        EventService.Instance.OnRatRush.AddListener(OnSupernaturalEvent);   
        EventService.Instance.OnSkullShower.AddListener(OnSupernaturalEvent);
        EventService.Instance.OnPotionDrink.AddListener(OnDrankPotion);
        EventService.Instance.OnDollRotationStart.AddListener(IsOnContinuosSuperNAturalEvent);
        EventService.Instance.OnDollRotationEnded.AddListener(ExitedContinuosSuperNAturalEvent);
        EventService.Instance.OnPaintingChangeEvent.AddListener(OnSupernaturalEvent);
    }

    private void OnDisable()
    {
        EventService.Instance.OnRatRush.RemoveListener(OnSupernaturalEvent);
        EventService.Instance.OnSkullShower.RemoveListener(OnSupernaturalEvent);
        EventService.Instance.OnPotionDrink.RemoveListener(OnDrankPotion);
        EventService.Instance.OnDollRotationStart.RemoveListener(IsOnContinuosSuperNAturalEvent);
        EventService.Instance.OnDollRotationEnded.RemoveListener(ExitedContinuosSuperNAturalEvent);
        EventService.Instance.OnPaintingChangeEvent.RemoveListener(OnSupernaturalEvent);
    }

    private void Start()
    {
        maxSanity = sanityLevel;
        playerController = GameService.Instance.GetPlayerController();
    }
    void Update()
    {
        if (playerController.PlayerState == PlayerState.Dead)
            return;

        float sanityDrop = updateSanity();

        increaseSanity(sanityDrop);
    }

    private float updateSanity()
    {
        float sanityDrop = sanityDropRate * Time.deltaTime;
        if (playerController.PlayerState == PlayerState.InDark)
        {
            sanityDrop *= 10f;
        }

        return isUnderSupernaturalEvent ?  sanityDrop * sanityDropMultiplierPerEvent : sanityDrop;
    }

    private void increaseSanity(float amountToDecrease)
    {
        Mathf.Floor(sanityLevel -= amountToDecrease);
        if (sanityLevel <= 0)
        {
            sanityLevel = 0;
            GameService.Instance.GameOver();
        }
        GameService.Instance.GetGameUI().UpdateInsanity(1f - sanityLevel / maxSanity);
    }

    private void decreaseSanity(float amountToIncrease)
    {
        Mathf.Floor(sanityLevel += amountToIncrease);
        if (sanityLevel > 100)
        {
            sanityLevel = 100;
        }
        GameService.Instance.GetGameUI().UpdateInsanity(1f - sanityLevel / maxSanity);
    }
    private void OnSupernaturalEvent()
    {
        increaseSanity(sanityDropAmountPerEvent);
    }

    private void IsOnContinuosSuperNAturalEvent()
    {
        isUnderSupernaturalEvent = true;
    }

    private void ExitedContinuosSuperNAturalEvent()
    {
        isUnderSupernaturalEvent = false;
    }

    private void OnDrankPotion(int potionEffect)
    {
        decreaseSanity(potionEffect);
    }
}