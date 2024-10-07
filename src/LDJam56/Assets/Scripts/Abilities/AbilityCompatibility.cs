using System;

[Serializable]
public class AbilityCompatibility
{
    public AbilityType[] ValidTypes;
    public AbilitySegmentCompatibility[] AbilitiesBefore;
    public AbilitySegmentCompatibility[] AbilitiesAfter;
    public string CombinationDescription;
}