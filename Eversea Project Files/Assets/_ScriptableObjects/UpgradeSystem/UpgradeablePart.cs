using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradablePart<T> : UpgradablePartSO where T : BaseUpgradeLevel
{
    [SerializeField] private List<T> upgradeLevelList;

    public override bool CanUpgrade()
    {
        if (RequirementPart != null)
            return !IsMaxLevel() && RequirementPart.CurrentUpgradeIndex >= GetNextUpgrade().LvlReq;
        else
            return !IsMaxLevel();
    }

    public override float GetNextUpgradeCost()
    {
        return GetNextUpgrade().Cost;
    }

    public override bool IsMaxLevel()
    {
        return CurrentUpgradeIndex == upgradeLevelList.Count - 1;
    }

    public T GetCurrentUpgrade() => upgradeLevelList[CurrentUpgradeIndex];
    private T GetNextUpgrade()
    {
        if(!IsMaxLevel())
            return upgradeLevelList[CurrentUpgradeIndex + 1];
        return null;
    }

    public override int GetNextRequiredLevel() => GetNextUpgrade().LvlReq;

    [ContextMenu("Debug Upgrade Index")]
    private void DebugUpgradeIndex()
    {
        Debug.Log(CurrentUpgradeIndex);
    }
}

[Serializable]
public class BaseUpgradeLevel
{
    [field: SerializeField] public float Cost { get; private set; }
    [field: SerializeField] public int LvlReq { get; private set; } /// OBS: lvl req index starts from 0 !
}
