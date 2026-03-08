
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private InputReaderSO input;
    [SerializeField] private InputActionReference lookAction;

    [Space(12)]
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private CustomCameraInput freeLookCameraInputProvider;

    [SerializeField] private CinemachineVirtualCameraBase overTheBoatCamera;

    private CinemachineVirtualCameraBase selectedCamera;

    //private bool canMoveCam = false;

    private void OnEnable()
    {
        input.MoveCameraEvent += SetCamMoveCameraTrue;
        input.MoveCameraCancelledEvent += SetCamMoveCameraFalse;
    }

    private void OnDisable()
    {
        input.MoveCameraEvent -= SetCamMoveCameraTrue;
        input.MoveCameraCancelledEvent -= SetCamMoveCameraFalse;
    }

    private void Start()
    {
        freeLookCamera.Priority = 20;
        selectedCamera = freeLookCamera;
    }

    public void SetCamera(CinemachineVirtualCamera camera)
    {
        selectedCamera.Priority = 10;
        camera.Priority = 20;
        selectedCamera = camera;
    }

    public void SetOverTheBoatCamera()
    {
        selectedCamera.Priority = 10;
        overTheBoatCamera.Priority = 20;
        selectedCamera = overTheBoatCamera;
    }

    private void SetCamMoveCameraTrue()
    {
        //canMoveCam = true;
        freeLookCameraInputProvider.SetCanMove(true);
    }

    private void SetCamMoveCameraFalse()
    {
        //canMoveCam = false;
        freeLookCameraInputProvider.SetCanMove(false);
    }

    public void SetMainCamera()
    {
        selectedCamera.Priority = 10;
        freeLookCamera.Priority = 20;
        selectedCamera = freeLookCamera;
    }
}
