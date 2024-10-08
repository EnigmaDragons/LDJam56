using UnityEngine;

[CreateAssetMenu]
public class SelectedDifficulty : ScriptableObject
{
    public Difficulty Difficulty;

    public void SelectEasy() => Difficulty = Difficulty.Easy;
    public void SelectMedium() => Difficulty = Difficulty.Medium;
    public void SelectHard() => Difficulty = Difficulty.Hard;
}