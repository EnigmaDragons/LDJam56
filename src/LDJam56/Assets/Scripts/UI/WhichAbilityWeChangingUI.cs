using System;
using UnityEngine;

public class WhichAbilityWeChangingUI : MonoBehaviour
{
    private Action<AbilityType> _onAbility;
    
    public void Init(Action<AbilityType> onAbility)
    {
        _onAbility = onAbility;
    }

    public void SelectAbility(AbilityType abilityType)
    {
        _onAbility(abilityType);
    }
}