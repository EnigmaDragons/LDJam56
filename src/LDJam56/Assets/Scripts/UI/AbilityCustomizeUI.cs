using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCustomizeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text abilityTypeText;
    [SerializeField] private CodeButton[] codeButtons;
    [SerializeField] private AllAbilities allAbilities;
    [SerializeField] private Button confirm;
    [SerializeField] private Button cancel;
    [SerializeField] private TMP_Text compatibleInteractionText;
    [SerializeField] private TMP_Text cooldownText;
    
    private CodeButton _currentSelectedButton;
    private int _indexSelected;
    private Ability _ability;
    private AbilityData _toAdd;
    private Action _onCancel;
    private AbilityType _abilityType;

    public void Start()
    {
        confirm.onClick.AddListener(Confirm);
        cancel.onClick.AddListener(Cancel);
    }

    public void Init(AbilityType abilityType, AbilityData abilityToAdd, Action onCancel)
    {
        _abilityType = abilityType;
        abilityTypeText.text = abilityType.ToString();
        _toAdd = abilityToAdd;
        _onCancel = onCancel;
        _ability = CurrentGameState.GetAbility(abilityType);
        codeButtons.ForEach(x => x.gameObject.SetActive(false));
        var codeButtonIndex = 0;
        for (var abilityInsertIndex = 0; abilityInsertIndex <= _ability.Components.Count; abilityInsertIndex++)
        {
            var compatibility = abilityToAdd.GetMaybeCompatible(abilityType, _ability.Components, abilityInsertIndex);
            if (compatibility.IsPresent)
            {
                var i = abilityInsertIndex;
                var button = codeButtons[codeButtonIndex];
                codeButtons[codeButtonIndex].Init(null, abilityToAdd, () => SetIndex(i, button, compatibility.Value.CombinationDescription, $"Cooldown: {_ability.BaseCooldown}s -> {_ability.BaseCooldown + abilityToAdd.GetCooldown(_ability.AbilityType)}s"));
                codeButtonIndex++;
            }
            if (abilityInsertIndex != _ability.Components.Count)
            {
                codeButtons[codeButtonIndex].Init(allAbilities.GetAbility(_ability.Components[abilityInsertIndex]), null, () => {});
                codeButtonIndex++;
            }
        }
        codeButtons.First(x => x.Selectable).OnPointerEnter(null);
        gameObject.SetActive(true);
    }

    private void SetIndex(int index, CodeButton button, string compatibleInteraction, string cooldown)
    {
        _indexSelected = index;
        _currentSelectedButton = button;
        codeButtons.ForEach(x =>
        {
            if (x == button)
                x.Select();
            else
                x.Deselect();
        });
        compatibleInteractionText.text = compatibleInteraction;
        cooldownText.text = cooldown;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
            Confirm();
        else if (Input.GetButtonDown("Cancel"))
            Cancel();

        if (_indexSelected != _ability.Components.Count && Input.GetKeyDown(KeyCode.DownArrow))
        {
            codeButtons
                .Skip(codeButtons.FirstIndexOf(x => x == _currentSelectedButton) + 1)
                .FirstOrMaybe(x => x.Selectable)
                .IfPresent(b => b.OnPointerEnter(null));
        }
        if (_indexSelected != 0 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            codeButtons.Take(codeButtons.FirstIndexOf(x => x == _currentSelectedButton))
                .FirstOrMaybe(x => x.Selectable)
                .IfPresent(b => b.OnPointerEnter(null));
        }
    }

    public void Confirm()
    {
        CurrentGameState.UpdateState(s =>
        {
            _ability.Components.Insert(_indexSelected, _toAdd.Type);
            _ability.BaseCooldown += _toAdd.GetCooldown(_abilityType);
        });
        Message.Publish(new AbilityUpgraded());
    }

    public void Cancel()
    {
        _onCancel();
    }
}