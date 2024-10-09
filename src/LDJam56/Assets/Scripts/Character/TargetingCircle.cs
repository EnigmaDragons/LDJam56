using UnityEngine;

public class TargetingCircle : OnMessage<TargetingUpdated>
{
    private Vector3 _direction;
    
    protected override void Execute(TargetingUpdated msg)
    {
        _direction = msg.Direction;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(_direction);
    }
}