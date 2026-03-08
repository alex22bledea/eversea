using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    [SerializeField] private UpgradablePartSO upgradablePart;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI UpgradeNameText;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;

    private void Start()
    {
        UpdateUI();

        button.onClick.AddListener(() =>
        {
            UpgradeSystem.Instance.TryToUpgrade(upgradablePart);
        });
    }

    private void OnEnable()
    {
        upgradablePart.OnCurrentUpgradeChanged += UpdateUI;
        if (upgradablePart.RequirementPart != null)
            upgradablePart.RequirementPart.OnCurrentUpgradeChanged += UpdateUI;
    }

    private void OnDisable()
    {
        upgradablePart.OnCurrentUpgradeChanged -= UpdateUI;
        if (upgradablePart.RequirementPart != null)
            upgradablePart.RequirementPart.OnCurrentUpgradeChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        #region Name Text

        if (upgradablePart.IsUnlocked)
            UpgradeNameText.text = upgradablePart.Name.ToUpper() + " " + upgradablePart.CurrentUpgradeIndex;
        else
            UpgradeNameText.text = upgradablePart.Name.ToUpper();

        #endregion

        #region Button Text

        string newText;

        if (upgradablePart.IsMaxLevel())
            newText = "MAX LEVEL\nREACHED";
        else if (!upgradablePart.CanUpgrade())
            newText = "REQUIRES\nLEVEL " + upgradablePart.GetNextRequiredLevel() + "\n" + upgradablePart.RequirementPart.Name.ToUpper();
        else if (!upgradablePart.IsUnlocked)
            newText = "UNLOCK FOR\n" + upgradablePart.GetNextUpgradeCost() + " GOLD";
        else
            newText = "UPGRADE FOR\n" + upgradablePart.GetNextUpgradeCost() + " GOLD";

        buttonText.text = newText;

        #endregion
    }
}
