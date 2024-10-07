using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CodeButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private TMP_Text existingText;
    [SerializeField] private TMP_Text addingText;

    private Action onSelect;
    private AbilityData _existingCode;
    private AbilityData _codeBeingAdded;

    public void Init(AbilityData existingCode, AbilityData codeBeingAdded, Action onSelect)
    {
        _existingCode = existingCode;
        _codeBeingAdded = codeBeingAdded;
        if (_existingCode == null)
        {
            existingText.gameObject.SetActive(false);
            addingText.gameObject.SetActive(true);
            addingText.text = _codeBeingAdded.DisplayName;
        }
        else
        {
            existingText.gameObject.SetActive(true);
            addingText.gameObject.SetActive(false);
            existingText.text = _existingCode.DisplayName;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        onSelect();
    }

    public void Deselect()
    {
        if (_codeBeingAdded != null)
            addingText.text = ""; 
    }

    public void Select()
    {
        if (_codeBeingAdded != null)
            addingText.text = _codeBeingAdded.DisplayName; 
    }
}