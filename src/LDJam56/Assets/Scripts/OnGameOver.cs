using UnityEngine;
using UnityEngine.Events;

public class OnGameOver : OnMessage<GameOver>
{
    [SerializeField] private UnityEvent e;
    
    protected override void Execute(GameOver msg)
    {
        e.Invoke();
    }
}
