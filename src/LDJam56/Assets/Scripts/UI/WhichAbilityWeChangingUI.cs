using System;
using UnityEngine;
using UnityEngine.UI;

public class WhichAbilityWeChangingUI : MonoBehaviour
{
    [SerializeField] private Button passiveButton;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button specialButton;
    [SerializeField] private Button mobilityButton;
    [SerializeField] private Button defenseButton;
    
    private Action<AbilityType> _onAbility;

    private void Start()
    {
        passiveButton.onClick.AddListener(() => SelectAbility(AbilityType.Passive));
        attackButton.onClick.AddListener(() => SelectAbility(AbilityType.Attack));
        specialButton.onClick.AddListener(() => SelectAbility(AbilityType.Special));
        mobilityButton.onClick.AddListener(() => SelectAbility(AbilityType.Mobility));
        defenseButton.onClick.AddListener(() => SelectAbility(AbilityType.Defense));
    }
    
    public void Init(Action<AbilityType> onAbility)
    {
        _onAbility = onAbility;
    }

    public void SelectAbility(AbilityType abilityType)
    {
        _onAbility(abilityType);
    }
}