using System;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class RandomlyDisableNChildren : MonoBehaviour
{
    [SerializeField] private int minDisabled = 0;
    [SerializeField] private int maxDisabled = 1;

    private void Awake()
    {
        var totalChildren = transform.childCount;
        var numDisabled = Random.Range(minDisabled, Math.Min(maxDisabled + 1, totalChildren));

        Enumerable.Range(0, totalChildren)
            .ToArray()
            .Shuffled()
            .Take(numDisabled).ForEach(i => transform.GetChild(i).gameObject.SetActive(false));
    }
}
