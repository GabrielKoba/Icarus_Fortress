using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventStringParam : UnityEvent<string> { }

public class GameEventListenerStringParam : MonoBehaviour
{
    [SerializeField]
    private GameEventStringParam gameEvent;
    [SerializeField]
    private UnityEventStringParam response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(string param)
    {
        response.Invoke(param);
    }
}
