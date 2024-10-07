using UnityEngine;
using UnityEngine.Events;

public class OnGameWon : OnMessage<GameWon>
{
    [SerializeField] private UnityEvent action;
    
    protected override void Execute(GameWon msg)
    {
        action.Invoke();
    }
}
