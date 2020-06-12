using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event String Param", menuName = "Game Event String Param", order = 52)]
public class GameEventStringParam : ScriptableObject
{
    private List<GameEventListenerStringParam> listeners = new List<GameEventListenerStringParam>();

    public void Raise(string param)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(param);
        }
    }

    public void RegisterListener(GameEventListenerStringParam listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListenerStringParam listener)
    {
        listeners.Remove(listener);
    }
}
