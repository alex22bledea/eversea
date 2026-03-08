
using Cinemachine;
using UnityEngine;

public class PortPlayerDock : BasePlayerDock
{
    [Space(12)]
    [SerializeField] private CinemachineVirtualCamera Camera;

    public override void HandleDock()
    {
        UIManager.Instance.OpenUIWindow(UIManager.UIWindow.None);
        CameraManager.Instance.SetCamera(Camera);
    }
}
