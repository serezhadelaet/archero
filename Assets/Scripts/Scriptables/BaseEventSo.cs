using System;
using UnityEngine;

public class BaseEventSo : ScriptableObject
{
    public event Action Event;

    public void Invoke() => Event?.Invoke();
}