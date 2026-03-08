using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BoatUpgradePart", menuName = "ScriptableObjects/UpgradeablePart/Boat")]
public class BoatUpgradePart : UpgradablePart<BoatUpgrade>
{
}

[Serializable]
public class BoatUpgrade : BaseUpgradeLevel
{
    [field: SerializeField] public float Speed {get; private set;}
    [field: SerializeField] public float Health { get; private set; }
}