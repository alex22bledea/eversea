using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "ScriptableObjects/Input Reader", fileName = "new InputReader")]
public class InputReaderSO : ScriptableObject, GameInput.IGameplayActions
{
    // Gameplay Actions

    public event Action<Vector2> MoveEvent;
    public event Action AttackEvent;
    public event Action FishEvent;
    public event Action MoveCameraEvent;
    public event Action MoveCameraCancelledEvent;
    public event Action MinigameInteractEvent;
    public event Action StoppedHoldingMinigameInteractEvent;
    public event Action MinigameCancelledEvent;
    public event Action GamePausedEvent;
    public event Action OpenUpgradeMenuEvent;
    public event Action OpenInventoryEvent;
    public event Action BackOrExitEvent;
    public event Action UndockEvent;
    public event Action DiscardItemEvent;
    public event Action InteractEvent;
    public event Action PlaceOrPickupItemEvent;
    public event Action RotateItemEvent;
    public event Action SellItemEvent;

    private GameInput gameInput;

    // Bindings

    public enum Binding
    {
        MoveUp,
        MoveDown,
        SteerLeft,
        SteerRight,
        MoveCamera,
        Fish,
        Attack,
        PauseGame,
        OpenInventory,
        BackOrExit,
        Undock,
        DiscardItem,
        Interact,
        PlaceOrPickupItem,
        RotateItem,
        SellItem
    }

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Gameplay.SetCallbacks(this);
        }

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) // Using the saved rebindings
            gameInput.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));

        gameInput.Gameplay.Enable();

        Debug.Log("input enabled");
    }

    private void OnDisable()
    {
        if (gameInput != null)
            gameInput.Gameplay.Disable();

        Debug.Log("input disabled");
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            case Binding.MoveUp:
                return gameInput.Gameplay.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return gameInput.Gameplay.Move.bindings[2].ToDisplayString();
            case Binding.SteerLeft:
                return gameInput.Gameplay.Move.bindings[3].ToDisplayString();
            case Binding.SteerRight:
                return gameInput.Gameplay.Move.bindings[4].ToDisplayString();
            case Binding.MoveCamera:
                return gameInput.Gameplay.MoveCamera.bindings[0].ToDisplayString();
            case Binding.Fish:
                return gameInput.Gameplay.Fish.bindings[0].ToDisplayString();
            case Binding.Attack:
                return gameInput.Gameplay.Attack.bindings[0].ToDisplayString();
            case Binding.PauseGame:
                return gameInput.Gameplay.PauseGame.bindings[0].ToDisplayString();
            case Binding.OpenInventory:
                return gameInput.Gameplay.OpenInventory.bindings[0].ToDisplayString();
            case Binding.DiscardItem:
                return gameInput.Gameplay.DiscardItem.bindings[0].ToDisplayString();
            case Binding.Undock:
                return gameInput.Gameplay.Undock.bindings[0].ToDisplayString();
            case Binding.BackOrExit:
                return gameInput.Gameplay.BackOrExit.bindings[0].ToDisplayString();
            case Binding.Interact:
                return gameInput.Gameplay.Interact.bindings[0].ToDisplayString();
            case Binding.PlaceOrPickupItem:
                return gameInput.Gameplay.PlaceOrPickupItem.bindings[0].ToDisplayString();
            case Binding.RotateItem:
                return gameInput.Gameplay.RotateItem.bindings[0].ToDisplayString();
            case Binding.SellItem:
                return gameInput.Gameplay.SellItem.bindings[0].ToDisplayString();
            default:
            {
                Debug.LogError("unknow switch statement: " + binding);
                return default;
            }
        }
    }

    public void RebindBinding(Binding binding, Action onRebindingFinished)
    {
        gameInput.Gameplay.Disable();

        InputAction inputAction = default;
        int inputIndex = default;

        switch (binding)
        {
            case Binding.MoveUp:
            {
                inputAction = gameInput.Gameplay.Move;
                inputIndex = 1;
                break;
            }
            case Binding.MoveDown:
            {
                inputAction = gameInput.Gameplay.Move;
                inputIndex = 2;
                break;
            }
            case Binding.SteerLeft:
            {
                inputAction = gameInput.Gameplay.Move;
                inputIndex = 3;
                break;
            }
            case Binding.SteerRight:
            {
                inputAction = gameInput.Gameplay.Move;
                inputIndex = 4;
                break;
            }
            case Binding.MoveCamera:
            {
                inputAction = gameInput.Gameplay.MoveCamera;
                inputIndex = 0;
                break;
            }
            case Binding.Fish:
            {
                inputAction = gameInput.Gameplay.Fish;
                inputIndex = 0;
                break;
            }
            case Binding.Attack:
            {
                inputAction = gameInput.Gameplay.Attack;
                inputIndex = 0;
                break;
            }
            case Binding.PauseGame:
            {
                inputAction = gameInput.Gameplay.PauseGame;
                inputIndex = 0;
                break;
            }
            case Binding.OpenInventory:
            {
                inputAction = gameInput.Gameplay.OpenInventory;
                inputIndex = 0;
                break;
            }
            case Binding.DiscardItem:
            {
                inputAction = gameInput.Gameplay.DiscardItem;
                inputIndex = 0;
                break;
            }
            case Binding.Undock:
            {
                inputAction = gameInput.Gameplay.Undock;
                inputIndex = 0;
                break;
            }
            case Binding.BackOrExit:
            {
                inputAction = gameInput.Gameplay.BackOrExit;
                inputIndex = 0;
                break;
            }
            case Binding.Interact:
            {
                inputAction = gameInput.Gameplay.Interact;
                inputIndex = 0;
                break;
            }
            case Binding.PlaceOrPickupItem:
            {
                inputAction = gameInput.Gameplay.PlaceOrPickupItem;
                inputIndex = 0;
                break;
            }
            case Binding.RotateItem:
            {
                inputAction = gameInput.Gameplay.RotateItem;
                inputIndex = 0;
                break;
            }
            case Binding.SellItem:
            {
                inputAction = gameInput.Gameplay.SellItem;
                inputIndex = 0;
                break;
            }
            default:
            {
                Debug.LogError("unknow switch statement: " + binding);
                break;
            }
        }

        inputAction.PerformInteractiveRebinding(inputIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                gameInput.Gameplay.Enable();
                onRebindingFinished();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, gameInput.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            AttackEvent?.Invoke();
    }

    public void OnFish(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
            FishEvent?.Invoke();

        if (context.phase == InputActionPhase.Performed)
            MinigameInteractEvent?.Invoke();
        if (context.phase == InputActionPhase.Canceled)
            StoppedHoldingMinigameInteractEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // nothing rn
    }

    public void OnMoveCamera(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MoveCameraEvent?.Invoke();
        if(context.phase == InputActionPhase.Canceled)
            MoveCameraCancelledEvent?.Invoke();
    }

    public void OnCancellMinigame(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
            MinigameCancelledEvent?.Invoke();
    }

    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            GamePausedEvent?.Invoke();
    }

    public void OnOpenUpgradeMenu(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            OpenUpgradeMenuEvent?.Invoke();
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
            OpenInventoryEvent?.Invoke();
    }

    public void OnBackOrExit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            BackOrExitEvent?.Invoke();
    }

    public void OnUndock(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            UndockEvent?.Invoke();
    }

    public void OnDiscardItem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            DiscardItemEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            InteractEvent?.Invoke();
    }

    public void OnPlaceOrPickupItem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            PlaceOrPickupItemEvent?.Invoke();
    }

    public void OnRotateItem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            RotateItemEvent?.Invoke();
    }

    public void OnSellItem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            SellItemEvent?.Invoke();
    }
}
