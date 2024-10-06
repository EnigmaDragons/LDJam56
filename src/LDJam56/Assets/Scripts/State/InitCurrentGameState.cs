using UnityEngine;

public sealed class InitCurrentGameState : MonoBehaviour
{
    [SerializeField] private GameplayRules rules;
    
    void Awake()
    {
        if (rules == null)
        {
            Log.Error("GameplayRules is null", this);
        }
        else
        {
            CurrentGameState.Init(new GameState { PlayerStats = new PlayerStats { MaxLife = rules.PlayerHealth, CurrentLife = rules.PlayerHealth } });
        }
    }
}
