using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CodeButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text cooldownChange;
    [SerializeField] private Color existingColor;
    [SerializeField] private Color addedColor;
    [SerializeField] private Color possibleColor;

    public bool Selectable => _codeBeingAdded != null;
    
    private Action _onSelect;
    private AbilityData _existingCode;
    private AbilityData _codeBeingAdded;

    public void Init(AbilityData existingCode, AbilityData codeBeingAdded, Action onSelect, Ability ability)
    {
        _onSelect = onSelect;
        _existingCode = existingCode;
        _codeBeingAdded = codeBeingAdded;
        if (_existingCode == null)
        {
            text.text = _codeBeingAdded.DisplayName;
            text.color = possibleColor;
            cooldownChange.text = $"Cooldown: {ability.BaseCooldown} -> {ability.BaseCooldown + codeBeingAdded.GetCooldown(ability.AbilityType)}";
        }
        else
        {
            text.text = _existingCode.DisplayName;
            text.color = existingColor;
        }
        gameObject.SetActive(true);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _onSelect();
    }

    public void Deselect()
    {
        if (_codeBeingAdded != null)
            text.color = possibleColor;
    }

    public void Select()
    {
        if (_codeBeingAdded != null)
            text.color = addedColor; 
    }
}