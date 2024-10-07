using UnityEngine;

public class TargetingCircle : OnMessage<TargetingUpdated>
{
    protected override void Execute(TargetingUpdated msg)
    {
        transform.rotation = Quaternion.LookRotation(msg.Direction);
    }
}