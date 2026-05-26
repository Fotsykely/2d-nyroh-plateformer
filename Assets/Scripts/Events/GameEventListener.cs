using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private UnityEvent response;

    void OnEnable() => gameEvent.Register(this);
    void OnDisable() => gameEvent.Unregister(this);

    public void OnEventRaised() => response.Invoke();
}
