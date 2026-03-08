using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Fish", menuName = "ScriptableObjects/Fish")]
public class FishSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public List<SelectedFishRarityChance> rarities { get; private set; }

    public FishRaritySO GetRandomRarity()
    {
        if (rarities.Count == 0)
        {
            Debug.LogError("Rarities list is empty!");
            return null;
        }

        float c = 0f;
        float random = UnityEngine.Random.value;
        for (int i = rarities.Count - 1; i > 0; i--)
        {
            c += rarities[i].Chance;
            if (random <= c)
                return rarities[i].Rarity;
        }
        return rarities[0].Rarity;
    }
}

[Serializable]
public struct SelectedFishRarityChance
{
    [field: SerializeField] public FishRaritySO Rarity { get; private set; }
    [field:SerializeField] public float Chance { get; private set; }
}