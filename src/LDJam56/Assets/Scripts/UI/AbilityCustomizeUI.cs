using System;
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

    private AbilityType _abilityType;
    private int _indexSelected;
    private Ability _ability;
    private AbilityData _toAdd;
    private Action _onCancel;
    private Action _onComplete;

    public void Start()
    {
        confirm.onClick.AddListener(Confirm);
        cancel.onClick.AddListener(Cancel);
    }

    public void Init(AbilityType abilityType, AbilityData abilityToAdd, Action onCancel, Action onComplete)
    {
        _abilityType = abilityType;
        abilityTypeText.text = abilityType.ToString();
        _toAdd = abilityToAdd;
        _onCancel = onCancel;
        _onComplete = onComplete;
        _ability = CurrentGameState.GetAbility(abilityType);
        codeButtons.ForEach(x => x.gameObject.SetActive(false));

        if (abilityType == AbilityType.Passive)
        {
            _indexSelected = _ability.Components.Count;
            for (var i = 0; i <= _ability.Components.Count; i++)
            {
                if (i == _ability.Components.Count)
                {
                    codeButtons[i].Init(null, abilityToAdd, () => { });
                    codeButtons[i].Select();
                }
                else
                    codeButtons[i].Init(allAbilities.GetAbility(_ability.Components[i]), null, () => {});
            }
        }
        else
        {
            codeButtons[0].gameObject.SetActive(true);
            codeButtons[0].Init(null, abilityToAdd, () => SetIndex(0));
            for (var i = 0; i < _ability.Components.Count; i++)
            {
                codeButtons[i * 2 + 1].Init(allAbilities.GetAbility(_ability.Components[i]), null, () => {});
                var indexToSet = i + 1;
                codeButtons[i * 2 + 2].Init(null, abilityToAdd, () => SetIndex(indexToSet));
            }
            SetIndex(0);
        }
        gameObject.SetActive(true);
    }

    private void SetIndex(int index)
    {
        if (_abilityType == AbilityType.Passive)
            return;
        _indexSelected = index;
        for (var i = 0; i <= _ability.Components.Count; i++)
        {
            if (i == index)
                codeButtons[i * 2].Select();
            else
                codeButtons[i * 2].Deselect();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
            Confirm();
        else if (Input.GetButtonDown("Cancel"))
            Cancel();
        
        if (_indexSelected != _ability.Components.Count && Input.GetKeyDown(KeyCode.DownArrow))
            SetIndex(_indexSelected + 1);
        if (_indexSelected != 0 && Input.GetKeyDown(KeyCode.UpArrow))
            SetIndex(_indexSelected - 1);
    }

    public void Confirm()
    {
        CurrentGameState.UpdateState(s => _ability.Components.Insert(_indexSelected, _toAdd.Type));
        _onComplete();
    }

    public void Cancel()
    {
        _onCancel();
    }
}