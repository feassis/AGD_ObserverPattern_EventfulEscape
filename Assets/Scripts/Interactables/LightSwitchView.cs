using System;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchView : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Light> lightsources = new List<Light>();
    [SerializeField] private SoundType soundType;
    [SerializeField] private AnimationCurve flickringLights;
    [SerializeField] private float flickeringDuration = 2f;
    private SwitchState currentState;

    private bool isLightFlickeing = false;
    private float elapsedTime = 0;

    private void OnEnable()
    {
        EventService.Instance.OnLightSwitchToggled.AddListener(onLightsToggled);
        EventService.Instance.OnLightsOffByGhostEvent.AddListener(onLightsOffByGhostEvent);
        EventService.Instance.OnWhisperingStart.AddListener(onWhisperingStart);
        EventService.Instance.OnWhisperingEnded.AddListener(onWhisperingEnded);
    }

    private void OnDisable()
    {
        EventService.Instance.OnLightSwitchToggled.RemoveListener(onLightsToggled);
        EventService.Instance.OnLightsOffByGhostEvent.RemoveListener(onLightsOffByGhostEvent);
        EventService.Instance.OnWhisperingStart.RemoveListener(onWhisperingStart);
        EventService.Instance.OnWhisperingEnded.RemoveListener(onWhisperingEnded);
    }

    private void Start()
    {
        currentState = SwitchState.Off;
    }

    private void Update()
    {
        if(currentState != SwitchState.On)
        {
            return;
        }

        if(!isLightFlickeing)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        if(elapsedTime > flickeringDuration)
        {
            elapsedTime = 0;
        }

        foreach (Light lightSource in lightsources)
        {
            lightSource.intensity = flickringLights.Evaluate(elapsedTime/flickeringDuration) ;
        }
    }

    public void Interact()
    {
        GameService.Instance.GetInstructionView().HideInstruction();
        EventService.Instance.OnLightSwitchToggled.InvokeEvent();
    }
    private void toggleLights()
    {
        bool lights = false;

        switch (currentState)
        {
            case SwitchState.On:
                currentState = SwitchState.Off;
                lights = false;
                break;
            case SwitchState.Off:
                currentState = SwitchState.On;
                lights = true;
                break;
            case SwitchState.Unresponsive:
                break;
        }
        foreach (Light lightSource in lightsources)
        {
            lightSource.enabled = lights;
        }
    }

    private void setLights(bool lights)
    {
        if (lights)
            currentState = SwitchState.On;
        else
            currentState = SwitchState.Off;

        foreach (Light lightSource in lightsources)
        {
            lightSource.enabled = lights;
        }
    }
    private void onLightsOffByGhostEvent()
    {
        GameService.Instance.GetSoundView().PlaySoundEffects(soundType);
        setLights(false);
    }
    private void onLightsToggled()
    {
        toggleLights();
        GameService.Instance.GetSoundView().PlaySoundEffects(soundType);
    }

    private void onWhisperingEnded()
    {
        isLightFlickeing = false;
        foreach (Light lightSource in lightsources)
        {
            lightSource.intensity =  1;
        }
    }

    private void onWhisperingStart()
    {
        isLightFlickeing = true;
    }
}
