using UnityEngine;
using UnityEngine.Events;

public class OnServerCoreRoomEntered : OnMessage<OnServerCoreRoomEntered>
{
    [SerializeField] private UnityEvent action;
    
    protected override void Execute(OnServerCoreRoomEntered msg)
    {
        action.Invoke();
    }
}
