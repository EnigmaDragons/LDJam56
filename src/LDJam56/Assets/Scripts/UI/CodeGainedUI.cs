using UnityEngine;

public class CodeGainedUI : OnMessage<PlayerGainedCode>
{
    [SerializeField] private ShowCodeGained shownUI;
    [SerializeField] private WhichAbilityWeChangingUI whereToApplyUI;
    [SerializeField] private AbilityCustomizeUI abilityCustomizeUI;

    private AbilityData _ability;
    
    private void Start()
    {
        whereToApplyUI.Init(SelectAbilityType);
    }
    
    protected override void Execute(PlayerGainedCode msg)
    {
        _ability = msg.Ability;
        Time.timeScale = 0;
        shownUI.Init(msg.Ability);
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
        }, () =>
        {
            abilityCustomizeUI.gameObject.SetActive(false);
            Time.timeScale = 1;
        });
    }
}