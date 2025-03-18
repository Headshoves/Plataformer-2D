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
    private Dictionary<string, UnityEvent<InfoBoardData>> infoBoardEvents;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        events = new Dictionary<string, UnityEvent>();
        intEvents = new Dictionary<string, UnityEvent<int>>();
        floatEvents = new Dictionary<string, UnityEvent<float>>();
        infoBoardEvents = new Dictionary<string, UnityEvent<InfoBoardData>>();

        InitializeEvents();
    }

    private void InitializeEvents()
    {
        AddIntEvent("OnScoreChanged");
        AddIntEvent("OnHealthChanged");
        AddEvent("OnPlayerDeath");
        AddEvent("OnPlayerFallDeath");  // Novo evento para morte por queda
        AddFloatEvent("OnInvincibilityStarted");
        AddEvent("OnGamePaused");
        AddEvent("OnGameResumed");
        AddEvent("OnGameOver");
        AddEvent("OnLevelComplete");
        AddEvent("OnLastLevelComplete");
        AddIntEvent("OnLevelLoaded");
        AddEvent("OnInfoBoardShow");
        AddEvent("OnInfoBoardClose");
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

    public void AddListener<T>(string eventName, UnityAction<T> listener) where T : ScriptableObject
    {
        if (typeof(T) == typeof(InfoBoardData))
        {
            if (!infoBoardEvents.ContainsKey(eventName))
                infoBoardEvents[eventName] = new UnityEvent<InfoBoardData>();
            infoBoardEvents[eventName].AddListener(listener as UnityAction<InfoBoardData>);
        }
    }

    public void RemoveListener(string eventName, UnityAction listener)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName].RemoveListener(listener);
        }
    }

    public void RemoveListener<T>(string eventName, UnityAction<T> listener) where T : ScriptableObject
    {
        if (typeof(T) == typeof(InfoBoardData))
        {
            if (infoBoardEvents.ContainsKey(eventName))
                infoBoardEvents[eventName].RemoveListener(listener as UnityAction<InfoBoardData>);
        }
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

    public void TriggerEvent(string eventName, InfoBoardData data)
    {
        if (infoBoardEvents.ContainsKey(eventName))
            infoBoardEvents[eventName].Invoke(data);
    }
}
