using UnityEngine;
using NaughtyAttributes;
using System;

[CreateAssetMenu(fileName = "SideCannonUpgradePart", menuName = "ScriptableObjects/UpgradeablePart/SideCannon")]
public class SideCannonUpgradePart : UpgradablePart<SideCannonUpgrade>
{
}

[Serializable]
public class SideCannonUpgrade : BaseUpgradeLevel
{
    [field: SerializeField] public float Damage { get; private set; }
    [MinMaxSlider(0, 60)] public Vector2 AtkSpeed;
    [field: SerializeField] public float FOV { get; private set; }
    [field: SerializeField] public float Range { get; private set; }
}
