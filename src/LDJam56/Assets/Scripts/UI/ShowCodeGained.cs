using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowCodeGained : MonoBehaviour
{
    [SerializeField] private TMP_Text abilityName;
    [SerializeField] private TMP_Text abilityDescription;
    [SerializeField] private Image abilitySprite;
    
    public void Init(AbilityData data)
    {
        abilityName.text = data.DisplayName;
        abilityDescription.text = data.Description;
        //abilitySprite.sprite = data.AbilityIcon;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}