using UnityEngine;

public class CodeGainedUI : OnMessage<CodeGainedUI>
{
    [SerializeField] private GameObject shownUI;
    [SerializeField] private GameObject whereToApplyUI;
    [SerializeField] private GameObject attackAbilityUI;
    [SerializeField] private GameObject specialAbilityUI;
    [SerializeField]

    protected override void Execute(CodeGainedUI msg)
    {
        
    }
}