
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePlayerDock : MonoBehaviour
{
    [SerializeField] private Vector3VariableSO playerPosition;

    [field: Space(12)]
    [field: SerializeField] public bool CanInteractWithHouses { get; private set; }

    [field: Space(12)]
    [field: SerializeField] public bool hasFixedBoatPosition {get; private set;}
    [field: SerializeField] public Vector3 boatPosition { get; private set; }

    [field: Space(12)]
    [field: SerializeField] public bool hasFixedBoatRotation { get; private set; }
    [field: SerializeField] public Vector3[] boatRotationForwordArray { get; private set; }

    [Space(12)]
    [SerializeField] private float bottomImagePositionY;
    [SerializeField] private float topImagePositionY;
    [SerializeField] private float playerAnimStartDistance;
    [SerializeField] private float playerInteractDistance;
    [SerializeField] private AnimationCurve imagePosAnimCurve;
    [SerializeField] private AnimationCurve imageAlphaAnimCurve;

    [Space(12)]
    [SerializeField] private Image dockImage;

    [Space(12)]
    [SerializeField] private Collider dockCollider;

    private bool isActive = true;

    private void FixedUpdate()
    {
        if (isActive)
            HandleDockIconVisibility();
    }

    public void DisableDock()
    {
        dockImage.color = new Color(dockImage.color.r, dockImage.color.g, dockImage.color.b, 0);

        dockCollider.enabled = false;
        isActive = false;
    }

    public void EnableDock()
    {
        dockImage.color = new Color(dockImage.color.r, dockImage.color.g, dockImage.color.b, 0);

        dockCollider.enabled = true;
        isActive = true;
    }

    private void HandleDockIconVisibility()
    {
        float SqDist = Helpers.SqDistance(transform.position, playerPosition.Value);
        float SqPlayerAnimStartDist = playerAnimStartDistance * playerAnimStartDistance;

        if (SqDist > SqPlayerAnimStartDist)
        {
            dockImage.color = new Color(dockImage.color.r, dockImage.color.g, dockImage.color.b, 0);
            dockImage.rectTransform.localPosition = new Vector3(0, bottomImagePositionY, 0);

            return;
        }

        float SqPlayerInteractDist = playerInteractDistance * playerInteractDistance;

        if (SqDist < SqPlayerInteractDist)
        {
            dockImage.color = new Color(dockImage.color.r, dockImage.color.g, dockImage.color.b, 1);
            dockImage.rectTransform.localPosition = new Vector3(0, topImagePositionY, 0);

            return;
        }

        float percentage = 1 - (SqDist - SqPlayerInteractDist) / (SqPlayerAnimStartDist - SqPlayerInteractDist);

        dockImage.color = new Color(dockImage.color.r, dockImage.color.g, dockImage.color.b, imageAlphaAnimCurve.Evaluate(percentage));
        dockImage.rectTransform.localPosition = new Vector3(0, bottomImagePositionY + (topImagePositionY - bottomImagePositionY) * imagePosAnimCurve.Evaluate(percentage), 0);
    }

    public Vector3 GetClosestBoatRotationForward(Vector3 boatForward)
    {
        if (boatRotationForwordArray.Length == 0)
            return Vector3.zero;

        Vector3 best = boatRotationForwordArray[0];

        for (int i = 1; i < boatRotationForwordArray.Length; i++)
        {
            float dot1 = Vector3.Dot(best, boatForward);
            float dot2 = Vector3.Dot(boatRotationForwordArray[i], boatForward);

            if (dot2 > dot1)
                best = boatRotationForwordArray[i];
        }

        return best;
    }

    public abstract void HandleDock();
}
