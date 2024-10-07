using UnityEngine;

[CreateAssetMenu]
public class AbilityData : ScriptableObject
{
    public AbilityComponentType Type;
    public AbilityType Primary;
    public string DisplayName;
    public string Description;
    public Sprite Icon;
    public float Duration;
    public float Amount;
    public float Speed;
    public float Range;
    
    public float KnockbackForce;

    public AbilityCompatibility[] Compatibilities;
}