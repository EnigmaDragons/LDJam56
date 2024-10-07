using UnityEngine;

public class CodeGainedUI : OnMessage<PlayerGainedCode, AbilityUpgraded>
{
    [SerializeField] private ShowCodeGained shownUI;
    [SerializeField] private WhichAbilityWeChangingUI whereToApplyUI;
    [SerializeField] private AbilityCustomizeUI abilityCustomizeUI;

    private AbilityData _ability;
    
    protected override void Execute(PlayerGainedCode msg)
    {
        whereToApplyUI.Init(SelectAbilityType, msg.Ability);
        _ability = msg.Ability;
        Time.timeScale = 0;
        shownUI.Init(msg.Ability);
    }

    protected override void Execute(AbilityUpgraded msg)
    {
        shownUI.gameObject.SetActive(false);
        whereToApplyUI.gameObject.SetActive(false);
        abilityCustomizeUI.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (shownUI.gameObject.activeInHierarchy)
        {
            if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Attack"))
            {
                shownUI.Disable();
                whereToApplyUI.gameObject.SetActive(true);
            }
        }
    }

    private void SelectAbilityType(AbilityType abilityType)
    {
        whereToApplyUI.gameObject.SetActive(false);
        abilityCustomizeUI.Init(abilityType, _ability, () =>
        {
            whereToApplyUI.gameObject.SetActive(true);
            abilityCustomizeUI.gameObject.SetActive(false);
        });
    }
}