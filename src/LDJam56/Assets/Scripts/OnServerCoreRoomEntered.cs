using UnityEngine;
using UnityEngine.Events;

public class OnServerCoreRoomEntered : OnMessage<ServerCoreRoomEntered>
{
    [SerializeField] private UnityEvent action;
    
    protected override void Execute(ServerCoreRoomEntered msg)
    {
        Log.Info("Server Core Event", this);
        action.Invoke();
    }
}
