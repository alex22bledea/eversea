using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MainCannonUpgradePart", menuName = "ScriptableObjects/UpgradeablePart/MainCannon")]
public class MainCannonUpgradePart : UpgradablePart<MainCannonUpgrade>
{
}

[Serializable]
public class MainCannonUpgrade : BaseUpgradeLevel
{
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float AtkSpeed { get; private set; }
}