using UnityEngine;
using UnityEngine.Events;

public class OnLowHealth : OnMessage<GameStateChanged>
{
    [SerializeField] private UnityEvent onLowHealth;
    [SerializeField] private UnityEvent onNotLowHealth;

    private bool isLowHealth = false;
    
    protected override void Execute(GameStateChanged msg)
    {
        int currentHP = CurrentGameState.ReadonlyGameState.PlayerStats.CurrentLife;
        bool newLowHealthState = currentHP <= 1;

        if (newLowHealthState != isLowHealth)
        {
            isLowHealth = newLowHealthState;
            if (isLowHealth)
            {
                onLowHealth?.Invoke();
            }
            else
            {
                onNotLowHealth?.Invoke();
            }
        }
    }
}
