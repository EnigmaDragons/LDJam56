using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class WhichAbilityWeChangingUI : MonoBehaviour
{
    [SerializeField] private PassiveAbilityButton passiveButton;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button specialButton;
    [SerializeField] private Button mobilityButton;
    [SerializeField] private Button defenseButton;
    [SerializeField] private GameplayRules gameplayRules;
    
    private Action<AbilityType> _onAbility;

    private void Start()
    {
        attackButton.onClick.AddListener(() => SelectAbility(AbilityType.Attack));
        specialButton.onClick.AddListener(() => SelectAbility(AbilityType.Special));
        mobilityButton.onClick.AddListener(() => SelectAbility(AbilityType.Mobility));
        defenseButton.onClick.AddListener(() => SelectAbility(AbilityType.Defense));
    }
    
    public void Init(Action<AbilityType> onAbility, AbilityData ability)
    {
        passiveButton.Init(ability);
        _onAbility = onAbility;
        attackButton.gameObject.SetActive(IsSelectable(AbilityType.Attack, ability));
        specialButton.gameObject.SetActive(IsSelectable(AbilityType.Special, ability));
        mobilityButton.gameObject.SetActive(IsSelectable(AbilityType.Mobility, ability));
        defenseButton.gameObject.SetActive(IsSelectable(AbilityType.Defense, ability));
    }

    private void SelectAbility(AbilityType abilityType)
    {
        _onAbility(abilityType);
    }

    private bool IsSelectable(AbilityType type, AbilityData abilityData)
    {
        var ability = CurrentGameState.GetAbility(type);
        if (gameplayRules.MaxAbilityComponents == ability.Components.Count)
            return false;
        return abilityData.IsValid(type, ability.Components);
    }
}