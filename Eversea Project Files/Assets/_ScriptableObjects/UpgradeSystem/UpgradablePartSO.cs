using System;
using UnityEngine;

public abstract class UpgradablePartSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: Space(12)]
    [field: SerializeField] public bool StartsUnlocked { get; private set; }
    public bool IsUnlocked { get; private set; }
    [field: SerializeField] public UpgradablePartSO RequirementPart { get; private set; }

    public event Action OnCurrentUpgradeChanged;

    public int CurrentUpgradeIndex { get; protected set; }

    public void ResetUpgrades()
    {
        if (StartsUnlocked)
        {
            CurrentUpgradeIndex = 0;
            IsUnlocked = true;
        }
        else
        {
            CurrentUpgradeIndex = -1;
            IsUnlocked = false;
        }

        OnCurrentUpgradeChanged?.Invoke();
    }

    /// <summary> Upgrades this part. MUST be checked it can be upgraded beforehand! </summary>
    public virtual void Upgrade()
    {
        CurrentUpgradeIndex++;

        if (!IsUnlocked)
            IsUnlocked = true;

        OnCurrentUpgradeChanged?.Invoke();
    }

    public abstract bool CanUpgrade();

    public abstract float GetNextUpgradeCost();

    public abstract bool IsMaxLevel();

    public abstract int GetNextRequiredLevel();
}
