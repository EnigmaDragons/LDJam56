using UnityEngine;

[CreateAssetMenu]
public class AbilityData : ScriptableObject
{
    public AbilityComponentType Type;
    public AbilityType Primary;
    public string DisplayName;
    public string Description;
    public float Duration;
    public float Amount;
    public float Speed;
    public float Range;

    public AbilityCompatibility[] Compatibilities;
}