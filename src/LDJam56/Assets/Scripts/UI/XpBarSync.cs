using UnityEngine;
using Renge.PPB;

public class XpBarSync : MonoBehaviour
{
    [SerializeField] private ProceduralProgressBar xpBar;
    [SerializeField] private GameplayRules gameplayRules;

    private bool isXpBarInitialized = false;

    private void Update()
    {
        if (!isXpBarInitialized)
        {
            if (!xpBar.IsInitialized)
                return;
            
            isXpBarInitialized = true;
        }

        int currentXp = CurrentGameState.ReadonlyGameState.PlayerStats.XP;
        int xpNeededToLevel = gameplayRules.XpNeededToLevel;

        // Calculate XP progress towards next level
        float xpProgress = (float)currentXp / xpNeededToLevel;

        // Clamp the value between 0 and 1
        xpProgress = Mathf.Clamp01(xpProgress);

        // Set the value on the progress bar
        xpBar.Value = xpProgress;
    }
}
