using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputReaderSO input;

    [Space(12)]
    [SerializeField] private VoidEventChannel NightEndedEventChannel;

    [Space(12), Header("Docking")]
    [SerializeField] private float playerDockInteractRadius;
    [SerializeField] private LayerMask playerDockLayerMask;

    [field: Space(12)]
    [field: SerializeField] public BoxCollider ShootCollider { get; private set; }

    [Space(12), Header("VariableSOs")]
    [SerializeField] private PlayerReference playerRef;
    [SerializeField] private Vector3VariableSO playerPos;
    [SerializeField] private FloatVariableSO boatSpeedMultiplier;

    [Space(12)]
    [SerializeField] private BoatUpgradePart boatUpgrades;
    [SerializeField] private FloatVariableSO playerMaxHealth;

    // Components
    private Crest.CustomControlledBoat movement;

    private Rigidbody rigidBody;

    private HealthSystem playerHealthSystem;

    #region Docking

    public bool IsDocked { get; private set; }
    private bool isDocking = false;
    public bool CanInteractWithHouses { get; private set; }

    #endregion

    private void Awake()
    {
        movement = GetComponent<Crest.CustomControlledBoat>();
        playerRef.SetValue(this);
        rigidBody = GetComponent<Rigidbody>();
        playerHealthSystem = GetComponent<HealthSystem>();

        IsDocked = false;
        CanInteractWithHouses = false;
    }

    private void Start()
    {
        playerHealthSystem.OnDeath += PlayerHealthSystem_OnDeath;
    }

    private void PlayerHealthSystem_OnDeath()
    {
        UIManager.Instance.OpenUIWindow(UIManager.UIWindow.GameOver);
    }

    private void OnEnable()
    {
        input.MoveEvent += SetMoveInputVector;
        input.InteractEvent += TryToDock;
        input.UndockEvent += TryToUndock;

        NightEndedEventChannel.OnEventRaised += HealToFull;

        boatUpgrades.OnCurrentUpgradeChanged += BoatUpgrades_OnCurrentUpgradeChanged;
    }

    private void OnDisable()
    {
        input.MoveEvent -= SetMoveInputVector;
        input.InteractEvent -= TryToDock;
        input.UndockEvent -= TryToUndock;

        NightEndedEventChannel.OnEventRaised -= HealToFull;

        boatUpgrades.OnCurrentUpgradeChanged -= BoatUpgrades_OnCurrentUpgradeChanged;
    }

    private void HealToFull()
    {
        playerHealthSystem.HealToFull();
    }

    private void FixedUpdate()
    {
        playerPos.SetValue(transform.position);
    }

    private void OnDestroy()
    {
        playerRef.SetValue(null);
    }

    private void BoatUpgrades_OnCurrentUpgradeChanged()
    {
        movement._enginePower = boatUpgrades.GetCurrentUpgrade().Speed * boatSpeedMultiplier.Value;
        playerMaxHealth.SetValue(boatUpgrades.GetCurrentUpgrade().Health);
    }

    private void TryToDock()
    {
        if ( !(UIManager.Instance.activeUIWindow == UIManager.UIWindow.None || UIManager.Instance.activeUIWindow == UIManager.UIWindow.MainInfo))
            return;

        if (IsDocked || isDocking)
            return;

        foreach (Collider col in Physics.OverlapSphere(transform.position, playerDockInteractRadius, playerDockLayerMask))
            if (col.TryGetComponent<BasePlayerDock>(out BasePlayerDock dock))
            {
                StartCoroutine(DockBoat(dock));
                // Debug.Log("docked here " + dock, dock);
                break;
            }
    }

    private void TryToUndock()
    {
        if ( !IsDocked || isDocking)
            return;

        if (!(UIManager.Instance.activeUIWindow == UIManager.UIWindow.None || UIManager.Instance.activeUIWindow == UIManager.UIWindow.Fishing))
            return;

        UndockBoat();
        UIManager.Instance.OpenUIWindow(UIManager.UIWindow.None);
    }

    private void SetMoveInputVector(Vector2 moveInput)
    {
        if (IsDocked)
            moveInput = Vector2.zero;

        movement.SetMoveInput(moveInput);
    }

    private IEnumerator DockBoat(BasePlayerDock dock)
    {
        IsDocked = true;
        CanInteractWithHouses = false;
        isDocking = true;

        rigidBody.constraints = RigidbodyConstraints.FreezePosition;
        //collider.enabled = false;
        movement.enabled = false;

        dock.HandleDock();

        /// Handle moving the boat
        if (dock.hasFixedBoatPosition || dock.hasFixedBoatRotation)
        {
            Vector3 startPos = transform.position;
            Quaternion startRotation = transform.rotation;

            Vector3 endPos = dock.hasFixedBoatPosition ? dock.boatPosition : startPos;
            Quaternion endRotation = dock.hasFixedBoatRotation ? Quaternion.LookRotation(dock.GetClosestBoatRotationForward(transform.forward)) : startRotation;

            float animDuration = 1;
            float time = 0;

            while (time < 1)
            {
                transform.position = Vector3.Lerp(startPos, endPos, time);
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
                time += Time.deltaTime / animDuration;
                yield return null;
            }

            transform.position = endPos;
            transform.rotation = endRotation;
        }

        CanInteractWithHouses = dock.CanInteractWithHouses;

        rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        isDocking = false;
        //collider.enabled = true;
        movement.enabled = true;
    }

    private void UndockBoat()
    {
        rigidBody.constraints = RigidbodyConstraints.None;
        IsDocked = false;
        CanInteractWithHouses = false;

        CameraManager.Instance.SetMainCamera();
    }

    [ContextMenu("Show Forward")]
    private void ShowForward()
    {
        Debug.Log(transform.forward);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position +  transform.forward * 100f);
    }
}
