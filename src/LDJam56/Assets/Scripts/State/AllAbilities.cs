using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class AllAbilities : ScriptableObject
{
    [SerializeField] private AbilityData[] allAbilities;
    
    public AbilityData[] GetAbilities(AbilityComponentType[] abilities) => allAbilities.Where(x => abilities.Contains(x.Type)).ToArray();
    public AbilityData GetAbility(AbilityComponentType ability) => allAbilities.FirstOrDefault(x => x.Type == ability);
    public AbilityData Random() => allAbilities.Random();
}