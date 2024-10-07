using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilityData : ScriptableObject
{
    public AbilityComponentType Type;
    public AbilityType Primary;
    public string DisplayName;
    public string Description;
    public string PassiveDescription;
    public Sprite Icon;
    public float Duration;
    public float Amount;
    public float Speed;
    public float Range;
    
    public float KnockbackForce;

    public AbilityCompatibility[] Compatibilities;

    public bool IsValid(AbilityType type, List<AbilityComponentType> components)
    {
        for (var i = 0; i <= components.Count; i++)
            if (GetMaybeCompatible(type, components, i).IsPresent)
                return true;
        return false;
    }
    
    public Maybe<AbilityCompatibility> GetMaybeCompatible(AbilityType type, List<AbilityComponentType> components, int indexToBeInserted)
    {
        return Compatibilities.FirstOrMaybe(compatibility =>
        {
            if (compatibility.Ability != type)
                return false;
            for (var i = 0; i < compatibility.abilitiesBefore.Length; i++)
            {
                var abilityBefore = compatibility.abilitiesBefore[i];
                if (abilityBefore == AbilityComponentType.None)
                {
                    if (indexToBeInserted != i)
                        return false;
                }
                else
                {
                    var indexToGrab = indexToBeInserted - i - 1;
                    if (indexToGrab < 0 || indexToGrab >= components.Count)
                        return false;
                    if (components[indexToGrab] != abilityBefore)
                        return false;
                }
            }
            for (var i = 0; i < compatibility.abilitiesAfter.Length; i++)
            {
                var abilityAfter = compatibility.abilitiesAfter[i];
                if (abilityAfter == AbilityComponentType.None)
                {
                    if (indexToBeInserted != compatibility.abilitiesAfter.Length + i)
                        return false;
                }
                else
                {
                    var indexToGrab = indexToBeInserted + i;
                    if (indexToGrab < 0 || indexToGrab >= components.Count)
                        return false;
                    if (components[indexToGrab] != abilityAfter)
                        return false;
                }
            }
            return true;
        });
    }
}