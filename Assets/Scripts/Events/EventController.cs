using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController
{
    public event Action BaseEvent;

    public void AddListener(Action listener) => BaseEvent += listener;

    public void RemoveListener(Action listener) => BaseEvent -= listener;

    public void InvokeEvent() => BaseEvent?.Invoke();
}
