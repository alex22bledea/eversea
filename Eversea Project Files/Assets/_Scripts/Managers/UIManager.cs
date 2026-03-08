using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : Singleton<UIManager>
{
    public event Action<UIWindow> OnWindowChanged;

    [SerializeField] private InputReaderSO input;

    [Space(12)]
    [SerializeField] private GameObject MainInfoUI;
    [SerializeField] private GameObject ShipwrightUI;
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private GameObject FishMerchantUI;
    [SerializeField] private GameObject FishingUI;
    [SerializeField] private GameObject CollectorUI;
    [SerializeField] private GameObject GameOverUI;

    [Space(12)]
    [SerializeField] private GameObject GamePauseUI;
    [SerializeField] private GameObject OptionsUI;

    [Space(12)]
    [SerializeField] private MarketPricesSO fishMerchantMarketPrices;
    [SerializeField] private MarketPricesSO collectorMarketPrices;

    private bool isUpgradeMenuActive;

    private void OnEnable()
    {
        input.GamePausedEvent += ToggleGamePaused;
        input.OpenInventoryEvent += ToggleInventoryOpened;
        input.BackOrExitEvent += BackOrExitWindow;
    }

    private void OnDisable()
    {
        input.GamePausedEvent -= ToggleGamePaused;
        input.OpenInventoryEvent -= ToggleInventoryOpened;
        input.BackOrExitEvent -= BackOrExitWindow;
    }

    public enum UIWindow
    {
        None,
        MainInfo,
        GamePaused,
        Options,
        Shipwright,
        Inventory,
        Fishing,
        FishMerchant,
        Collector,
        GameOver
    }

    public UIWindow activeUIWindow { get; private set; }
    private UIWindow previousUIWindow;

    private bool isOptionsOpened = false;

    private List<GameObject> activeUIs = new List<GameObject>();

    public void ToggleGamePaused()
    {
        if (activeUIWindow == UIWindow.GamePaused)
        {
            if (isOptionsOpened)
                CloseOptionsUI();

            OpenUIWindow(previousUIWindow);
            previousUIWindow = UIWindow.None;
        }
        else
            OpenUIWindow(UIWindow.GamePaused);
    }

    public void ToggleInventoryOpened()
    {
        if (activeUIWindow == UIWindow.Inventory)
            OpenUIWindow(UIWindow.None);
        else if (activeUIWindow == UIWindow.None)
            OpenUIWindow(UIWindow.Inventory);

        /// Otherwise you cannot open/close the inventory
    }

    private void BackOrExitWindow()
    {
        if (!(activeUIWindow == UIWindow.FishMerchant || activeUIWindow == UIWindow.Collector || activeUIWindow == UIWindow.Shipwright))
            return;

        OpenUIWindow(UIWindow.None);
    }

    public void OpenOptionsUI()
    {
        if (activeUIWindow != UIWindow.GamePaused)
            return;

        isOptionsOpened = true;
        OptionsUI.SetActive(true);
        GamePauseUI.SetActive(false);
        activeUIs.Add(OptionsUI);
    }

    public void CloseOptionsUI()
    {
        if ( !activeUIs.Contains(OptionsUI))
            return;

        isOptionsOpened = false;
        OptionsUI.SetActive(false);
        GamePauseUI.SetActive(true);
        activeUIs.Remove(OptionsUI);
    }

    public void OpenUIWindow(UIWindow window)
    {
        List<GameObject> UIsToClose = new List<GameObject>(activeUIs);
        List<GameObject> UIsToOpen = new List<GameObject>();

        switch (window)
        {
            case UIWindow.GamePaused:
            {
                UIsToOpen = new List<GameObject>(activeUIs) { GamePauseUI };

                if(activeUIWindow != UIWindow.GamePaused)
                    previousUIWindow = activeUIWindow;

                break;
            }

            case UIWindow.Shipwright:
            {
                UIsToOpen = new List<GameObject> { MainInfoUI, ShipwrightUI };

                break;
            }

            case UIWindow.None:
            {
                UIsToOpen = new List<GameObject> { MainInfoUI };

                break;
            }

            case UIWindow.Inventory:
            {
                UIsToOpen = new List<GameObject> { MainInfoUI, InventoryUI };

                break;
            }

            case UIWindow.FishMerchant:
            {
                UIsToOpen = new List<GameObject> { MainInfoUI, InventoryUI, FishMerchantUI };

                break;   
            }

            case UIWindow.Fishing:
            {
                UIsToOpen = new List<GameObject> { MainInfoUI, InventoryUI, FishingUI };

                break;
            }

            case UIWindow.Collector:
            {
                UIsToOpen = new List<GameObject> { MainInfoUI, InventoryUI, CollectorUI };

                break;
            }

            case UIWindow.GameOver:
            {
                UIsToOpen = new List<GameObject> { GameOverUI };

                break;
            }

            default:
            {
                Debug.LogError("unknow case in UIManager script: " + window);

                break;
            }
        }

        activeUIWindow = window;

        foreach (GameObject UIGameObject in UIsToClose)
            if (UIsToOpen.Contains(UIGameObject))
            {
                UIsToOpen.Remove(UIGameObject); /// Already opened;
            }
            else
            {
                /// Special cases

                if (UIGameObject == InventoryUI)
                    InventoryManager.Instance.ProperlyCloseInventory();
                else if (UIGameObject == FishingUI)
                {
                    InventoryManager.Instance.ProperlyCloseTemporaryInventory();

                    if (FishingMinigamesManager.Instance.IsPlayingMinigame)
                        FishingMinigamesManager.Instance.CancellMinigame();
                }
                else if (UIGameObject == FishMerchantUI)
                    InventoryManager.Instance.SetCannotSellItems();
                else if(UIGameObject == CollectorUI)
                    InventoryManager.Instance.SetCannotSellItems();
                else if (UIGameObject == GamePauseUI)
                    GameManager.Instance.UnpauseGame();
                else if(UIGameObject == GameOverUI)
                    GameManager.Instance.UnpauseGame();

                /// Closing the UI

                if(UIGameObject == FishingUI)
                    UIGameObject.GetComponent<CanvasGroup>().SetCanvasGroupVisible(false);
                else
                    UIGameObject.SetActive(false);

                activeUIs.Remove(UIGameObject);
            }

        foreach (GameObject UIGameObject in UIsToOpen)
        {
            /// Opening the UI
            if (UIGameObject == FishingUI)
                UIGameObject.GetComponent<CanvasGroup>().SetCanvasGroupVisible(true);
            else
                UIGameObject.SetActive(true);

            activeUIs.Add(UIGameObject);

            /// Special cases

            if (UIGameObject == FishMerchantUI)
                InventoryManager.Instance.SetCanSellItems(fishMerchantMarketPrices);
            if (UIGameObject == CollectorUI)
                InventoryManager.Instance.SetCanSellItems(collectorMarketPrices);
            else if (UIGameObject == GamePauseUI)
                GameManager.Instance.PauseGame();
            else if (UIGameObject == FishingUI)
            {
                FishingMinigamesManager.Instance.PrepareMinigame();
            }
            else if (UIGameObject == GameOverUI)
                GameManager.Instance.PauseGame();
        }

        OnWindowChanged.Invoke(activeUIWindow);
    }
    
}
