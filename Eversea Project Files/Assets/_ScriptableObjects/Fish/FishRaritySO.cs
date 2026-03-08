using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new FishRarity", menuName = "ScriptableObjects/FishRarity")]
[Serializable]
public class FishRaritySO : ScriptableObject
{
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public Color Color { get; private set; }
}
