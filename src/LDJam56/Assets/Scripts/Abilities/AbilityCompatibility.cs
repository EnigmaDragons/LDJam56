using System;

[Serializable]
public class AbilityCompatibility
{
    public AbilityType Ability;
    public AbilityComponentType[] abilitiesBefore;
    public AbilityComponentType[] abilitiesAfter;
}