using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCustomizeUI : MonoBehaviour
{
    [SerializeField] private CodeButton[] codeButtons;
    [SerializeField] private AllAbilities allAbilities;
    [SerializeField] private Button confirm;
    [SerializeField] private Button cancel;

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
        _toAdd = abilityToAdd;
        _onCancel = onCancel;
        _onComplete = onComplete;
        codeButtons.ForEach(x => x.gameObject.SetActive(false));
        codeButtons[0].gameObject.SetActive(true);
        codeButtons[0].Init(null, abilityToAdd, () => SetIndex(0));
        _ability = CurrentGameState.GetAbility(abilityType);
        for (var i = 0; i < _ability.Components.Count; i++)
        {
            codeButtons[i * 2 - 1].gameObject.SetActive(true);
            codeButtons[i * 2 - 1].Init(allAbilities.GetAbility(_ability.Components[i]), null, () => {});
            codeButtons[i * 2].gameObject.SetActive(true);
            var indexToSet = i;
            codeButtons[i * 2].Init(null, abilityToAdd, () => SetIndex(indexToSet));
        }
        SetIndex(0);
    }

    private void SetIndex(int index)
    {
        _indexSelected = index;
        for (var i = 0; i < _ability.Components.Count; i++)
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
        _ability.Components.Insert(_indexSelected, _toAdd.Type);
        _onComplete();
    }

    public void Cancel()
    {
        _onCancel();
    }
}