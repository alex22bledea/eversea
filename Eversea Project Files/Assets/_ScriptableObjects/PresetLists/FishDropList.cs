using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new FishDropList", menuName = "ScriptableObjects/PresetLists/FishDrop")]
public class FishDropList : PresetListSO<FishDrop>
{
    public (FishSO fish, FishRaritySO rarity) GetRandomFishWithRarity()
    {
        /// 
        FishSO fish = GetRandomFish();
        FishRaritySO rarity = fish.GetRandomRarity();
        return (fish, rarity);
    }

    private FishSO GetRandomFish()
    {
        if (Items.Count == 0)
        {
            Debug.LogError("Fish Drop List is empty!");
            return null;
        }

        float c = 0f;
        float random = UnityEngine.Random.value;
        for (int i = Items.Count - 1; i > 0; i--)
        {
            c += Items[i].Chance;
            if (random <= c)
                return Items[i].Fish;
        }
        return Items[0].Fish;
    }
}

[Serializable]
public struct FishDrop
{
    [field: SerializeField] public FishSO Fish { get; private set; }
    [field: SerializeField] public float Chance { get; private set; }
}
