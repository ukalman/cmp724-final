using System.Collections.Generic;
using UnityEngine;

public static class RandomUtils
{
    public static T GetWeightedRandom<T>(Dictionary<T, float> weightedItems)
    {
        float totalWeight = 0f;
        foreach (var weight in weightedItems.Values)
            totalWeight += weight;

        float randomValue = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var kvp in weightedItems)
        {
            cumulative += kvp.Value;
            if (randomValue <= cumulative)
                return kvp.Key;
        }

        // Fallback (float precision issue)
        foreach (var kvp in weightedItems)
            return kvp.Key;

        throw new System.Exception("GetWeightedRandom failed: dictionary is empty.");
    }
}