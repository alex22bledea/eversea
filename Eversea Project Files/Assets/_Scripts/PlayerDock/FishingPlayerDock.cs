
using UnityEngine;

public class FishingPlayerDock : BasePlayerDock
{
    [Space(12)]
    [SerializeField] private FishingSpot fishingSpot;

    public override void HandleDock()
    {
        FishingMinigamesManager.Instance.SetSelectedFishingSpot(fishingSpot);
        UIManager.Instance.OpenUIWindow(UIManager.UIWindow.Fishing);
        CameraManager.Instance.SetOverTheBoatCamera();
    }
}
