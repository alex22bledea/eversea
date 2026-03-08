using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : Singleton<UpgradeSystem>
{
    [SerializeField] private float startingMoney = 0;

    [SerializeField] private FloatVariableSO moneyMultiplier;

    public float Money { get; private set; }

    [SerializeField] public List<UpgradablePartSO> upgradablePartList;

    private void Start()
    {
        ResetAllUpgradableParts();
        Money = startingMoney;
    }

    private void ResetAllUpgradableParts()
    {
        foreach (UpgradablePartSO upgradablePart in upgradablePartList)
        {
            upgradablePart.ResetUpgrades();
        }
    }

    public void TryToUpgrade(UpgradablePartSO upgradablePart)
    {
        UpgradablePartSO match = upgradablePartList.Find((x) => x == upgradablePart);

        if (match == null)
        {
            Debug.LogError("Upgradable part not found in the list");
            return;
        }

        if (upgradablePart.IsMaxLevel())
            return;

        float upgradeCost = upgradablePart.GetNextUpgradeCost();
        if (Money >= upgradeCost && upgradablePart.CanUpgrade())
        {
            Money -= upgradeCost;
            upgradablePart.Upgrade();
        }
        // TODO : Add info box for insufficient money
    }

    public void AddMoney(float value) => Money += value * moneyMultiplier.Value;
}
