using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class GameEvents : MonoBehaviour
{
    private static GameEvents instance;
    public static GameEvents Instance => instance;

    private Dictionary<string, UnityEvent> events;
    private Dictionary<string, UnityEvent<int>> intEvents;
    private Dictionary<string, UnityEvent<float>> floatEvents;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        events = new Dictionary<string, UnityEvent>();
        intEvents = new Dictionary<string, UnityEvent<int>>();
        floatEvents = new Dictionary<string, UnityEvent<float>>();

        InitializeEvents();
    }

    private void InitializeEvents()
    {
        AddIntEvent("OnScoreChanged");
        AddIntEvent("OnHealthChanged");
        AddEvent("OnPlayerDeath");
        AddFloatEvent("OnInvincibilityStarted");
    }

    public void AddListener(string eventName, UnityAction listener)
    {
        if (!events.ContainsKey(eventName))
            events[eventName] = new UnityEvent();
        events[eventName].AddListener(listener);
    }

    public void AddIntListener(string eventName, UnityAction<int> listener)
    {
        if (!intEvents.ContainsKey(eventName))
            intEvents[eventName] = new UnityEvent<int>();
        intEvents[eventName].AddListener(listener);
    }

    public void AddFloatListener(string eventName, UnityAction<float> listener)
    {
        if (!floatEvents.ContainsKey(eventName))
            floatEvents[eventName] = new UnityEvent<float>();
        floatEvents[eventName].AddListener(listener);
    }

    private void AddEvent(string eventName)
    {
        if (!events.ContainsKey(eventName))
            events[eventName] = new UnityEvent();
    }

    private void AddIntEvent(string eventName)
    {
        if (!intEvents.ContainsKey(eventName))
            intEvents[eventName] = new UnityEvent<int>();
    }

    private void AddFloatEvent(string eventName)
    {
        if (!floatEvents.ContainsKey(eventName))
            floatEvents[eventName] = new UnityEvent<float>();
    }

    public void TriggerEvent(string eventName)
    {
        if (events.ContainsKey(eventName))
            events[eventName].Invoke();
    }

    public void TriggerEvent(string eventName, int value)
    {
        if (intEvents.ContainsKey(eventName))
            intEvents[eventName].Invoke(value);
    }

    public void TriggerEvent(string eventName, float value)
    {
        if (floatEvents.ContainsKey(eventName))
            floatEvents[eventName].Invoke(value);
    }
}
