
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingUI : MonoBehaviour
{
    [SerializeField] private Image ItemPreviewImage;

    [Space(12)]
    [SerializeField] private Image noFishAvailableImage;
    [SerializeField] private TextMeshProUGUI cannotCatchFishText;

    [Space(12)]
    [SerializeField] private TextMeshProUGUI StockText;
    private readonly string STOCK = "Stock: ";

    [Space(12)]
    [SerializeField] private GameObject fishActionButton;
    [SerializeField] private TextMeshProUGUI fishActionButtonText;

    private enum StockTypes
    {
        Depleted,
        Low,
        Medium,
        High,
    }

    private void Start()
    {
        FishingMinigamesManager.Instance.OnFishingGamePrepared += FishingMinigamesManager_OnFishingGamePrepared;
        FishingMinigamesManager.Instance.OnFishingGameStarted += FishingMinigamesManager_OnFishingGameStarted;
        FishingMinigamesManager.Instance.OnFishingMinigameEnded += FishingMinigamesManager_OnFishingGameEnded;
    }

    private void FishingMinigamesManager_OnFishingGameStarted()
    {
        fishActionButtonText.text = "Pull";
    }

    private void FishingMinigamesManager_OnFishingGameEnded()
    {
        fishActionButtonText.text = "Start fishing";
    }

    private void FishingMinigamesManager_OnFishingGamePrepared()
    {
        if (FishingMinigamesManager.Instance.CurrentItem != null)
        {
            ItemPreviewImage.gameObject.SetActive(true);
            noFishAvailableImage.gameObject.SetActive(false);
            cannotCatchFishText.gameObject.SetActive(false);

            ItemPreviewImage.sprite = FishingMinigamesManager.Instance.CurrentItem.Icon;

            int availableFish = FishingMinigamesManager.Instance.GetSelectedFishingSpotAvailableFish();
            if (availableFish <= 2)
                StockText.text = STOCK + StockTypes.Low.ToString();
            else if(availableFish <= 3)
                StockText.text = STOCK + StockTypes.Medium.ToString();
            else
                StockText.text = STOCK + StockTypes.High.ToString();
            StockText.color = Color.white;

            fishActionButton.SetActive(true);
        }
        else
        {
            ItemPreviewImage.gameObject.SetActive(false);
            noFishAvailableImage.gameObject.SetActive(true);
            cannotCatchFishText.gameObject.SetActive(true);

            StockText.text = STOCK + StockTypes.Depleted.ToString();
            StockText.color = Color.red;

            fishActionButton.SetActive(false);
        }
    }
}
