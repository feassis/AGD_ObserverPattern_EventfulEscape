using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivimentService
{
    private EventService eventService;

    private const string keyMaster = "KeyMaster";
    private const string sanitySaver = "SanitySaver";
    private const string masterOfShadows = "MasterOfShadows";
    private const string tormentedSavior = "TormentedSurvivor";

    private const int numberOfKeys = 8;
    private const float requiredTimeInDark = 120f;
    private const int numberOfPotions = 10;
    private int potionsDrinked = 0;
    private const float sanityTrashold = 0.9f;

    public void Init(EventService eventService)
    {
#if UNITY_EDITOR
        PlayerPrefs.DeleteAll();
#endif

        this.eventService = eventService;

        this.eventService.OnKeyPickedUp.AddListener(onKeyPickedUp);
        this.eventService.OnPlayerInDark.AddListener(onPlayerIsInDark);
        this.eventService.OnPotionDrink.AddListener(onPotionsDrinked);
        this.eventService.PlayerEscapedEvent.AddListener(onPlayerEscaped);
    }

    private void onPotionsDrinked(int obj)
    {
        potionsDrinked++;

        if(potionsDrinked == numberOfPotions)
        {
            if (PlayerPrefs.GetInt(sanitySaver) == 0)
            {
                GameService.Instance.GetGameUI().AchievimentUnlocked("Sanity Saver Unlocked!!");
            }

            PlayerPrefs.SetInt(sanitySaver, 1);
        }
    }

    private void onPlayerIsInDark(float timeInDark)
    {
        if(timeInDark > requiredTimeInDark)
        {
            if (PlayerPrefs.GetInt(masterOfShadows) == 0)
            {
                GameService.Instance.GetGameUI().AchievimentUnlocked("Master Of Shadows Unlocked!!");
            }

            PlayerPrefs.SetInt(masterOfShadows, 1);
            this.eventService.OnPlayerInDark.RemoveListener(onPlayerIsInDark);
        }
    }

    ~AchivimentService() 
    {
        this.eventService.OnKeyPickedUp.RemoveListener(onKeyPickedUp);
        this.eventService.OnPlayerInDark.RemoveListener(onPlayerIsInDark);
        this.eventService.OnPotionDrink.RemoveListener(onPotionsDrinked);
        this.eventService.PlayerEscapedEvent.RemoveListener(onPlayerEscaped);
    }

    private void onKeyPickedUp(int keys)
    {
        if (numberOfKeys == keys)
        {
            if(PlayerPrefs.GetInt(keyMaster) == 0)
            {
                GameService.Instance.GetGameUI().AchievimentUnlocked("Key Master Unlocked!!");
            }

            PlayerPrefs.SetInt(keyMaster, 1);
        }
    }

    private void onPlayerEscaped()
    {
        float sanity = GameService.Instance.PlayerSanity.GetSanityPercentage();

        if(sanity >= sanityTrashold)
        {
            if (PlayerPrefs.GetInt(tormentedSavior) == 0)
            {
                GameService.Instance.GetGameUI().AchievimentUnlocked("Tormented Savior Unlocked!!");
            }

            PlayerPrefs.SetInt(tormentedSavior, 1);
        }
    }

    public void DecideAchievimentUI(GameObject keyMasterFill, GameObject sanitySaverFill, GameObject masterOfShadowsFill, GameObject tormentedSurvivorFill)
    {
        keyMasterFill.SetActive(PlayerPrefs.GetInt(keyMaster) != 0);
        sanitySaverFill.SetActive(PlayerPrefs.GetInt(sanitySaver) != 0);
        masterOfShadowsFill.SetActive(PlayerPrefs.GetInt(masterOfShadows) != 0);
        tormentedSurvivorFill.SetActive(PlayerPrefs.GetInt(tormentedSavior) != 0);
    }
}
