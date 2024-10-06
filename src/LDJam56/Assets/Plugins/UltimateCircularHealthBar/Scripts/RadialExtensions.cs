using UnityEngine;
using System.Collections;
using RengeGames.HealthBars;

public static class RadialSegmentedHealthBarExtensions
{
    public static IEnumerator AnimateHPLoss(this RadialSegmentedHealthBar healthBar, float amountToRemove, float duration)
    {
        float startValue = healthBar.RemoveSegments.Value;
        float endValue = Mathf.Clamp(startValue + amountToRemove, 0, healthBar.SegmentCount.Value);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            healthBar.SetRemovedSegments(currentValue);
            yield return null;
        }

        // Ensure we end up at the exact desired value
        healthBar.SetRemovedSegments(endValue);
    }

    public static IEnumerator AnimateOneSegmentLoss(this RadialSegmentedHealthBar healthBar)
    {
        float startValue = healthBar.RemoveSegments.Value;
        float endValue = Mathf.Min(startValue + 1, healthBar.SegmentCount.Value);
        float duration = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            healthBar.SetRemovedSegments(currentValue);
            yield return null;
        }

        // Ensure we end up at the exact desired value
        healthBar.SetRemovedSegments(endValue);
    }

    public static IEnumerator AnimateChange(this RadialSegmentedHealthBar healthBar, float maxHp, float currentHp, float duration = 1.5f)
    {
        float startSegmentCount = healthBar.SegmentCount.Value;
        float startRemovedSegments = healthBar.RemoveSegments.Value;
        
        float targetSegmentCount = maxHp;
        float targetRemovedSegments = maxHp - currentHp;
        
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            float currentSegmentCount = Mathf.Lerp(startSegmentCount, targetSegmentCount, t);
            float currentRemovedSegments = Mathf.Lerp(startRemovedSegments, targetRemovedSegments, t);
            
            healthBar.SetSegmentCount(Mathf.RoundToInt(currentSegmentCount));
            healthBar.SetRemovedSegments(currentRemovedSegments);
            
            yield return null;
        }

        // Ensure we end up at the exact desired values
        healthBar.SetSegmentCount(Mathf.RoundToInt(targetSegmentCount));
        healthBar.SetRemovedSegments(targetRemovedSegments);
    }
}
