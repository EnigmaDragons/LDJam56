using UnityEngine;

public sealed class InitCurrentGameState : MonoBehaviour
{
    [SerializeField] private GameplayRules rules;
    
    void Awake() => CurrentGameState.Init(new GameState { PlayerStats = new PlayerStats { MaxLife = rules.PlayerHealth, CurrentLife = rules.PlayerHealth }});
}
