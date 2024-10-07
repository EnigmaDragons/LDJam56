using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!compatibility.ValidTypes.Contains(type))
                return false;
            for (var i = 0; i < compatibility.AbilitiesBefore.Length; i++)
            {
                var abilityBefore = compatibility.AbilitiesBefore[i];
                var indexToGrab = indexToBeInserted - i - 1;
                if (!abilityBefore.Types.AnyNonAlloc())
                    continue; //no requirements is always valid go next
                //if there is nothing here
                if (indexToGrab < 0 || indexToGrab >= components.Count)
                {
                    //if you dont have None as an option and you have a required option
                    if (!abilityBefore.Types.Contains(AbilityComponentType.None) && abilityBefore.Types.AnyNonAlloc(x => x > 0))
                        return false;
                    continue;
                }
                //if there is a required type and this is not among them
                if (abilityBefore.Types.AnyNonAlloc(x => x >= 0) && !abilityBefore.Types.Any(x => x >= 0 && x == components[indexToGrab]))
                    return false;
                //if there is a restricted type and it has it
                if (abilityBefore.Types.AnyNonAlloc(x => x < 0 && (AbilityComponentType)Math.Abs((int)x) == components[indexToGrab]))
                    return false;
            }
            for (var i = 0; i < compatibility.AbilitiesAfter.Length; i++)
            {
                var abilityAfter = compatibility.AbilitiesAfter[i];
                var indexToGrab = indexToBeInserted + i;
                if (!abilityAfter.Types.AnyNonAlloc())
                    continue; //no requirements is always valid go next
                //if there is nothing here
                if (indexToGrab < 0 || indexToGrab >= components.Count)
                {
                    //if you dont have None as an option and you have a required option
                    if (!abilityAfter.Types.Contains(AbilityComponentType.None) && abilityAfter.Types.AnyNonAlloc(x => x > 0))
                        return false;
                    continue;
                }
                //if there is a required type and this is not among them
                if (abilityAfter.Types.AnyNonAlloc(x => x >= 0) && !abilityAfter.Types.Any(x => x >= 0 && x == components[indexToGrab]))
                    return false;
                //if there is a restricted type and it has it
                if (abilityAfter.Types.AnyNonAlloc(x => x < 0 && (AbilityComponentType)Math.Abs((int)x) == components[indexToGrab]))
                    return false;
            }
            return true;
        });
    }
}