using System;
using UnityEngine;

public class HoldBarMinigame : BaseFishingMinigame
{
    [Header("Components")]
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform box;
    [SerializeField] private RectTransform target;

    [Header("Values")]

    [SerializeField] private float targetHalfHeight;

    [Serializable]
    private class DictionaryOfFishRarityAndCircleSetup : SerializableDictionary<FishRaritySO, HoldBarSetupSO> { };

    [SerializeField] private DictionaryOfFishRarityAndCircleSetup fishRarityToSetup;

    [SerializeField] private FishRaritySO baseFishRarity;

    [Header("Test")]

    [SerializeField] private RectTransform line1;
    [SerializeField] private RectTransform line2;
    [SerializeField] private RectTransform line3;

    private HoldBarSetupSO setup = default;

    #region Target Behaviour

    private bool isWaiting;
    private float waitTimer;
    private float lastY;
    private float destY;
    private float targetSpeed;

    #endregion

    #region Box Behaviour

    private float boxSpeed;
    private bool isMovingBox;

    private bool _inRange;
    private bool InRange {
        get { return _inRange; }
        set {
            if (value != _inRange)
                compMeter.SetMultiplier(value ? setup.inRangeMult : 1f);
            _inRange = value;
        }
    }

    #endregion

    private void OnEnable()
    {
        input.MinigameInteractEvent += EnableMoveBox;
        input.StoppedHoldingMinigameInteractEvent += DisableIsMovingBox;
    }

    private void OnDisable()
    {
        input.MinigameInteractEvent -= EnableMoveBox;
        input.StoppedHoldingMinigameInteractEvent -= DisableIsMovingBox;
    }

    private void EnableMoveBox() => isMovingBox = true;
    private void DisableIsMovingBox() => isMovingBox = false;

    private void Update()
    {
        if (isStarted)
        {
            MoveBox();

            MoveTarget();

            InRange = Mathf.Abs(box.localPosition.y - target.localPosition.y) <= setup.boxRange;
        }
        else
            InRange = false;
    }

    private void MoveBox()
    {
        float boxDir = isMovingBox ? 1 : -1;
        boxSpeed = Mathf.Lerp(boxSpeed, setup.boxMaxSpeed * boxDir, setup.boxAccel * Time.deltaTime);
        float upperLim = setup.height / 2 - setup.boxHalfHeight;
        float lowerLim = -setup.height / 2 + setup.boxHalfHeight;
        box.SetLocalY(Mathf.Clamp(box.localPosition.y + boxSpeed * Time.deltaTime, lowerLim, upperLim));

        if (box.localPosition.y == lowerLim || box.localPosition.y == upperLim)
            boxSpeed = 0f;

        line1.localPosition = box.localPosition + new Vector3(0f, setup.boxRange);
        line2.localPosition = box.localPosition + new Vector3(0f, -setup.boxRange);
    }

    private void MoveTarget()
    {
        waitTimer -= Time.deltaTime;

        if (!isWaiting)
        {
            float dir = target.localPosition.y < destY ? 1 : -1;
            targetSpeed = Mathf.Lerp(targetSpeed, dir * setup.targetMaxSpeed, setup.targetAccel * Time.deltaTime);
            target.SetLocalY(Mathf.Clamp(target.localPosition.y + targetSpeed * Time.deltaTime, -setup.height / 2 + targetHalfHeight, setup.height / 2 - targetHalfHeight));

            float y = target.localPosition.y;
            if (destY >= Mathf.Min(lastY, y) && destY <= Mathf.Max(lastY, y)) // Destination Reached
                ChooseTargetBehaviour();

            lastY = y;
        }
        else if (waitTimer < 0f)
        {
            ChooseTargetBehaviour();
        }

        line3.localPosition = target.localPosition;
    }

    private void ChooseTargetBehaviour()
    {
        float rand = UnityEngine.Random.value;
        if (rand < 0.4f)
        {
            isWaiting = true;
            targetSpeed = 0f;

            waitTimer = UnityEngine.Random.Range(setup.waitTime.x, setup.waitTime.y);
        }
        else
        {
            isWaiting = false;
            targetSpeed /= 2;

            float minDist = setup.travelDist.x;
            float distToTop = setup.height / 2 - target.localPosition.y - targetHalfHeight;
            float distToBot = setup.height / 2 + target.localPosition.y - targetHalfHeight;

            float dirSgn;
            if (distToBot < minDist)
                dirSgn = 1;
            else if (distToTop < minDist)
                dirSgn = -1;
            else dirSgn = Mathf.Sign(UnityEngine.Random.Range(-1f, 1f));

            float dist = UnityEngine.Random.Range(setup.travelDist.x, setup.travelDist.y);

            destY = Mathf.Clamp(target.localPosition.y + dirSgn * dist, -setup.height / 2 + targetHalfHeight, setup.height / 2 - targetHalfHeight);

            lastY = target.localPosition.y;
        }
    }

    public override void PrepareMinigame()
    {
        setup = fishRarityToSetup[baseFishRarity];

        base.PrepareMinigame();

        background.sizeDelta = new Vector2(background.sizeDelta.x, setup.height);

        target.SetLocalY(setup.height / 3f);
        targetSpeed = 0f;

        box.SetLocalY(-setup.height / 3f);
        box.sizeDelta = new Vector2(45f, 2 * setup.boxHalfHeight);
        boxSpeed = setup.boxMaxSpeed / 3;

        InRange = false;

        DisableIsMovingBox();
    }

    public override void StartMinigame()
    {
        base.StartMinigame();

        ChooseTargetBehaviour();
    }
}
