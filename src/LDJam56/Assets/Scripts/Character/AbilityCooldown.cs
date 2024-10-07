using System;
using RengeGames.HealthBars;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    [SerializeField] private RadialSegmentedHealthBar cooldownRadial;
    [SerializeField] private AbilityType type;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;

    private bool _onCooldown = false;

    private void Start()
    {
        cooldownRadial.SetPercent(0);
        _onCooldown = false;
        text.text = "";
        image.enabled = false;
    }

    private void Update()
    {
        var cooldown = GetCooldown();
        var baseCooldown = GetBaseCooldown();
        if (_onCooldown && cooldown <= 0)
        {
            _onCooldown = false;
            text.text = "";
            image.enabled = false;
            cooldownRadial.SetPercent(0);
        }
        else if (!_onCooldown && cooldown > 0)
        {
            _onCooldown = true;
            cooldownRadial.SetPercent(1);
            image.enabled = true;
        }
        
        if (_onCooldown)
        {
            text.text = Math.Ceiling(cooldown).ToString();
            cooldownRadial.SetPercent(cooldown / baseCooldown);
        }
    }

    private float GetCooldown()
    {
        if (type == AbilityType.Attack)
            return CurrentGameState.ReadonlyGameState.Attack.CooldownRemaining;
        if (type == AbilityType.Special)
            return CurrentGameState.ReadonlyGameState.Special.CooldownRemaining;
        if (type == AbilityType.Mobility)
            return CurrentGameState.ReadonlyGameState.Mobility.CooldownRemaining;
        if (type == AbilityType.Defense)
            return CurrentGameState.ReadonlyGameState.Defense.CooldownRemaining;
        return 0;
    }

    private float GetBaseCooldown()
    {
        if (type == AbilityType.Attack)
            return CurrentGameState.ReadonlyGameState.Attack.Cooldown(CurrentGameState.ReadonlyGameState.PlayerStats);
        if (type == AbilityType.Special)
            return CurrentGameState.ReadonlyGameState.Special.Cooldown(CurrentGameState.ReadonlyGameState.PlayerStats);
        if (type == AbilityType.Mobility)
            return CurrentGameState.ReadonlyGameState.Mobility.Cooldown(CurrentGameState.ReadonlyGameState.PlayerStats);
        if (type == AbilityType.Defense)
            return CurrentGameState.ReadonlyGameState.Defense.Cooldown(CurrentGameState.ReadonlyGameState.PlayerStats);
        return 0;
    }
}