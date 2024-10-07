using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class PassiveAbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject hoverDescription;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;
        
        private AbilityComponentType _type;

        private void Start()
        {
            button.onClick.AddListener(OnClick);
            hoverDescription.gameObject.SetActive(false);
        }

        public void Init(AbilityData ability)
        {
            _type = ability.Type;
            text.text = ability.PassiveDescription;
        }

        private void OnClick()
        {
            CurrentGameState.UpdateState(s =>
            {
                s.Passives.Components.Add(_type);
                if (_type == AbilityComponentType.Speed)
                    s.PlayerStats.Speed += 0.1f;
                if (_type == AbilityComponentType.Projectile)
                    s.PlayerStats.Projectile += 1;
                if (_type == AbilityComponentType.Explode)
                    s.PlayerStats.AoEPotency += 0.1f;
                if (_type == AbilityComponentType.Explode)
                    s.PlayerStats.AoEPotency += 0.1f;
                if (_type == AbilityComponentType.Shield)
                    s.PlayerStats.AfterHitShieldPotency += 0.5f;
            });
            Message.Publish(new AbilityUpgraded());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverDescription.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hoverDescription.gameObject.SetActive(false);
        }
    }
}